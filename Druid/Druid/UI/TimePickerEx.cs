using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class TimePickerEx : TimePicker, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(TimePickerEx));

		public TimePickerEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(TimePickerEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is TimePickerEx picker) {
						picker.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
