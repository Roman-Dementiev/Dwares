﻿using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class BoxViewEx : BoxView, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(BoxViewEx));

		public BoxViewEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(BoxViewEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is BoxViewEx boxView) {
						boxView.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
