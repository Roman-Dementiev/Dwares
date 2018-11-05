using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;
using BingMapsRESTToolkit;


namespace Dwares.Drums.Bing
{
	public class BingRouteInfo : RouteInfo
	{
		public BingRouteInfo(Route route)
		{
			DistanceInMiles = BingConvert.DistanceInMiles(route.TravelDistance, route.DistanceUnitType);
			TravelTime = BingConvert.Duration(route.TravelDuration, route.TimeUnitType);
			TrafficCongestion = route.TrafficCongestion;
		}
	}

	public class BingRoute : BingRouteInfo, IRoute
	{
		public BingRoute(Route route) :
			base(route)
		{
			int legCount = route.RouteLegs.Length;
			legs = new Drums.RouteLeg[legCount];
			for (int i = 0; i < legCount; i++) {
				var l = route.RouteLegs[i];
				legs[i] = new Drums.RouteLeg() {
					DistanceInMiles = BingConvert.DistanceInMiles(l.TravelDistance, route.DistanceUnitType),
					TravelTime = BingConvert.Duration(l.TravelDuration, route.TimeUnitType),
					//TODO
					//StartPoint = ;
					//EndPoint = ;
				};
			}
		}

		Drums.RouteLeg[] legs;
		public IEnumerable<IRouteLeg> Legs => legs;
	}
}
