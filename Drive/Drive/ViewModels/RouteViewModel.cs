using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Drive.Views;


namespace Drive.ViewModels
{
	public class RouteViewModel : CollectionViewModel<RouteCardViewModel>, IRootContentViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(RouteViewModel));

		public RouteViewModel() :
			base(ApplicationScope, RouteCardViewModel.CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Route";
		}


		public Type ContentViewType()
		{
			return typeof(RouteView);
		}

		public Type ControlsViewType(bool landscape)
		{
			return null;
		}
	}
}
