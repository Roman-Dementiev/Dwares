using System;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Xaml
{
	public class ColorPalette : MarkupExtension<Satchel.ColorPalette>
	{
		public ColorPalette() { }

		public string Name { set; get; }

		public override Satchel.ColorPalette ProvideValue(IServiceProvider serviceProvider)
		{
			var palette = Satchel.ColorPalette.ByName(Name);
			return palette;
		}
	}

	public class MaterialColorPalette : ColorPalette
	{
		public override Satchel.ColorPalette ProvideValue(IServiceProvider serviceProvider)
		{
			var name = Name;
			if (string.IsNullOrEmpty(name))
				name = Resources.MaterialPalette.DefaultName;

			var palette = Satchel.ColorPalette.GetInstance<Resources.MaterialPalette>(name);
			return palette;
		}
	}
}
