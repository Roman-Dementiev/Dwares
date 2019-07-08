using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Satchel
{
	public class ColorScheme : IColorPalette
	{
		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

		Metadata metadata = new Metadata();

		public ColorScheme(string name, string design = null)
		{
			//Debug.EnableTracing(@class);

			Colors = new ColorCollection(name, design);
			Satchel.ColorPalette.AddNamedPalette(this);
		}

		public ColorScheme(IDictionary<string, object> dict, string name = null, string design = null) :
			this(name, design)
		{
			//Debug.EnableTracing(@class);

			if (dict != null) {
				Load(dict);
			}
		}

		public string Name {
			get => Colors.Name;
		}
		public string Design {
			get => Colors.Design;
		}

		public IColorPalette ColorPalette { get; set; }
		public ColorCollection Colors { get; }


		public void Load(IDictionary<string, object> dict)
		{
			Colors.Load(dict, metadata, this);
		}

		public virtual bool TryGetColor(string name, string variant, out Color color)
		{
			if (Colors.TryGetColor(name, variant, out color)) {
				return true;
			}

			if (ColorPalette != null) {
				var colorName = metadata.GetAsString(name);
				if (!string.IsNullOrEmpty(colorName) && ColorPalette.TryGetColor(colorName, variant, out color)) {
					return true;
				}
			}

			return false;
		}

		//public static ColorScheme Create(ResourceDictionary dict)
		//{
		//	object value;
		//	if (dict != null && dict.TryGetValue(keyColorSchemeDesign, out value) && value is string disign) {
		//		if (disign == MaterialColorScheme.DesignName) {
		//			return new MaterialColorScheme(dict);
		//		} else if (disign == WearColorScheme.DesignName) {
		//			return new WearColorScheme(dict);
		//		}
		//	}

		//	return new ColorScheme(dict);
		//}
	}
}
