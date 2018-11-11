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

		public RouteStop ArrivedStop { get; set; }
		// ReadyToGo or EnRoute
		public RouteStop NextStop { get; set; }

		public void NewRoute(DateTime startTime)
		{
			Clear();
			StartTime = startTime;
			StartStop.LeaveTime = null;
			StartStop.State = RouteStopState.ReadyToGo;
			NextStop = StartStop;
			Add(StartStop);
		}

		public new void Clear()
		{
			base.Clear();
		}

		public new async void Add(RouteStop routeStop)
		{
			base.Add(routeStop);
			await Update();
		}

		public new void Remove(RouteStop routeStop)
		{
			if (routeStop == null)
				return;

			int index = IndexOf(routeStop);
			if (index < 0)
				return;

			RemoveRange(index, 1);
		}

		public new void RemoveAt(int index)
		{
			if (index >= 0 && index < Count) {
				RemoveRange(index, 1);
			}
		}

		public void RemoveStops(RouteStop firstStop, RouteStop secondStop)
		{
			int index = IndexOf(firstStop);
			if (index > 0) {
				if (index < Count-1 && this[index+1] == secondStop) {
					RemoveRange(index, 2);
					return;
				}
				RemoveRange(index, 1);
			}

			Remove(secondStop);
		}

		private void RemoveRange(int firstIndex, int count)
		{
			Debug.Assert(firstIndex >= 0 && count > 0 && firstIndex+count <= Count);

			for (int i = 0; i < count; i++) {
				base.RemoveAt(firstIndex);
			}

			ResetEstimations(firstIndex);
		}

		private void ResetEstimations(int srartIndex)
		{
			for (int i = srartIndex; i < Count; i++) {
				var stop = this[i];
				var state = stop.State;

				if (state < RouteStopState.EnRoute)
					stop.StartTime = null;
				if (state < RouteStopState.Arrived)
					stop.ArriveTime = null;
				if (state < RouteStopState.Left)
					stop.LeaveTime = null;
			}
		}

		public async void AddRun(Run run, bool update = true)
		{
			if (StartTime == null) {
				StartTime = DateTime.Now;
			}
			if (StartStop.LeaveTime == null) {
				StartStop.LeaveTime = (DateTime)StartTime;
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
				await Update();
			}
		}

		bool needUpdateAll;
		bool updating;

		public async Task Update(bool updateAll = false)
		{
			needUpdateAll |= updateAll;
			if (updating)
				return;

			updating = true;
			try {
				await RunUpdate(false);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			finally {
				updating = false;
				needUpdateAll = false;
			}
		}

		public void UpdateEstimations(bool update = true)
		{
			AddDrivingTime = new TimeSpan(0, Settings.AddDrivingTime, 0);
			AddDrivingTimeWithTrafic = new TimeSpan(0, Settings.AddDrivingTimeWithTrafic, 0);
			DefaultStopTime = new TimeSpan(0, Settings.DefaultStopTime, 0);
			WheelchairStopTime = new TimeSpan(0, Settings.WheelchairStopTime, 0);

			if (update) {
				var task = RunUpdate(true);
				Debug.Assert(task.IsCompleted);
			}
		}

		private async Task RunUpdate(bool estimateOnly)
		{
			bool repeat;
			do {
				bool forceUpdates;
				if (estimateOnly) {
					forceUpdates = false;
				} else {
					forceUpdates = needUpdateAll;
					needUpdateAll = false;
				}
				repeat = false;

				int index;
				ScheduleTime? startTme = null;
				if (ArrivedStop != null) {
					index = IndexOf(ArrivedStop);
					Debug.Assert(index >= 0);

					startTme = ArrivedStop.LeaveTime;
					index++;
				}
				else if (NextStop != null) {
					index = IndexOf(NextStop);
					Debug.Assert(index >= 0);

					startTme = NextStop.StartTime;
				} else {
					Debug.Fail();
					index = 0;
				}

				for  ( ; index < Count; index++)
				{
					var stop = this[index];
					if (stop.State >= RouteStopState.Arrived) {
						Debug.Fail();
						startTme = stop.LeaveTime;
						continue;
					}

					if (stop.StartTime == null) {
						stop.StartTime = startTme;
					}

					bool updated = false;
					if (!estimateOnly) {
						if (forceUpdates || stop.TimeTillArrive == null || stop.State == RouteStopState.EnRoute) {
							updated = await stop.UpdateTimeTillArrive();
							if (updated) {
								stop.ArriveTime = null;
								stop.LeaveTime = null;

								ResetEstimations(index+1);
								//for (int i = index+1; i < Count; i++) {
								//	var s = this[i];
								//	s.StartTime = null;
								//	s.ArriveTime = null;
								//	s.LeaveTime = null;
								//}
							}
						}
					}

					if ((estimateOnly || stop.ArriveTime == null) && StartTime != null && stop.TimeTillArrive != null) {
						stop.ArriveTime = new ScheduleTime((ScheduleTime)StartTime, (TimeSpan)stop.TimeTillArrive);
					}

					if ((estimateOnly || stop.LeaveTime == null) && stop.ArriveTime != null) {
						var defaultStopTime = new TimeSpan(0, Settings.DefaultStopTime, 0);
						stop.LeaveTime = new ScheduleTime((ScheduleTime)stop.ArriveTime, defaultStopTime);
						// TODO: adjust departure to RouteStop.SheduledTime and wheelchair
					}


					if (updated || needUpdateAll) {
						repeat = true;
						break;
					}

					startTme = stop.LeaveTime;
				}
			}
			while (repeat);
		}

		//public void OnStopUpdated(RouteStop stop)
		//{
		//	int index = IndexOf(stop);
		//	if (index <= 0)
		//		return;

		//	RecalculateTimes(index);
		//}

		//void RecalculateTimes(int fromIndex = 1)
		//{
		//	if (fromIndex <= 0 || fromIndex >= Count)
		//		return;

		//	var index = fromIndex;
		//	var stop = this[index];
		//	var departure = this[index-1].LeaveTime;
		//	while (departure != null && stop.TimeTillArrive != null) {
		//		var arrival = new ScheduleTime((ScheduleTime)departure, (TimeSpan)stop.TimeTillArrive);
		//		departure = new ScheduleTime(arrival, DefaultStopTime);
		//		// TODO: adjust departure to RouteStop.SheduledTime and wheelchair

		//		stop.ArriveTime = arrival;
		//		stop.LeaveTime = departure;

		//		if (++index == Count)
		//			break;
		//		stop = this[index];
		//	}

		//}

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

			ArrivedStop = null;
			stop.StartTime = ScheduleTime.Now;
			stop.State = RouteStopState.EnRoute;

			if (index > 0) {
				var prevStop = this[index-1];
				prevStop.State = RouteStopState.Left;
				prevStop.LeaveTime = stop.StartTime;
			}
		}

		public async Task Arrive(RouteStop stop)
		{
			int index = IndexOf(stop);
			if (index < 0 || stop.State != RouteStopState.EnRoute)
				return;

			var now = ScheduleTime.Now;
			ArrivedStop = stop;
			stop.ArriveTime = now;
			stop.State = RouteStopState.Arrived;
			stop.LeaveTime = EstimateLeaveTime(now);

			if (++index < Count) {
				NextStop = this[index];
				NextStop.State = RouteStopState.ReadyToGo;
				NextStop.StartTime = stop.LeaveTime;
			} else {
				NextStop = null;
			}

			await Update();
		}

		public ScheduleTime EstimateLeaveTime(ScheduleTime arriveTime)
		{
			var leaveTime = new ScheduleTime(arriveTime, DefaultStopTime);
			// TODO: adjust departure to RouteStop.SheduledTime and wheelchair

			return leaveTime;
		}
	}
}

