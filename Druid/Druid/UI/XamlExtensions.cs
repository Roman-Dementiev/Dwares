using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI.Xaml
{
	//public class ColorPalette : IMarkupExtension
	//{
	//	public ColorPalette() { }

	//	public string Name { set; get; }

	//	public object ProvideValue(IServiceProvider serviceProvider)
	//	{
	//		var palette = Satchel.ColorPalette.ByName(Name);
	//		return palette;
	//	}
	//}

	//public class ColorScheme : IMarkupExtension
	//{
	//	public ColorScheme() { }

	//	public string Name { set; get; }

	//	public object ProvideValue(IServiceProvider serviceProvider)
	//	{
	//		var scheme = UIColorScheme.ByName(Name);
	//		return scheme;
	//	}
	//}

	public class PaletteColor : IMarkupExtension
	{
		public PaletteColor() { }

		IColorPalette Palette { get; set; }
		public string Name { get; set; }
		public string Variant { set; get; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			var palette = Palette; //?? UsingPalette;
			if (palette == null)
				return Color.Transparent;

			return palette.GetColor(Name, Variant, default);
		}
	}
}
