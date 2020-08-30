using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class CheckBoxEx : CheckBox, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(CheckBoxEx));

		public CheckBoxEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(CheckBoxEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is CheckBoxEx checkBox) {
						checkBox.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
