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
		public static bool TryGetColor(this IColorPalette palette, ColorName colorName, string variant, out Color color)
		{
			if (colorName.IsValid) {
				if (string.IsNullOrEmpty(variant))
					variant = colorName.Variant;

				return palette.TryGetColor(colorName.Name, variant, out color);
			} else {
				color = default;
				return false;	
			}
		}
	
		public static Color GetColor(this IColorPalette palette, string name, string variant = null, Color defaultValue = default)
		{
			Color color;
			if (palette.TryGetColor(name, variant, out color)) {
				return color;
			} else {
				return defaultValue;
			}
		}

		public static Color GetColor(this IColorPalette palette, ColorName colorName, string variant = null, Color defaultValue = default)
		{
			if (colorName.IsValid) {
				Color color;
				if (string.IsNullOrEmpty(variant))
					variant = colorName.Variant;
					
				if (palette.TryGetColor(colorName.Name, variant, out color))
					return color;
			}

			return defaultValue;
		}

	}


	public class ColorPalette : ColorCollection, IColorPalette
	{
		//static ClassRef @class = new ClassRef(typeof(ColorPalette));
		static Dictionary<string, IColorPalette> namedPalettes = new Dictionary<string, IColorPalette>();

		public ColorPalette(string name, string design)
		{
			//Debug.EnableTracing(@class);
			Name = name;
			Design = design;
			
			AddNamedPalette(this);
		}

		public ColorPalette(IDictionary<string, object> resources, string name = null, string design = null) :
			this(name, design)
		{
			if (resources != null) {
				//Load(dict, this);
				ResourcesLoader.Load(resources, this, LoadColor);
			}
		}

		string name;
		public string Name {
			get => name;
			set {
				var oldName = name;
				name = value;
				OnNameChanged(this, oldName, value);
			}
		}
		public string Design { get; set; }

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

		public static void OnNameChanged(IColorPalette palette, string oldName, string newName)
		{
			if (newName == oldName)
				return;

			if (!string.IsNullOrEmpty(oldName))
				namedPalettes.Remove(oldName);

			if (!string.IsNullOrEmpty(newName))
				namedPalettes[newName] = palette;
		}

		public static T GetInstance<T>(string name) where T : IColorPalette, new()
		{
			var palette = ByName(name);
			if (palette is T result) {
				return result;
			} else {
				return new T();
			}
		}
	}
}
