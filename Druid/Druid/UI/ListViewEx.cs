using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ListViewEx : ListView
	{
		//static ClassRef @class = new ClassRef(typeof(ListViewEx));
		public ListViewEx()
		{
			//Debug.EnableTracing(@class);

			this.ApplyTheme();
			UITheme.CurrentThemeChanged += OnCurrentUIhemeChanged;
		}

		public static readonly BindableProperty ThemeStyleProperty =
			BindableProperty.Create(
				nameof(ThemeStyle),
				typeof(string),
				typeof(ListViewEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ListViewEx listView) {
						listView.ApplyTheme(listView.ThemeStyle);
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
