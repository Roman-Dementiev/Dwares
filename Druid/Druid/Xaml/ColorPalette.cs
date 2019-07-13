using System;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Xaml
{
	class ColorPalette : MarkupExtension<IColorPalette>
	{
		public ColorPalette() { }

		public string Name { set; get; }

		public override IColorPalette ProvideValue(IServiceProvider serviceProvider)
		{
			var palette = Satchel.ColorPalette.ByName(Name);
			return palette;
		}
	}
}
