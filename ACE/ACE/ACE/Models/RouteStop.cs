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
		Arrived
	}

	public class RouteStop: PropertyNotifier
	{
		public static readonly TaskQueue Updates = new TaskQueue();

		public RouteStop(RouteStopType type, string name, ILocation location, ILocation origin, ScheduleTime? scheduledTime)
		{
			RouteStopType = type;
			State = RouteStopState.Pending;
			Name = name;
			Origin = origin;
			Location = location ?? throw new ArgumentNullException(nameof(Location));
			ScheduledTime = scheduledTime;
		}

		public string Name { get; }
		public ILocation Location { get; }
		public string Address => Location.Address;

		public RouteStopType RouteStopType { get; }
		
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
		public ScheduleTime? SrartTime {
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

		ScheduleTime? departTime;
		public ScheduleTime? DepartTime {
			get => departTime;
			set => SetProperty(ref departTime, value);
		}

		//ScheduleTime? actualStart;
		//public ScheduleTime? ActualStart {
		//	get => actualStart;
		//	set => SetProperty(ref actualStart, value);
		//}

		//public TimeSpan? RemaningTime{ get; }
	
		public bool IsUpdatable {
			get => Origin?.IsValidLocation() == true && Location.IsValidLocation();
		}

		ulong updateId;
		private ulong UpdateId {
			get => updateId;
			set {
				if (SetProperty(ref updateId, value)) {
					FirePropertyChanged(nameof(Updating));
				}
			}
		}

		public bool Updating => UpdateId > 0;

		private async void Update()
		{
			if (IsUpdatable) {
				var info = await Maps.GetRouteInfo(Origin, Location);
				if (info != null) {
					TimeTillArrive = info.TravelTime;
					AppData.Route.OnStopUpdated(this);
				}
			}

			UpdateId = 0;
		}

		public void EnqueueUpdate()
		{
			if (IsUpdatable) {
				UpdateId = Updates.AddTask(Update);
			}
		}

	}
}
