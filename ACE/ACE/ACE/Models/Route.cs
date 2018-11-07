using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Drums;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public class Route : ObservableCollection<RouteStop>
	{
		static TimeSpan MinStopTime = new TimeSpan(0, 10, 0); // 10 min

		public Route(Contact startPoint)
		{
			StartStop = new RouteStop(RouteStopType.Endpoint, startPoint.Name, startPoint, null, null);
		}
		
		public DateTime? StartTime { get; set; }
		public RouteStop StartStop { get; private set; }
		public RouteStop LastStop => Collection.Last((IList<RouteStop>)this);
		public ILocation LastLocation => LastStop?.Location;

		public void NewRoute(DateTime startTime)
		{
			Clear();
			StartTime = startTime;
			StartStop.EstimatedDeparture = null;
			Add(StartStop);
		}

		public new void Clear()
		{
			base.Clear();
		}

		public new void Add(RouteStop roadStop)
		{
			base.Add(roadStop);
			roadStop.EnqueueUpdate();
		}

		public async void AddRun(Run run, bool update = true)
		{
			if (StartTime == null) {
				StartTime = DateTime.Now;
			}
			if (StartStop.EstimatedDeparture == null) {
				StartStop.EstimatedDeparture = StartTime;
			}
			if (Count == 0) {
				Add(StartStop);
			}

			var pickupStop = new RouteStop(RouteStopType.HomePickup, run.OriginName, run.Origin, LastLocation, run.OriginTime);
			var dropoffStop = new RouteStop(RouteStopType.OfficeDropoff, run.DestinationName, run.Destination, run.Origin, run.DestinationTime);
			Add(pickupStop);
			Add(dropoffStop);

			if (update) {
				await RouteStop.Updates.Run();
			}
		}

		public void OnStopUpdated(RouteStop stop)
		{
			int index = IndexOf(stop);
			if (index <= 0)
				return;

			var departure = this[index-1].EstimatedDeparture;
			while (departure != null && stop.EstimatedDuration != null) {
				var arrival = new ScheduleTime((ScheduleTime)departure, (TimeSpan)stop.EstimatedDuration);
				departure = new ScheduleTime(arrival, MinStopTime);
				// TODO: adjust departure to RouteStop.SheduledTime

				stop.EstimatedArrival = arrival;
				stop.EstimatedDeparture = departure;

				if (++index < Count) {
					stop = this[index];
				} else
					break;
			}
		}
	}
}

