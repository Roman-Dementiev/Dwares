using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Druid.UI;
using Xamarin.Forms;

namespace Dwares.Rookie.Views
{
	public class DefaultTheme : UITheme
	{
		//static ClassRef @class = new ClassRef(typeof(DefaultYheme));

		public DefaultTheme()
		{
			//Debug.EnableTracing(@class);

			AddStyle(nameof(Label), typeof(Label),
				Label.FontSizeProperty, 20,
				Label.FontAttributesProperty, FontAttributes.Bold
				);

			AddStyle(nameof(StaticText), typeof(StaticText),
				Label.FontSizeProperty, 20
				);
		}
	}
}
