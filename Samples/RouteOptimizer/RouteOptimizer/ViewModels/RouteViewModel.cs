using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.ViewModels;
using RouteOptimizer.Models;


namespace RouteOptimizer.ViewModels
{
	public class RouteViewModel : CardCollectionViewModel<RouteStop, RouteStopCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteViewModel));

		public RouteViewModel() :
			base(App.Current.Route.Stops)
		{
			//Debug.EnableTracing(@class);

			Title = "Route";
		}

		public ObservableCollection<RouteStopCardModel> Stops => Items;
	}
}
