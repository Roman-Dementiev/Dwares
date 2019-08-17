using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class GridEx : Grid
	{
		//static ClassRef @class = new ClassRef(typeof(GridEx));

		public GridEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => { Style = UITheme.Current.GetStyle(Flavor); });
		}


		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(GridEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is GridEx grid && newValue is string value) {
						grid.Style = UITheme.Current.GetStyle(value);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
