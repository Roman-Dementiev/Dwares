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

			this.ApplyTheme();
			UITheme.CurrentThemeChanged += OnCurrentUIhemeChanged;
		}

		public static readonly BindableProperty ThemeStyleProperty =
			BindableProperty.Create(
				nameof(ThemeStyle),
				typeof(string),
				typeof(LabelEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is LabelEx label) {
						label.ApplyTheme(label.ThemeStyle);
					}
				});

		public string ThemeStyle {
			set { SetValue(ThemeStyleProperty, value); }
			get { return (string)GetValue(ThemeStyleProperty); }
		}

		private void OnCurrentUIhemeChanged(object sender, EventArgs e)
		{
			this.ApplyTheme(ThemeStyle);
		}
	}
}
