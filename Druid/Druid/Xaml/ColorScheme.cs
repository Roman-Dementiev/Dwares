using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Xaml
{
	public class ColorScheme : MarkupExtension<Satchel.ColorScheme>
	{	
		public ColorScheme() { }
			
		public string Name { set; get; }

		public override Satchel.ColorScheme ProvideValue(IServiceProvider serviceProvider)
		{
			var scheme = Satchel.ColorPalette.ByName(Name) as Satchel.ColorScheme;
			return scheme;
		}
	}
}
