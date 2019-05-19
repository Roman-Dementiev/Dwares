using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Drive.ViewModels
{
	public class RouteViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(RouteViewModel));

		public RouteViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Roure";
		}

		public Color ActiveBottomButtonColor {
			get => AppScope.ActiveBottomButtonColor;
		}
	}
}
