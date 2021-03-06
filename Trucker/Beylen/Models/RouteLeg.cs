﻿using System;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public enum RouteLegStatus
	{
		Pending,
		Enroute,
		Complete
	}

	public class RouteLeg //: Model
	{
		//static ClassRef @class = new ClassRef(typeof(RouteLeg));

		public RouteLeg()
		{
			//Debug.EnableTracing(@class);
		}

		public RouteStop StartPoint { get; set; }
		public RouteStop EndPoint { get; set; }
		public RouteLegStatus Status { get; set; }
		public TimeSpan? Duration { get; set; }

		public bool DurationRequested { get; set; }


		//public RouteStop StartPoint {
		//	get => startPoint;
		//	set => SetProperty(ref startPoint, value);
		//}
		//RouteStop startPoint;

		//public RouteStop EndPoint {
		//	get => endPoint;
		//	set => SetProperty(ref endPoint, value);
		//}
		//RouteStop endPoint;

		//public RouteLegStatus Status {
		//	get => status;
		//	set => SetProperty(ref status, value);
		//}
		//RouteLegStatus status;

		//public TimeSpan? Duration {
		//	get => duration;
		//	set => SetProperty(ref duration, value);
		//}
		//TimeSpan? duration = null;

	}
}
