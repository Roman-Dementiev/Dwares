using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.Satchel
{
	public class ColorPalette : ColorCollection
	{
		//static ClassRef @class = new ClassRef(typeof(ColorPalette));

		public const string keyPaletteName = "PaletteName";
		public const string keyPaletteDesign = "PaletteDesign";

		static Dictionary<string, ColorPalette> namedPalettes = new Dictionary<string, ColorPalette>();

		public ColorPalette(IDictionary<string, object> dict)
		{
			//Debug.EnableTracing(@class);

			if (dict != null) {
				Load(dict);
			}
		}

		public override string Name {
			get => GetMeta(keyPaletteName);
		}

		public override string Design {
			get => GetMeta(keyPaletteDesign);
		}

		public override void Load(IDictionary<string, object> dict)
		{
			var oldName = Name;
			base.Load(dict);

			var newName = Name;
			if (newName != oldName) {
				OnNameChanged(oldName, newName, namedPalettes);
			}
		}

		public static ColorPalette ByName(string name) 
			=> ByName(name, namedPalettes);
	}
}
