using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class SwitchEx : Switch, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(SwicthEx));

		public SwitchEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(SwitchEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SwitchEx _switch) {
						_switch.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
