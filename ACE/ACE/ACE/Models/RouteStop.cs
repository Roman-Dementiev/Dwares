using System;
using System.Threading.Tasks;
using Dwares.Drums;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public enum RouteStopType
	{
		Endpoint,
		HomePickup,
		HomeDropoff,
		OfficePickup,
		OfficeDropoff
	}

	public enum RouteStopState
	{
		Pending,
		ReadyToGo,
		EnRoute,
		Arrived,
		Left
	}

	public class RouteStop: PropertyNotifier
	{
		public RouteStop(RouteStopType type, string name, ILocation location, ScheduleTime? scheduledTime)
		{
			RouteStopType = type;
			State = RouteStopState.Pending;
			Name = name;
			Origin = origin;
			Location = location ?? throw new ArgumentNullException(nameof(Location));
			ScheduledTime = scheduledTime;
		}

		public RouteStopType RouteStopType { get; }

		string name;
		public string Name {
			get => name;
			set => SetProperty(ref name, value);
		}

		public string Address {
			get => Location.Address;
			set {
				if (value != Location?.Address) {
					Location = new Location { Address = value };
				}
			}
		}

		ILocation location;
		public ILocation Location {
			get => location;
			private set {
				if (SetProperty(ref location, value)) {
					FirePropertyChanged(Address);
				}
			}
		}

		RouteStopState state;
		public RouteStopState State {
			get => state;
			set => SetProperty(ref state, value);
		}

		ILocation origin;
		public ILocation Origin {
			get => origin;
			set => SetProperty(ref origin, value);
		}

		ScheduleTime? scheduledTime;
		public ScheduleTime? ScheduledTime {
			get => scheduledTime;
			set => SetProperty(ref scheduledTime, value);
		}

		ScheduleTime? startTime;
		public ScheduleTime? StartTime {
			get => startTime;
			set => SetProperty(ref startTime, value);
		}

		TimeSpan? timeTillArrive;
		public TimeSpan? TimeTillArrive {
			get => timeTillArrive;
			set => SetProperty(ref timeTillArrive, value);
		}


		ScheduleTime? arriveTime;
		public ScheduleTime? ArriveTime {
			get => arriveTime;
			set => SetProperty(ref arriveTime, value);
		}

		ScheduleTime? leaveTime;
		public ScheduleTime? LeaveTime {
			get => leaveTime;
			set => SetProperty(ref leaveTime, value);
		}

	
		public bool IsUpdatable {
			get => Origin?.IsValidLocation() == true && Location.IsValidLocation();
		}

		public async Task<bool> UpdateTimeTillArrive()
		{
			if (IsUpdatable) {
				var info = await Maps.GetRouteInfo(Origin, Location);
				if (info != null) {
					TimeTillArrive = info.TravelTime;
					return true;
				}
			}
			return false;
		}

		public async Task<bool> Update(bool forceMapsRequest, bool forceCalculation)
		{
			bool updated = false;
			if (forceMapsRequest || TimeTillArrive == null || State == RouteStopState.EnRoute) {
				updated = await UpdateTimeTillArrive();
				if (updated) {
					ArriveTime = null;
					LeaveTime = null;
				}
			}

			if ((forceCalculation || ArriveTime == null) && StartTime != null && TimeTillArrive != null) {
				ArriveTime = new ScheduleTime((ScheduleTime)StartTime, (TimeSpan)TimeTillArrive);
			}

			if ((forceCalculation || LeaveTime == null) && ArriveTime != null) {
				var defaultStopTime = new TimeSpan(0, Settings.DefaultStopTime, 0);
				LeaveTime = new ScheduleTime((ScheduleTime)ArriveTime, defaultStopTime);
				// TODO: adjust departure to RouteStop.SheduledTime and wheelchair
			}

			return updated;
		}
	}
}
