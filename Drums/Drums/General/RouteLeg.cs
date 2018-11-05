using System;


namespace Dwares.Drums
{
	public class RouteLeg : IRouteLeg
	{
		public double DistanceInMiles { get; set; }
		public TimeSpan TravelTime { get; set; }
		//TODO
		//public ILocation StartPoint { get; set; }
		//public ILocation EndPoint { get; set; }
	}
}
