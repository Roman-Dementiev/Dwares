using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class NavigationPageEx : NavigationPage
	{
		//static ClassRef @class = new ClassRef(typeof(NavigationPageEx));

		public NavigationPageEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(UITheme_CurrentThemeChanged);
		}

		public NavigationPageEx(Page root, string flavor = null) :
			base(root)
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(UITheme_CurrentThemeChanged);

			if (!string.IsNullOrEmpty(flavor)) {
				Flavor = flavor;
			}
		}

		private void UITheme_CurrentThemeChanged()
		{
			Style = UITheme.Current.GetStyle(Flavor);
		}


		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(NavigationPageEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is NavigationPageEx page && newValue is string value) {
						page.Style = UITheme.Current.GetStyle(value);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

	}
}
