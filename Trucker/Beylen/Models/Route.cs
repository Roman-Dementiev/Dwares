using Dwares.Drums;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

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
			Stops.CollectionChanged += (s, e) => ResetLegs();

		}
		public DateOnly Date { get; }

		public Place StartPoint { get; set; }
		public Place EndPoint { get; set; }

		public ObservableCollection<RouteStop> Stops { get; }
		private List<RouteLeg> CurrentLegs { get; set; }
		private List<RouteLeg> PreviousLegs { get; set; }


		public bool DeleteStopOnCompleted { get; set; } = false;
		public bool DirectionsForCurrentOnly { get; set; } = true;

		public void Clear()
		{
			Stops.Clear();
			CurrentLegs = null;
			PreviousLegs = null;
			IsStarted = false;
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
					var lastStop = Stops[Stops.Count - 1];
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
					await AddStop(new RouteEndStop(this) { Ordinal = lastStop.Ordinal + 1 }, addToStorage: true);
				}
			}
		}

		public async Task AddNew(RouteStop stop)
		{
			await Add(stop, true);
			await CalculateDurations();
		}

		public async Task Add(RouteStop stop, bool addToStorage)
		{
			if (Stops.Count > 0) {
				var last = Stops[Stops.Count-1];
				stop.Ordinal = last.Ordinal + 1;
			} else if (addToStorage && stop.Kind != RouteStopKind.StartPoint) { 
				await AddStop(new RouteStartStop(this), addToStorage);
				stop.Ordinal = 1;
			} else {
				stop.Ordinal = 0;
			}
			
			await AddStop(stop, addToStorage);
		}

		async Task AddStop(RouteStop stop, bool addToStorage)
		{
			Debug.Assert(stop.Route == this);

			if (addToStorage)
				await AppStorage.Instance.AddRouteStop(stop);

			if (Stops.Count > 0 && stop.Kind != RouteStopKind.EndPoint && HasEndPoint) {
				Stops.Insert(Stops.Count-1, stop);
			} else {
				Stops.Add(stop);
			}

			Debug.AssertIsNull(CurrentLegs);
			UpdateLegs();

			Debug.AssertNotNull(stop.Leg);
			RequestLegDuration(stop.Leg);
		}

		public async Task DeleteStop(RouteStop stop)
		{
			int ord = stop.Ordinal;
			int index = Stops.IndexOf(stop);
			if (index < 0) {
				Debug.Print($"### Route.DeleteStop(): Stop #{ord} not found in the Route");
			}

			await AppStorage.Instance.DeleteRouteStop(stop);
			Stops.RemoveAt(index);

			if (Stops.Count == 0) {
				IsStarted = false;
				return;
			}

			if (index < Stops.Count && (stop.Status == RoutеStopStatus.Enroute || stop.Status == RoutеStopStatus.Arrived)) {
				var next = Stops[index];
				await ChangeStopStatus(next, RoutеStopStatus.Enroute);
			}

			if (ord > 0 && stop.Status == RoutеStopStatus.Pending) {
				while (index < Stops.Count) {
					stop = Stops[index++];
					stop.Ordinal = ord++;
					await AppStorage.Instance.ChangeRouteStopOrdinal(stop);
				}
			}

			UpdateETAs();
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
				lastStop = new RouteEndStop(this);
				locations.Add(new Location { Address = lastStop.Address });
			}

			await Maps.MapApplication.OpenDirections(locations, Maps.DefaultOptions);
		}

		public bool IsStarted { get; set; }

		public async Task Start()
		{
			if (IsStarted)
				return;

			IsStarted = true;

			await AddEndPoint();
			UpdateETAs();
			
			await RequestLegDurations(true);
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

			stop.ArrivalTime = DateTime.Now;
			await ChangeStopStatus(stop, RoutеStopStatus.Arrived);
			stop.Leg.Status = RouteLegStatus.Complete;

			if (index < Stops.Count-1) {
				// Trigger next RouteStopCardModel.UpdateFromSource();
				var next = Stops[index+1];
				next.SetStatus(RoutеStopStatus.Pending, forceNotification: true);
			}

			if (stop.Kind == RouteStopKind.EndPoint && DeleteStopOnCompleted) {
				await DeleteStop(stop);
			}

			UpdateETAs();
			await	RequestLegDurations(true);
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
				await Start();
			}

			stop.DepartureTime = DateTime.Now;

			await ChangeStopStatus(stop, RoutеStopStatus.Departed);

			if (index < Stops.Count-1) {
				var next = Stops[index+1];
				await ChangeStopStatus(next, RoutеStopStatus.Enroute);
				next.Leg.Status = RouteLegStatus.Enroute;
			}

			if (DeleteStopOnCompleted) {
				await DeleteStop(stop);
			}

			UpdateETAs();
			await RequestLegDurations(true);
		}

		public async Task ChangeStopStatus(RouteStop stop, RoutеStopStatus status)
		{
			stop.Status = status;
			await AppStorage.Instance.ChangeRouteStopStatus(stop);
		}

		public async Task ChangeStopsOrdinals(int startIndex, int startOrdinal)
		{
			for (int i = startIndex, ordinal = startOrdinal; i < Stops.Count; i++) {
				var stop = Stops[i];
				stop.Ordinal = ordinal++;
				await AppStorage.Instance.ChangeRouteStopOrdinal(stop);
			}
		}

		static RouteLegStatus StopToLegStatus(RouteStop stop)
		{
			switch (stop.Status) {
				case RoutеStopStatus.Pending:
					return RouteLegStatus.Pending;
				case RoutеStopStatus.Enroute:
					return RouteLegStatus.Enroute;
				default:
					return RouteLegStatus.Complete;
			}
		}

		static bool SameLeg(RouteLeg leg, RouteStop start, RouteStop end, RouteLegStatus? status)
		{
			if (leg == null)
				return false;

			return leg.StartPoint == start && leg.EndPoint == end && (status == null || leg.Status == status);

		}

		RouteLeg GetKnownLeg(RouteStop start, RouteStop end, RouteLegStatus status)
		{
			if (PreviousLegs != null)
			{
				foreach (var leg in PreviousLegs) {
					if (SameLeg(leg, start, end, status))
						return leg;
				}
			}

			return null;
		}

		public List<RouteLeg> UpdateLegs()
		{
			if (CurrentLegs == null)
			{
				CurrentLegs = new List<RouteLeg>();

				RouteStop prevStop = null;
				for (int i = 0; i < Stops.Count; i++) {
					var stop = Stops[i];
					var legStatus = StopToLegStatus(stop);
					if (!SameLeg(stop.Leg, prevStop, stop, legStatus))
					{
						stop.Leg = GetKnownLeg(prevStop, stop, legStatus);
						if (stop.Leg == null) {
							stop.Leg = new RouteLeg { 
								StartPoint = prevStop,
								EndPoint = stop,
								Status = legStatus
							};
						}
					}
					CurrentLegs.Add(stop.Leg);

					prevStop = stop;
				}

				PreviousLegs = null;
			}

			return CurrentLegs;
		}

		void ResetLegs()
		{
			if (CurrentLegs != null) {
				PreviousLegs = CurrentLegs;
				CurrentLegs = null;
			}
		}

		public void UpdateETAs()
		{
			DateTime now = DateTime.Now;
			TimeSpan? lastETD = null;

			for (int i = 0; i < Stops.Count; i++)
			{
				var stop = Stops[i];

				switch (stop.Status)
				{
				case RoutеStopStatus.Pending:
					if (lastETD != null && stop.LegDuration != null) {
						stop.ETA = lastETD?.Add((TimeSpan)stop.LegDuration);
						lastETD = stop.ETA?.Add(stop.StopDuration);
					} else {
						stop.ETA = lastETD = null;
					}
					break;
				case RoutеStopStatus.Enroute:
					stop.ETA = stop.LegDuration;
					lastETD = stop.ETA?.Add(stop.StopDuration);
					break;
				case RoutеStopStatus.Arrived:
					stop.ETA = null;
					var etd = stop.ArrivalTime?.Add(stop.StopDuration);
					if (stop.Kind != RouteStopKind.StartPoint && etd != null && etd > now) {
						lastETD = ((DateTime)etd).Subtract(now);
					} else {
						lastETD = TimeSpan.Zero;
					}
					break;
				case RoutеStopStatus.Departed:
					stop.ETA = null;
					lastETD = TimeSpan.Zero;
					break;
				}

				stop.UpdateInfo();
			}
		}

		Queue<RouteLeg> RequestedDurations { get; } = new Queue<RouteLeg>();
		bool calculatingDurations = false;

		public void RequestLegDuration(RouteLeg leg)
		{
			if (leg == null || leg.Status == RouteLegStatus.Complete || RequestedDurations.Contains(leg))
				return;

			RequestedDurations.Enqueue(leg);
		}


		public async Task RequestLegDurations(bool calculate)
		{
			var legs = UpdateLegs();
			foreach (var leg in legs) {
				if (leg.Status != RouteLegStatus.Complete)
					RequestLegDuration(leg);
			}

			if (calculate) {
				await CalculateDurations();
			}
		}


		public async Task CalculateDurations()
		{
			if (calculatingDurations)
				return;

			calculatingDurations = true;
			try {
				var maps = Maps.MapService;

				while (RequestedDurations.Count > 0) {
					var leg = RequestedDurations.Dequeue();
					if (leg.Status == RouteLegStatus.Complete)
						continue;

					var waypoints = new Waypoint[2];
					if (leg.Status == RouteLegStatus.Enroute) {
						var location = await Location.GetCurrentLocation();
						waypoints[0] = Waypoint.FromLocation(WaypointType.SatrtPoint, location);
					} else {
						waypoints[0] = Waypoint.FromAddress(WaypointType.SatrtPoint, leg.StartPoint.Address);
					}
					waypoints[1] = Waypoint.FromAddress(WaypointType.EndPoint, leg.EndPoint.Address);

					var options = new RouteOptions {
						TravelMode = TravelMode.Driving,
						HighwaysRestriction = Restriction.None,
						TollsRestriction = Restriction.Avoid,
						Optimization = Optimization.Default
					};

					var info = await maps.GetRouteInfo(waypoints, options);

					leg.Duration = info.TravelTime;
					UpdateETAs();
				}
			}
			finally {
				calculatingDurations = false;
			}
		}

		public void CancelCalculations()
		{
			RequestedDurations.Clear();
		}
	}
}
