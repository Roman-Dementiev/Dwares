using System;
using System.Collections.Generic;
using BingMapsRESTToolkit;


namespace Dwares.Drums.Bing
{
	public class BingWaypoint : SimpleWaypoint, IWaypoint
	{
		public BingWaypoint(IWaypoint wp)
		{
			if (wp.Coordinate != null) {
				Coordinate = BingCoordinate.ToBing(wp.Coordinate);
			}
			if (wp.Address != null) {
				Address = wp.Address;
			}

			WaypointType = wp.WaypointType;
			IsViaPoint = wp.WaypointType == WaypointType.ViaPoint;
		}

		public WaypointType WaypointType { get; set; }
		public string Landmark { get; set; }
		ICoordinate ILocation.Coordinate => Coordinate as BingCoordinate;


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
