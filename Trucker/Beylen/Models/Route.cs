using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drums;
using System.Linq;

namespace Beylen.Models
{
	public class Route
	{
		//static ClassRef @class = new ClassRef(typeof(Route));

		public Route() : this(DateOnly.Today) { }

		public Route(DateOnly date)
		{
			//Debug.EnableTracing(@class);

			Date = date;
			Stops = new ObservableCollection<RouteStop>();
		}

		public ObservableCollection<RouteStop> Stops { get; }

		public DateOnly Date { get; }

		public bool DeleteStopOnCompleted { get; set; } = true;
		public bool DirectionsForCurrentOnly { get; set; } = true;

		public void Clear()
		{
			Stops.Clear();
		}

		public bool HasCustomerStop(Customer customer)
		{
			foreach (var stop in Stops) {
				if (stop is CustomerStop customerStop) {
					if (customerStop.Customer == customer)
						return true;
				}
			}
			return false;
		}

		public bool HasEndPoint {
			get {
				if (Stops.Count > 0) {
					var lastStop = Stops[Stops.Count-1];
					return lastStop.Kind == RouteStopKind.EndPoint;
				} else {
					return false;
				}
			}
		}

		public async Task AddEndPoint()
		{
			if (Stops.Count > 0) {
				var lastStop = Stops[Stops.Count-1];
				if (lastStop.Kind != RouteStopKind.EndPoint) {
					await AddStop(new RouteEndStop { Ordinal = lastStop.Ordinal + 1 }, addToStorage: true);
				}
			}
		}

		public async Task AddNew(RouteStop stop)
		{
			await Add(stop, true);
		}

		public async Task Add(RouteStop stop, bool addToStorage)
		{
			if (Stops.Count > 0) {
				var last = Stops[Stops.Count-1];
				stop.Ordinal = last.Ordinal + 1;
			} else if (addToStorage && stop.Kind != RouteStopKind.StartPoint) { 
				await AddStop(new RouteStartStop(), addToStorage);
				stop.Ordinal = 1;
			} else {
				stop.Ordinal = 0;
			}
			
			await AddStop(stop, addToStorage);
		}

		async Task AddStop(RouteStop stop, bool addToStorage)
		{
			if (addToStorage)
				await AppStorage.Instance.AddRouteStop(stop);

			if (stop.Kind != RouteStopKind.EndPoint && HasEndPoint) {
				Stops.Insert(Stops.Count-1, stop);
			} else {
				Stops.Add(stop);
			}
		}

		public async Task DeleteStop(RouteStop stop)
		{
			int ord = stop.Ordinal;
			int index = Stops.IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.DeleteStop(): Stop #{ord} not found in the Route");
			}

			await AppStorage.Instance.DeleteRouteStop(stop);
			this.Stops.Remove(stop);

			if (ord > 0 && stop.Status == RoutеStopStatus.Pending) {
				while (index < Stops.Count) {
					stop = Stops[index++];
					stop.Ordinal = ord++;
					await AppStorage.Instance.ChangeRouteStopOrdinal(stop);
				}
			}
		}


		bool CanShowDirections(RouteStop stop, out RouteStop prev, bool currentOnly)
		{
			int index = Stops.IndexOf(stop);
			if (index > 0) {
				prev = Stops[index-1];
			} else {
				prev = null;
			}

			if (string.IsNullOrWhiteSpace(stop.Address) || stop.Status >= RoutеStopStatus.Arrived)
				return false;

			if (stop.Status == RoutеStopStatus.Enroute)
				return true;
			if (prev == null || (currentOnly && prev.Status < RoutеStopStatus.Arrived))
				return false;

			return !string.IsNullOrWhiteSpace(prev.Address);
		}

		public bool CanShowDirections(RouteStop stop)
		{
			RouteStop prev;
			return CanShowDirections(stop, out prev, DirectionsForCurrentOnly);
		}

		public async Task ShowDirections(RouteStop stop)
		{
			RouteStop prev;
			if (!CanShowDirections(stop, out prev, DirectionsForCurrentOnly))
				return;

			if (stop.Status == RoutеStopStatus.Enroute) {
				await Maps.MapApplication.OpenDirections(
					null,
					new Location { Address = stop.Address },
					Maps.DefaultOptions);
			} else {
				await Maps.MapApplication.OpenDirections(
					new Location { Address = prev.Address },
					new Location { Address = stop.Address },
					Maps.DefaultOptions);
			}
		}

		public async Task ShowRouteMap()
		{
			var locations = new List<ILocation>();

			RouteStop lastStop = null;
			foreach (var stop in Stops) {
				if (stop.Status == RoutеStopStatus.Departed)
					continue;

				if (stop.Status == RoutеStopStatus.Enroute)
					locations.Add(await Location.GetCurrentLocation());

				locations.Add(new Location { Address = stop.Address });
				lastStop = stop;
			}

			if (locations.Count == 0)
				return;

			if (lastStop.Kind != RouteStopKind.EndPoint) {
				lastStop = new RouteEndStop();
				locations.Add(new Location { Address = lastStop.Address });
			}

			await Maps.MapApplication.OpenDirections(locations, Maps.DefaultOptions);
		}

		public async Task Arrive(RouteStop stop)
		{
			int index = Stops.IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.Arrive(): unknown stop #{stop.Ordinal}");
				return;
			}
			if (stop.Status != RoutеStopStatus.Enroute) {
				Debug.Print($"### Route.Arrive(): stop #{stop.Ordinal} not in Enroute state ({stop.Status})");
				return;
			}

			await ChangeStatus(stop, RoutеStopStatus.Arrived);

			if (index < Stops.Count-1) {
				// Trigger next RouteStopCardModel.UpdateFromSource();
				var next = Stops[index+1];
				next.SetStatus(RoutеStopStatus.Pending, forceNotification: true);
			}
			else if (stop.Kind == RouteStopKind.EndPoint && DeleteStopOnCompleted) {
				await DeleteStop(stop);
			}
		}

		public async Task Depart(RouteStop stop)
		{
			Debug.Assert(stop.Status == RoutеStopStatus.Enroute);

			int index = Stops.IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.Depart(): unknown stop #{stop.Ordinal}");
				return;
			}
			if (stop.Status != RoutеStopStatus.Arrived) {
				Debug.Print($"### Route.Depart(): stop #{stop.Ordinal} not in Arrived state ({stop.Status})");
				return;
			}
			if (stop.Kind == RouteStopKind.EndPoint) {
				Debug.Print($"### Route.Depart(): stop #{stop.Ordinal} is end point");
				return;
			}

			if (stop.Kind == RouteStopKind.StartPoint) {
				await AddEndPoint();
			}

			await ChangeStatus(stop, RoutеStopStatus.Departed);

			if (index < Stops.Count-1) {
				var next = Stops[index+1];
				await ChangeStatus(next, RoutеStopStatus.Enroute);
			}

			if (DeleteStopOnCompleted) {
				await DeleteStop(stop);
			}
		}

		public async Task ChangeStatus(RouteStop stop, RoutеStopStatus status)
		{
			stop.Status = status;
			await AppStorage.Instance.ChangeRouteStopStatus(stop);
		}

		public async Task ChangeOrdinals(int startIndex, int startOrdinal)
		{
			for (int i = startIndex, ordinal = startOrdinal; i < Stops.Count; i++) {
				var stop = Stops[i];
				stop.Ordinal = ordinal++;
				await AppStorage.Instance.ChangeRouteStopOrdinal(stop);
			}
		}
	}
}
