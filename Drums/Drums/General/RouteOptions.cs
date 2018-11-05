using System;


namespace Dwares.Drums
{
	public class RouteOptions : IRouteOptions
	{
		public TravelMode TravelMode { get; set; }
		public Restriction HighwaysRestriction { get; set; }
		public Restriction TollsRestriction { get; set; }
		public Optimization Optimization { get; set; }

		//public static readonly RouteOptions Default = new RouteOptions {
		//	TravelMode = TravelMode.Driving,
		//	Optimization = Optimization.TimeWithTraffic
		//};
	}
}
