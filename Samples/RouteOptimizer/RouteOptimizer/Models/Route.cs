using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;

namespace RouteOptimizer.Models
{
	public class Route
	{
		//static ClassRef @class = new ClassRef(typeof(Route));

		public Route()
		{
			//Debug.EnableTracing(@class);
		}

		public OrderableCollection<RouteStop> Stops { get; } = new OrderableCollection<RouteStop>();
		//public ObservableCollection<RouteLeg> Legs { get; } = new ObservableCollection<RouteLeg>();

		public void Add(RouteStop stop)
		{
			if (Stops.Count > 0) {
				var last = Stops[Stops.Count-1];
				stop.Leg = new RouteLeg {
					StartPoint = last,
					EndPoint = stop
				};
			}
			//stop.Ordinal = Stops.Count+1;
			Stops.Add(stop);
		}

		public void Clear()
		{
			Stops.Clear();
		}
	}
}
