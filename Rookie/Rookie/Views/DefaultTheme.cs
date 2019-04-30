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

			AddStyle("FieldLabel", typeof(LabelEx),
				Label.FontSizeProperty, 16,
				Label.FontAttributesProperty, FontAttributes.Bold
				);

			AddStyle("StaticText", typeof(LabelEx),
				Label.FontSizeProperty, 16
				);
		}
	}
}
