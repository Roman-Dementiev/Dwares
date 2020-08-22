using System;
using System.Collections.Generic;
using BingMapsRESTToolkit;


namespace Dwares.Drums.Bing
{
	public class BingWaypoint : SimpleWaypoint, IWaypoint
	{
		public BingWaypoint(IWaypoint wp)
		{
			if (wp.HasCoordinate) {
				Coordinate = BingCoordinate.ToBing(wp.GetCoordinate());
			}
			if (wp.HasAddress) {
				Address = wp.GetAddress();
			}

			WaypointType = wp.WaypointType;
			IsViaPoint = wp.WaypointType == WaypointType.ViaPoint;
		}

		public WaypointType WaypointType { get; set; }
		public string Landmark { get; set; }

		bool ILocation.HasAddress => !string.IsNullOrEmpty(Address);
		string ILocation.GetAddress() => Address;

		bool ILocation.HasCoordinate => Coordinate != null;
		ICoordinate ILocation.GetCoordinate() => Coordinate as BingCoordinate;


		public static BingWaypoint ToBing(IWaypoint wp)
		{
			if (wp == null)
				return null;

			if (wp is BingWaypoint bwp)
				return bwp;

			return new BingWaypoint(wp);
		}

		public static List<SimpleWaypoint> List(IEnumerable<IWaypoint> waypoints)
		{
			var list = new List<SimpleWaypoint>();
			foreach (var wp in waypoints) {
				list.Add(new BingWaypoint(wp));
			}
			return list;
		}
	}
}
