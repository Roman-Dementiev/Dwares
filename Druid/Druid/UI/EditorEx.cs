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

			this.ApplyFlavor(Flavor);
			UITheme.CurrentThemeChanged += (s, e) => this.ApplyFlavor(Flavor);
		}


		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(EditorEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EditorEx editor) {
						editor.ApplyFlavor(editor.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
