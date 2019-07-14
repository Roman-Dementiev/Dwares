using System;
using System.Collections.Generic;
using System.Text;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Xaml
{
	class MaterialColorPalette : MarkupExtension<IColorPalette>
	{
		public override IColorPalette ProvideValue(IServiceProvider serviceProvider)
		{
			return Resources.MaterialColorPalette.Instance;
		}
	}
}
