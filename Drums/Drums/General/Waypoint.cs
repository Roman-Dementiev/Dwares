using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Drums
{
	public class Waypoint : Location, IWaypoint
	{
		public Waypoint(WaypointType type)
		{
			WaypointType = type;
		}

		public Waypoint(WaypointType type, ILocation location) :
			base(location)
		{
			WaypointType = type;
		}

		public WaypointType WaypointType { get; }

		public static Waypoint FromLocation(WaypointType type, ILocation location)
		{
			if (location == null)
				return null;

			return new Waypoint(type, location);
		}

		public static Waypoint FromAddress(WaypointType type, string address)
		{
			if (address == null)
				return null;

			return new Waypoint(type, null) { Address = address };
		}
	}
}
