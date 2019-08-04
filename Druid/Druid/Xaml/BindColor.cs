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

		public Satchel.ColorPalette Source { get; set; }
		public string Name { get; set; }

		public override Color ProvideValue(IServiceProvider serviceProvider)
		{
			var palette = Source; //?? Satchel.ColorScheme.BindingPalette;
			if (palette == null)
				return default;

			var color = palette.GetColor(Name);
			return color;
		}
	}
}
