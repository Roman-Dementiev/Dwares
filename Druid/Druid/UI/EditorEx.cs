using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class EditorEx : Editor 
	{
		//static ClassRef @class = new ClassRef(typeof(EditorEx));

		public EditorEx()
		{
			//Debug.EnableTracing(@class);

			this.ApplyTheme();
			UITheme.CurrentThemeChanged += OnCurrentUIhemeChanged;
		}


		public static readonly BindableProperty ThemeStyleProperty =
			BindableProperty.Create(
				nameof(ThemeStyle),
				typeof(string),
				typeof(EditorEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EditorEx editor) {
						editor.ApplyTheme(editor.ThemeStyle);
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
