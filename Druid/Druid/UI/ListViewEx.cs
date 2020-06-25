using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ListViewEx : ListView, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(ListViewEx));
		public ListViewEx()
		{
			//Debug.EnableTracing(@class);

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ListViewEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ListViewEx listView) {
						listView.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
