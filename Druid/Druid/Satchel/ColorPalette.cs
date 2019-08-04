using System;
using System.Collections.Generic;
using System.Reflection;
using Dwares.Druid.Resources;
using Dwares.Dwarf;
using Xamarin.Forms;

namespace Dwares.Druid.Satchel
{
	[ContentProperty("Resources")]
	public class ColorPalette /* : ColorCollection, IColorPalette */
	{
		//static ClassRef @class = new ClassRef(typeof(ColorPalette));
		static Dictionary<string, ColorPalette> namedPalettes = new Dictionary<string, ColorPalette>();

		public ColorPalette()
		{
			Resources = new ResourceDictionary();
		}

		public ColorPalette(ResourceDictionary resources)
		{
			Guard.ArgumentNotNull(resources, nameof(resources));

			Resources = resources;
		}

		public ResourceDictionary Resources { get; set; }

		public string Design { get; set; }
		public string DefaultVariant { get; set; }

		string name;
		public string Name {
			get => name;
			set => ChangeName(name, value);
		}

		void ChangeName(string oldName, string newName)
		{
			if (newName == oldName)
				return;

			if (!string.IsNullOrEmpty(oldName))
				namedPalettes.Remove(oldName);

			name = newName;

			if (!string.IsNullOrEmpty(newName))
				namedPalettes[newName] = this;
		}

		protected virtual bool TryGetColor(object value, out Color color)
		{
			if (value is Color colorValue) {
				color = colorValue;
				return true;
			} else {
				color = default;
				return false;
			}
		}

		public bool TryGetColor(ColorName colorName, out Color color)
		{
			if (colorName.IsValid) {
				object value;
				if (Resources.TryGetValue(colorName, out value)) {
					return TryGetColor(value, out color);
				}
			}

			color = default;
			return false;
		}

		public Color GetColor(string name, Color defaultValue = default)
		{
			return GetColor(new ColorName(name), defaultValue);
		}

		public Color GetColor(ColorName colorName, Color defaultValue = default)
		{
			if (colorName.IsValid) {
				Color color;
				if (TryGetColor(colorName, out color))
					return color;

				if (colorName.Variant == null && DefaultVariant != null) {
					colorName = new ColorName { Name = colorName.Name, Variant = DefaultVariant };
					if (TryGetColor(colorName, out color))
						return color;
				}
			}

			return defaultValue;
		}

		public static ColorPalette ByName(string name)
		{
			if (!string.IsNullOrEmpty(name) && namedPalettes.TryGetValue(name, out var palette)) {
				return palette;
			} else {
				return null;
			}
		}

		public static T GetInstance<T>(string name) where T : ColorPalette, new()
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
