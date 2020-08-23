using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class FlyoutGoToPageMenuItem : MenuItem
	{
		//static ClassRef @class = new ClassRef(typeof(FlyoutGotoPageMenuItem));

		public FlyoutGoToPageMenuItem()
		{
			//Debug.EnableTracing(@class);

			this.Command = new Command(async () => {
				var route = Route;
				if (!string.IsNullOrEmpty(route)) {
					await Shell.Current.GoToAsync(route);
					Shell.Current.FlyoutIsPresented = false;
				}
			});
		}

		public static readonly BindableProperty RouteProperty =
			BindableProperty.Create(
				nameof(Route),
				typeof(string),
				typeof(FlyoutGoToPageMenuItem)
				//, propertyChanged: (bindable, oldValue, newValue) => {
				//	if (bindable is FlyoutGotoPageMenuItem item && newValue is string route) {
				//		//
				//	}
				//}
				);

		public string Route {
			set { SetValue(RouteProperty, value); }
			get { return (string)GetValue(RouteProperty); }
		}
	}
}
