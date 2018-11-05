using System;
using Dwares.Dwarf.Toolkit;
using BingMapsRESTToolkit;
using System.Collections.Generic;

namespace Dwares.Drums.Bing
{
	public static class BingConvert
	{
		public static double DistanceInMiles(double distance, DistanceUnitType unit)
		{
			if (unit == DistanceUnitType.Kilometers) {
				return UnitConverter.KilometersToMiles(distance);
			} else {
				return distance;
			}
		}

		public static TimeSpan Duration(double duration, TimeUnitType unit)
		{
			if (unit == TimeUnitType.Second) {
				int sec = (int)Math.Truncate(duration);
				int ms = (int)Math.Truncate((duration - sec) * 1000);
				return new TimeSpan(0, 0, sec, ms);
			} else {
				int min = (int)Math.Truncate(duration);
				int sec = (int)Math.Truncate((duration - min) * 60);
				return new TimeSpan(0, min, sec);
			}
		}

		public static TravelModeType TravelMode(IRouteOptions options)
		{
			var travelMode = options?.TravelMode;
			switch (travelMode)
			{
			case Drums.TravelMode.Walking: 
				return TravelModeType.Walking;
			case Drums.TravelMode.Transit: 
				return TravelModeType.Transit;
			case Drums.TravelMode.Truck: 
				return TravelModeType.Truck;
			default: 
				return TravelModeType.Driving;
			}
		}

		public static RouteOptimizationType Optimization(IRouteOptions options)
		{
			var optimization = options?.Optimization;
			if (optimization == null || optimization == Drums.Optimization.Default) {
				optimization = Drum.Instance.DefaultOptions.Optimization;
			}

			switch (optimization)
			{
			case Drums.Optimization.Distance:
				return RouteOptimizationType.Distance;
			case Drums.Optimization.Time:
				return RouteOptimizationType.Time;
			case Drums.Optimization.TimeWithTraffic:
				return RouteOptimizationType.TimeWithTraffic;
			case Drums.Optimization.TimeAvoidClosure:
				return RouteOptimizationType.TimeAvoidClosure;
			default:
				return RouteOptimizationType.TimeWithTraffic;
			}
		}

		public static List<AvoidType> Avoid(IRouteOptions options)
		{
			if (options == null) {
				options = Drum.Instance.DefaultOptions;
			}

			var list = new List<AvoidType>();

			switch (options.HighwaysRestriction)
			{
			case Restriction.Avoid:
				list.Add(AvoidType.Highways);
				break;
			case Restriction.Minimize:
				list.Add(AvoidType.MinimizeHighways);
				break;
			}

			switch (options.TollsRestriction)
			{
			case Restriction.Avoid:
				list.Add(AvoidType.Tolls);
				break;
			case Restriction.Minimize:
				list.Add(AvoidType.MinimizeTolls);
				break;
			}

			return list;
		}
	}
}
