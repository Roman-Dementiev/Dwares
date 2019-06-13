using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Drive.Models
{
	public class Route : OrderedCollection<RouteStop>
	{
		//static ClassRef @class = new ClassRef(typeof(Route));

		public Route() :
			base(RouteStop.Compare)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
