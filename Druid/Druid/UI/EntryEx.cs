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

			this.ApplyTheme();
			UITheme.CurrentThemeChanged += OnCurrentUIhemeChanged;
		}

		public static readonly BindableProperty ThemeStyleProperty =
			BindableProperty.Create(
				nameof(ThemeStyle),
				typeof(string),
				typeof(EntryEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EntryEx entry) {
						entry.ApplyTheme(entry.ThemeStyle);
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
