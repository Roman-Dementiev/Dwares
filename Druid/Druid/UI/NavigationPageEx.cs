using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class NavigationPageEx : NavigationPage, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(NavigationPageEx));

		public NavigationPageEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(NavigationPageEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is NavigationPageEx page) {
						page.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

	}
}
