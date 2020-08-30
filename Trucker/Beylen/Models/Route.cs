﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drums;
using System.Linq;

namespace Beylen.Models
{
	public class Route : ObservableCollection<RouteStop>
	{
		//static ClassRef @class = new ClassRef(typeof(Route));
		const bool DirectionsForCurrentOnly = true;

		public static bool DeleteStopOnCompleted { get; set; } = true;

		public Route() : this(DateOnly.Today) { }

		public Route(DateOnly date)
		{
			//Debug.EnableTracing(@class);

			Date = date;
		}

		public DateOnly Date { get; }

		private new void Add(RouteStop stop) => base.Add(stop);
		public Task<Exception> AddNew(RouteStop stop) => Add(stop, true);

		public async Task<Exception> Add(RouteStop stop, bool addToStorage)
		{
			if (Count > 0) {
				var last = this[Count-1];
				stop.Ordinal = last.Ordinal + 1;
			} else if (addToStorage && stop.Kind != RouteStopKind.StartPoint) { 
				var exc = await AddStop(new RouteStartStop(), addToStorage);
				if (exc != null)
					return exc;
				stop.Ordinal = 1;
			} else {
				stop.Ordinal = 0;
			}
			
			return await AddStop(stop, addToStorage);
		}

		async Task<Exception> AddStop(RouteStop stop, bool addToStorage)
		{
			try {
				if (addToStorage)
					await AppStorage.Instance.AddRouteStop(stop);

				Add(stop);
				return null;
			} catch (Exception ex) {
				return ex;
			}
		}

		public async Task<Exception> DeleteStop(RouteStop stop)
		{
			int ord = stop.Ordinal;
			int index = IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.DeleteStop(): Stop #{ord} not found in the Route");
			}

			try {
				await AppStorage.Instance.DeleteRouteStop(stop);
				this.Remove(stop);

				if (ord > 0 && stop.Status == RouteStatus.Pending) {
					while (index < Count) {
						stop = this[index++];
						stop.Ordinal = ord++;
						await AppStorage.Instance.ChangeRouteStopSeq(stop);
					}
				}
				return null;
			} catch (Exception exc) {
				return exc;
			}
		}


		bool CanShowDirections(RouteStop stop, out RouteStop prev, bool currentOnly)
		{
			int index = IndexOf(stop);
			if (index > 0) {
				prev = this[index - 1];
			} else {
				prev = null;
			}

			if (string.IsNullOrWhiteSpace(stop.Address) || stop.Status >= RouteStatus.Arrived)
				return false;

			if (stop.Status == RouteStatus.Enroute)
				return true;
			if (prev == null || (currentOnly && prev.Status < RouteStatus.Arrived))
				return false;

			return !string.IsNullOrWhiteSpace(prev.Address);
		}

		public bool CanShowDirections(RouteStop stop)
		{
			RouteStop prev;
			return CanShowDirections(stop, out prev, DirectionsForCurrentOnly);
		}

		public async Task<Exception> ShowDirections(RouteStop stop)
		{
			RouteStop prev;
			if (!CanShowDirections(stop, out prev, DirectionsForCurrentOnly))
				return null;

			//Location start = null;
			//if (stop.Status == RouteStatus.Enroute) {
			//	start = await Location.GetCurrentLocation();
			//} else {
			//	start = new Location { Address = prev.Address };
			//}

			try {
				//await Maps.MapApplication.OpenDirections(start, 
				//	new Location { Address = stop.Address },
				//	Maps.DefaultOptions);

				if (stop.Status == RouteStatus.Enroute) {
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
			} catch (Exception exc) {
				return exc;
			}
			return null;
		}

		public async Task<Exception> ShowRouteMap()
		{
			try {
				var locations = new List<ILocation>();

				RouteStop lastStop = null;
				foreach (var stop in this) {
					if (stop.Status == RouteStatus.Departed)
						continue;

					if (stop.Status == RouteStatus.Enroute)
						locations.Add(await Location.GetCurrentLocation());

					locations.Add(new Location { Address = stop.Address });
					lastStop = stop;
				}

				if (locations.Count == 0)
					return null;

				if (lastStop.Kind != RouteStopKind.EndPoint) {
					lastStop = new RouteEndStop();
					locations.Add(new Location { Address = lastStop.Address });
				}

				await Maps.MapApplication.OpenDirections(locations, Maps.DefaultOptions);
			}
			catch (Exception exc) {
				return exc;
			}
			return null;
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
				Debug.Print($"### Route.Arrive(): unknown stop #{stop.Ordinal}");
				return null;
			}
			if (stop.Status != RouteStatus.Enroute) {
				Debug.Print($"### Route.Arrive(): stop #{stop.Ordinal} not in Enroute state ({stop.Status})");
				return null;
			}

			var exc = await ChangeStatus(stop, RouteStatus.Arrived);

			if (index < Count-1) {
				// Trigger next RouteStopCardModel.UpdateFromSource();
				var next = this[index+1];
				next.SetStatus(RouteStatus.Pending, forceNotification: true);
			}
			else if (stop.Kind == RouteStopKind.EndPoint && DeleteStopOnCompleted) {
				exc = await DeleteStop(stop);
			}

			return exc;
		}

		public async Task<Exception> Depart(RouteStop stop)
		{
			Debug.Assert(stop.Status == RouteStatus.Enroute);

			int index = IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.Depart(): unknown stop #{stop.Ordinal}");
				return null;
			}
			if (stop.Status != RouteStatus.Arrived) {
				Debug.Print($"### Route.Depart(): stop #{stop.Ordinal} not in Arrived state ({stop.Status})");
				return null;
			}
			if (stop.Kind == RouteStopKind.EndPoint) {
				Debug.Print($"### Route.Depart(): stop #{stop.Ordinal} is end point");
				return null;
			}

			Exception exc = null;

			if (stop.Kind == RouteStopKind.StartPoint) {
				var last = this[Count-1];
				if (last.Kind != RouteStopKind.EndPoint) {
					exc = await AddStop(new RouteEndStop { Ordinal = last.Ordinal + 1}, true);
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

			if (DeleteStopOnCompleted) {
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
