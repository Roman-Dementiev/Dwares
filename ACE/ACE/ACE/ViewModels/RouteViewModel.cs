using System;
using System.Collections.Specialized;
using Dwares.Dwarf.Collections;
using Dwares.Druid.Support;
using ACE.Models;
using ACE.Views;

namespace ACE.ViewModels
{
	public class RouteViewModel : CollectionViewModel<RouteItem>
	{
		public RouteViewModel()
		{
			ParentScope = AppScope;
			Items = new ShadowCollection<RouteItem,RouteStop>(AppData.Route, (routeStop => new RouteItem(routeStop)));
		}
	}
}
