using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ShellContentEx : ShellContent, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(ShellContentEx));

		public ShellContentEx()
		{
			//Debug.EnableTracing(@class);
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ShellContentEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ShellContentEx shell) {
						shell.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

	}
}
