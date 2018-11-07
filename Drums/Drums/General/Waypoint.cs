using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Drums
{
	public class Waypoint : Location, IWaypoint
	{
		public Waypoint(WaypointType type, ILocation location = null) :
			base(location)
		{
			WaypointType = type;
		}

		public WaypointType WaypointType { get; }
	}
}
