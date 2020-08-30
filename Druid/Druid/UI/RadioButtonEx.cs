using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class RadioButtonEx : RadioButton, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(RadioButtonEx));

		public RadioButtonEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(RadioButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is RadioButtonEx radio) {
						radio.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
