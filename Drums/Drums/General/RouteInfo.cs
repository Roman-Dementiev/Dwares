using System;
using Dwares.Dwarf;

namespace Dwares.Drums
{
	public class RouteInfo : IRouteInfo
	{
		public double DistanceInMiles { get; set; }
		public TimeSpan TravelTime { get; set; }
		public string TrafficCongestion { get; set; }

		public override string ToString()
		{
			return Strings.Properties(this);
		}
	}
}
