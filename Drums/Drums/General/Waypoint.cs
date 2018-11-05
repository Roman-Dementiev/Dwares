using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Drums
{
	public class Waypoint : Location, IWaypoint
	{
		public WaypointType WaypointType { get; set; }
	}
}
