using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class DatePickerEx : DatePicker, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(DatePickerEx));

		public DatePickerEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(DatePickerEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is DatePickerEx picker) {
						picker.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
