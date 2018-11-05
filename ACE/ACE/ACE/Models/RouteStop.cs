using System;
using Dwares.Drums;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public enum RouteStopType
	{
		Endpoint,
		Pickup,
		Dropoff
	}

	public class RouteStop: PropertyNotifier
	{
		public RouteStop(RouteStopType type, string name, ILocation destination, ILocation origin)
		{
			RouteStopType = type;
			Name = name;
			Destinations = destination ?? throw new ArgumentNullException(nameof(destination));
			Origin = origin;
		}

		public RouteStopType RouteStopType { get; }
		public string Name { get; }
		public string Address => Destinations.Address;
		public ILocation Destinations { get; }

		public ILocation Origin { get; }
		public ScheduleTime EstimatedStart { get; }
		public ScheduleTime EstimatedArrival { get; }
		public TimeSpan EsctimatedDurations { get; }

		public bool Started { get; }
		public ScheduleTime? ActualStart { get; }
		public TimeSpan? RemaningTime{ get; }

	}
}
