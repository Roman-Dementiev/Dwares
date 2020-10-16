using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public class ButtonEx : Button, IThemeAware
	{
		public ButtonEx()
		{
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ButtonEx button) {
						button.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

		public string Writ {
			get => writ;
			set {
				if (value != writ) {
					OnPropertyChanging();
					writ = value;
					Command = new WritCommand(writ);
					OnPropertyChanged();
				}
			}
		}
		string writ;

	}
}
