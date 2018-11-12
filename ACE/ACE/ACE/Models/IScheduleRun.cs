using System;
using System.Collections.Generic;
using System.Text;
using Dwares.Drums;


namespace ACE.Models
{
	public interface IRun
	{
		string OriginName { get; }
		ILocation OriginLocation { get; }
		ScheduleTime? OriginTime { get; }

		string DestinationName { get; }
		ILocation DestinationLocation { get; }
		ScheduleTime? DestinationTime { get; }
	}

	public interface IScheduleRun : IRun
	{
		Contact Client { get; }

		RouteStop PickupStop { get; set; }
		RouteStop DropoffStop { get; set; }
	}
}
