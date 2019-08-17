using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class LabelEx : Label
	{
		//static ClassRef @class = new ClassRef(typeof(LabelEx));

 		public LabelEx()
		{
			//Debug.EnableTracing(@class);

			this.ApplyFlavor(Flavor);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor(Flavor));
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(LabelEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is LabelEx label) {
						label.ApplyFlavor(label.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
