using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Drums;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public class Route : ObservableCollection<RouteStop>
	{
		public Route(Contact startPoint)
		{
			StartStop = new RouteStop(RouteStopType.Endpoint, startPoint.Name, startPoint, null, null);
			UpdateEstimations(false);
		}

		public TimeSpan AddDrivingTime { get; private set; }
		public TimeSpan AddDrivingTimeWithTrafic { get; private set; }
		public TimeSpan DefaultStopTime { get; private set; }
		public TimeSpan WheelchairStopTime { get; private set; }

		public DateTime? StartTime { get; private set; }
		public RouteStop StartStop { get; private set; }
		public RouteStop LastStop => Collection.Last((IList<RouteStop>)this);
		public ILocation LastLocation => LastStop?.Location;

		// ReadyToGo or EnRoute
		public RouteStop NextStop { get; set; }

		public void NewRoute(DateTime startTime)
		{
			Clear();
			StartTime = startTime;
			StartStop.DepartTime = null;
			StartStop.State = RouteStopState.ReadyToGo;
			NextStop = StartStop;
			Add(StartStop);
		}

		public new void Clear()
		{
			base.Clear();
		}

		public new void Add(RouteStop routeStop)
		{
			base.Add(routeStop);
			routeStop.EnqueueUpdate();
		}

		public new void Remove(RouteStop routeStop)
		{
			if (routeStop == null)
				return;

			int index = IndexOf(routeStop);
			if (index < 0)
				return;

			RemoveAt(index);
		}

		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);

			#error TODO
			RecalculateTimes(index);
		}

		public async void AddRun(Run run, bool update = true)
		{
			if (StartTime == null) {
				StartTime = DateTime.Now;
			}
			if (StartStop.DepartTime == null) {
				StartStop.DepartTime = StartTime;
			}
			if (Count == 0) {
				StartStop.State = RouteStopState.ReadyToGo;
				NextStop = StartStop;
				Add(StartStop);
			}

			var pickupStop = new RouteStop(RouteStopType.HomePickup, run.OriginName, run.Origin, LastLocation, run.OriginTime);
			var dropoffStop = new RouteStop(RouteStopType.OfficeDropoff, run.DestinationName, run.Destination, run.Origin, run.DestinationTime);
			Add(pickupStop);
			Add(dropoffStop);

			run.OriginStop = pickupStop;
			run.DestinationStop = dropoffStop;

			if (update) {
				await RouteStop.Updates.Run();
			}
		}

		public void OnStopUpdated(RouteStop stop)
		{
			int index = IndexOf(stop);
			if (index <= 0)
				return;

			RecalculateTimes(index);
		}

		//Suspender recalcilateSuspender;
		//int recalculateFromIndex = 0;

		//public void SuspendRecalculate() => recalcilateSuspender.Suspend();
		//public void ResumeRecalculate() => recalcilateSuspender.Resume();

		//void RequestRecalculate(int fromIndex = 1)
		//{
		//	Debug.Assert(fromIndex > 0 && fromIndex < Count);

		//	if (recalculateFromIndex == 0 || fromIndex < recalculateFromIndex) {
		//		recalculateFromIndex = fromIndex;
		//	}
		//	recalcilateSuspender.RequestAction();
		//}

		public void UpdateEstimations(bool recalculate = true)
		{
			AddDrivingTime = new TimeSpan(0, Settings.AddDrivingTime, 0);
			AddDrivingTimeWithTrafic = new TimeSpan(0, Settings.AddDrivingTimeWithTrafic, 0);
			DefaultStopTime = new TimeSpan(0, Settings.DefaultStopTime, 0);
			WheelchairStopTime = new TimeSpan(0, Settings.WheelchairStopTime, 0);

			if (recalculate) {
				RecalculateTimes();
			}
		}

		void RecalculateTimes(int fromIndex = 1)
		{
			if (fromIndex <= 0 || fromIndex >= Count)
				return;

			var index = fromIndex;
			var stop = this[index];
			var departure = this[index-1].DepartTime;
			while (departure != null && stop.TimeTillArrive != null) {
				var arrival = new ScheduleTime((ScheduleTime)departure, (TimeSpan)stop.TimeTillArrive);
				departure = new ScheduleTime(arrival, DefaultStopTime);
				// TODO: adjust departure to RouteStop.SheduledTime and wheelchair

				stop.ArriveTime = arrival;
				stop.DepartTime = departure;

				if (++index == Count)
					break;
				stop = this[index];
			}

		}

		public async Task ShowDirections(RouteStop stop)
		{
			int index = IndexOf(stop);
			if (index < 0)
				return;

			//ILocation from = new Location { Address = "4143 Paul St Philadelphia PA 19124" };
			ILocation from, dest;
			dest = stop.Location;
			if (dest == null)
				return;

			from = (index > 0) ? this[index-1].Location : null;
			if (from == null) {
				from = await Location.GetCurrentLocation();
			}

			await Maps.OpenDirections(from, dest);
		}

		public async Task GoToNextStop()
		{
			await GoTo(NextStop);
		}

		public async Task ArriveAtNextStop()
		{
			await Arrive(NextStop);
		}

		public async Task GoTo(RouteStop stop)
		{
			int index = IndexOf(stop);
			if (index < 0 || stop.State != RouteStopState.ReadyToGo)
				return;

			stop.SrartTime = ScheduleTime.Now;
			stop.State = RouteStopState.EnRoute;

			ILocation from;
			if (index > 0) {
				var prevStop = this[index-1];
				prevStop.DepartTime = stop.SrartTime;
				from = prevStop.Location;
			} else {
				from = null;
			}
			if (from == null) {
				from = await Location.GetCurrentLocation();
			}

			var dest = stop.Location;
			if (dest == null)
				return;
			// TODO
		}

		public async Task Arrive(RouteStop stop)
		{
			int index = IndexOf(stop);
			if (index < 0 || stop.State != RouteStopState.EnRoute)
				return;

			stop.ArriveTime = ScheduleTime.Now;
			stop.State = RouteStopState.Arrived;

			if (index < Count-1) {
				NextStop = this[index+1];
				NextStop.State = RouteStopState.ReadyToGo;
			} else {
				NextStop = null;
			}
		}
	}
}

