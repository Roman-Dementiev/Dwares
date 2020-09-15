using System;
using Dwares.Dwarf;
using RouteOptimizer.Models;


namespace RouteOptimizer.ViewModels
{
	public class RouteStopCardModel : CardViewModel<RouteStop>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStopCardModel));

		public RouteStopCardModel()
		{
			//Debug.EnableTracing(@class);
		}

		public RouteStopCardModel(RouteStop source) :
			base(source)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
