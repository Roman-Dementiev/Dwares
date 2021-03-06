﻿using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class EditorEx : Editor, IThemeAware 
	{
		//static ClassRef @class = new ClassRef(typeof(EditorEx));

		public EditorEx()
		{
			//Debug.EnableTracing(@class);

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}


		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(EditorEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EditorEx editor) {
						editor.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
