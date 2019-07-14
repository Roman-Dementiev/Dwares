using System;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Dwares.Druid.Xaml
{
	public class BindColor : MarkupExtension<Color>
	{
		//static ClassRef @class = new ClassRef(typeof(BindColor));

		public BindColor()
		{
			//Debug.EnableTracing(@class);
		}

		public IColorPalette Palette { get; set; }
		public string Name { get; set; }
		//public string Variant { set; get; }

		public override Color ProvideValue(IServiceProvider serviceProvider)
		{
			var palette = Palette; //?? Satchel.ColorScheme.BindingPalette;
			if (palette == null)
				return default;

			var colorName = new ColorName(Name);
			return palette.GetColor(colorName, null/*Variant*/);
		}
	}
}
