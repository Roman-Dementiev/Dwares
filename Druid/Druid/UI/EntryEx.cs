using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class EntryEx : Entry
	{
		//static ClassRef @class = new ClassRef(typeof(LabelEx));

		public EntryEx()
		{
			//Debug.EnableTracing(@class);

			this.ApplyFlavor(Flavor);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor(Flavor));
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(EntryEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EntryEx entry) {
						entry.ApplyFlavor(entry.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
