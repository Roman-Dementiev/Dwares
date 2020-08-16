using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	public class Route : ObservableCollection<RouteStop>
	{
		//static ClassRef @class = new ClassRef(typeof(Route));

		public static bool DeleteStopOnDepart { get; set; } = false;

		public Route() : this(DateOnly.Today) { }

		public Route(DateOnly date)
		{
			//Debug.EnableTracing(@class);

			Date = date;
		}

		public DateOnly Date { get; }

		public new Task<Exception> Add(RouteStop stop) => Add(stop, true);
		public async Task<Exception> Add(RouteStop stop, bool addToStorage)
		{
			if (Count > 0) {
				var last = this[Count-1];
				stop.Seq = last.Seq + 1;
			} else if (stop.Kind != RouteStopKind.StartPoint) { 
				var exc = await AddStop(new RouteStartStop(), addToStorage);
				if (exc != null)
					return exc;
				stop.Seq = 1;
			} else {
				stop.Seq = 0;
			}
			
			return await AddStop(stop, addToStorage);
		}

		async Task<Exception> AddStop(RouteStop stop, bool addToStorage = true)
		{
			try {
				if (addToStorage)
					await AppStorage.Instance.AddRouteStop(stop);

				base.Add(stop);
				return null;
			} catch (Exception ex) {
				return ex;
			}
		}

		public async Task<Exception> DeleteStop(RouteStop stop)
		{
			int seq = stop.Seq;
			int index = IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.DeleteStop(): Stop #{seq} not found in the Route");
			}

			try {
				await AppStorage.Instance.DeleteRouteStop(stop);
				this.Remove(stop);

				if (seq > 0 && stop.Status == RouteStatus.Pending) {
					while (index < Count) {
						stop = this[index++];
						stop.Seq = seq++;
						await AppStorage.Instance.ChangeRouteStopSeq(stop);
					}
				}
				return null;
			} catch (Exception exc) {
				return exc;
			}
		}

		public bool HasCustomerStop(Customer customer)
		{
			foreach (var stop in this) {
				if (stop is CustomerStop customerStop) {
					if (customerStop.Customer == customer)
						return true;
				}
			}
			return false;
		}

		public async Task<Exception> Arrive(RouteStop stop)
		{
			int index = IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.Arrive(): unknown stop #{stop.Seq}");
				return null;
			}
			if (stop.Status != RouteStatus.Enroute) {
				Debug.Print($"### Route.Arrive(): stop #{stop.Seq} not in Enroute state ({stop.Status})");
				return null;
			}

			return await ChangeStatus(stop, RouteStatus.Arrived);
		}

		public async Task<Exception> Depart(RouteStop stop)
		{
			Debug.Assert(stop.Status == RouteStatus.Enroute);

			int index = IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.Depart(): unknown stop #{stop.Seq}");
				return null;
			}
			if (stop.Status != RouteStatus.Arrived) {
				Debug.Print($"### Route.Depart(): stop #{stop.Seq} not in Arrived state ({stop.Status})");
				return null;
			}
			if (stop.Kind == RouteStopKind.EndPoint) {
				Debug.Print($"### Route.Depart(): stop #{stop.Seq} is end point");
				return null;
			}

			Exception exc = null;

			if (stop.Kind == RouteStopKind.StartPoint) {
				var last = this[Count-1];
				if (last.Kind != RouteStopKind.EndPoint) {
					exc = await AddStop(new RouteEndStop { Seq = last.Seq + 1});
					if (exc != null)
						return exc;
				}
			}

			exc = await ChangeStatus(stop, RouteStatus.Departed);
			if (exc != null)
				return exc;

			if (index < Count-1) {
				var next = this[index+1];
				exc = await ChangeStatus(next, RouteStatus.Enroute);
			}

			if (DeleteStopOnDepart) {
				exc = await DeleteStop(stop);
			}

			return exc;
		}

		public async Task<Exception> ChangeStatus(RouteStop stop, RouteStatus status)
		{
			try {
				stop.Status = status;
				await AppStorage.Instance.ChangeRouteStopStatus(stop);
				return null;
			}
			catch (Exception exc) {
				return exc;
			}
		}
	}
}
