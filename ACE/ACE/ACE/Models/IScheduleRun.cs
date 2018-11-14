//using System;
//using System.Collections.Generic;
//using System.Text;
//using Dwares.Drums;


//namespace ACE.Models
//{
//	public enum SheduleRunType
//	{
//		Appoitment,
//		ReturnHpme,
//		MidddleRun
//	}

//	// TODO: move/delete?
//	public interface IRouteRun
//	{
//		//string OriginName { get; }
//		//ILocation OriginLocation { get; }
//		//ScheduleTime? OriginTime { get; }

//		//string DestinationName { get; }
//		//ILocation DestinationLocation { get; }
//		//ScheduleTime? DestinationTime { get; }

//		RouteStop OriginStop { get; set; }
//		RouteStop Destination { get; }
//	}

//	public interface IScheduleRun
//	{
//		SheduleRunType RunType { get; }
//		Contact Client { get; }
//		//Contact Office { get; }

//		RouteStop PickupStop { get; }
//		ScheduleTime PickupTime { get; }
//		RouteStop DropoffStop { get; }
//		ScheduleTime DropoffTime { get; }
//	}
//}
