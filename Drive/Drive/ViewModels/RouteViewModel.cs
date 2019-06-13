using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Drive.ViewModels
{
	public class RouteViewModel : CollectionViewModel<RouteItem>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteViewModel));

		public RouteViewModel() :
			base(ApplicationScope, RouteItem.CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Route";
		}
	}
}
