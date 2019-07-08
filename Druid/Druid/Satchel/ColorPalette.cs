using System;
using System.Collections.Generic;
using System.Reflection;
using Dwares.Dwarf;
using Xamarin.Forms;

namespace Dwares.Druid.Satchel
{
	public interface IColorPalette
	{
		string Name { get; }
		string Design { get; }

		bool TryGetColor(string name, string variant, out Color color);
	}

	public static partial class Extensions
	{
		public static Color GetColor(this IColorPalette palette, string name, string variant = null, Color defaultValue = default)
		{
			Color color;
			if (palette.TryGetColor(name, variant, out color)) {
				return color;
			} else {
				return defaultValue;
			}
		}
	}


	public class ColorPalette : ColorCollection, IColorPalette
	{
		//static ClassRef @class = new ClassRef(typeof(ColorPalette));
		static Dictionary<string, IColorPalette> namedPalettes = new Dictionary<string, IColorPalette>();

		public ColorPalette(string name, string design) :
			base(name, design)
		{
			//Debug.EnableTracing(@class);
			AddNamedPalette(this);
		}

		public static IColorPalette ByName(string name)
		{
			if (!string.IsNullOrEmpty(name) && namedPalettes.TryGetValue(name, out var palette)) {
				return palette;
			} else {
				return null;
			}
		}

		public static void AddNamedPalette(IColorPalette palette)
		{
			if (!string.IsNullOrEmpty(palette.Name)) {
				namedPalettes[palette.Name] = palette;
			}
		}
	}
}
