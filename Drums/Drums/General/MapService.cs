using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dwares.Drums
{
	public abstract class MapService : IMapService
	{
		public MapService(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public abstract Task<IRouteInfo> GetRouteInfo(IEnumerable<IWaypoint> waypoints, IRouteOptions options);
	}
}
