using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Drums
{
	public abstract class MapService : NameHolder, IMapService
	{
		public MapService(string name) : base(name) { }

		public abstract Task<IRouteInfo> GetRouteInfo(IEnumerable<IWaypoint> waypoints, IRouteOptions options);
	}
}
