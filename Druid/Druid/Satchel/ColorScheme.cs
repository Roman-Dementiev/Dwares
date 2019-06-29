using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.Satchel
{
	public class ColorScheme : ColorCollection
	{
		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

		public const string keyColorSchemeName = "ColorSchemeName";
		public const string keyColorSchemeDesign = "ColorSchemeDesign";

		static Dictionary<string, ColorScheme> namedSchemes = new Dictionary<string, ColorScheme>();

		public ColorScheme(string name, string design = null)
		{
			//Debug.EnableTracing(@class);

			if (!string.IsNullOrEmpty(name)) {
				SetMeta(keyColorSchemeName, name);
			}

			if (design != null) {
				SetMeta(keyColorSchemeDesign, design);
			}
		}

		public IColorCollection Colors { get; }

		public ColorScheme(IDictionary<string, object> dict)
		{
			//Debug.EnableTracing(@class);

			if (dict != null) {
				Load(dict);
			}
		}


		public override string Name {
			get => GetMeta(keyColorSchemeName);
		}

		public override string Design {
			get => GetMeta(keyColorSchemeDesign);
		}


		public override void Load(IDictionary<string, object> dict)
		{
			var oldName = Name;
			base.Load(dict);

			var newName = Name;
			if (newName != oldName) {
				OnNameChanged(oldName, newName, namedSchemes);
			}
		}

		public override bool TryGetColor(string name, string variant, out Color color, Color defaultValue = default)
		{
			if (Colors != null) {
				var colorName = GetMeta(name);
				if (!string.IsNullOrEmpty(colorName) && Colors.TryGetColor(colorName, variant, out color, defaultValue)) {
					return true;
				}
			}

			return TryGetColor(name, variant, out color, defaultValue);

		}
		public static ColorScheme ByName(string name)
			=> ByName(name, namedSchemes);

		public static ColorScheme Create(ResourceDictionary dict)
		{
			object value;
			if (dict != null && dict.TryGetValue(keyColorSchemeDesign, out value) && value is string disign) {
				if (disign == MaterialColorScheme.DesignName) {
					return new MaterialColorScheme(dict);
				} else if (disign == WearColorScheme.DesignName) {
					return new WearColorScheme(dict);
				}
			}

			return new ColorScheme(dict);
		}
	}
}
