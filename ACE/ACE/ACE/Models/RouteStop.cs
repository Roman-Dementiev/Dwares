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

	public class RouteStop: PropertyNotifier
	{
		public static readonly TaskQueue Updates = new TaskQueue();

		public RouteStop(RouteStopType type, string name, ILocation location, ILocation origin, ScheduleTime? scheduledTime)
		{
			RouteStopType = type;
			Name = name;
			Origin = origin;
			Location = location ?? throw new ArgumentNullException(nameof(Location));
			ScheduledTime = scheduledTime;
		}

		public RouteStopType RouteStopType { get; }

		public string Name { get; }
		public string Address => Location.Address;

		public ILocation Origin { get; }
		public ILocation Location { get; }

		ScheduleTime? scheduledTime;
		public ScheduleTime? ScheduledTime {
			get => scheduledTime;
			set => SetProperty(ref scheduledTime, value);
		}

		TimeSpan? estimatedDuration;
		public TimeSpan? EstimatedDuration {
			get => estimatedDuration;
			set => SetProperty(ref estimatedDuration, value);
		}

		//ScheduleTime? estimatedStart;
		//public ScheduleTime? EstimatedStart {
		//	get => estimatedStart;
		//	set => SetProperty(ref estimatedStart, value);
		//}

		ScheduleTime? estimatedArrival;
		public ScheduleTime? EstimatedArrival {
			get => estimatedArrival;
			set => SetProperty(ref estimatedArrival, value);
		}

		ScheduleTime? estimatedDeparture;
		public ScheduleTime? EstimatedDeparture {
			get => estimatedDeparture;
			set => SetProperty(ref estimatedDeparture, value);
		}

		public bool Started => ActualStart != null;
		public ScheduleTime? ActualStart { get; private set; }

		//public TimeSpan? RemaningTime{ get; }
	
		public void Start()
		{
			if (Started)
				return;

			ActualStart = ScheduleTime.Now;
			PropertiesChanged(nameof(Started), nameof(ActualStart));

			// TODO
		}

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
				var info = await Drum.GetRouteInfo(Origin, Location);
				if (info != null) {
					EstimatedDuration = info.TravelTime;
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
