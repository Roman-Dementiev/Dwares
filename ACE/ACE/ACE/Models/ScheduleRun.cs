using System;
using Dwares.Drums;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public class ScheduleRun : PropertyNotifier, IScheduleRun
	{
		protected ScheduleRun() { }

		//protected ScheduleRun(Contact client, Contact office)
		//{
		//	Client = client ?? throw new ArgumentNullException(nameof(client));
		//	Office = office ?? throw new ArgumentNullException(nameof(office));
		//}

		public Contact Client { get; }
		public Contact Office { get; }

		public RouteStop PickupStop { get; set; }
		public RouteStop DropoffStop { get; set; }

		public string OriginName => PickupStop?.Name;
		public ILocation OriginLocation => PickupStop?.Location;
		public ScheduleTime? OriginTime => PickupStop?.ScheduledTime;

		public string DestinationName => DropoffStop?.Name;
		public ILocation DestinationLocation => DropoffStop?.Location;
		public ScheduleTime? DestinationTime => DropoffStop?.ScheduledTime;
	}
}
