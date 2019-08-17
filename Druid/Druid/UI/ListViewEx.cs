using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ListViewEx : ListView
	{
		//static ClassRef @class = new ClassRef(typeof(ListViewEx));
		public ListViewEx()
		{
			//Debug.EnableTracing(@class);

			this.ApplyFlavor(Flavor);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor(Flavor));
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ListViewEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ListViewEx listView) {
						listView.ApplyFlavor(listView.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
