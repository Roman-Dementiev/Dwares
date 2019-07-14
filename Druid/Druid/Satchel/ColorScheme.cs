using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Satchel
{
	public class ColorScheme : IColorPalette
	{
		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

		Metadata metadata = new Metadata();

		public ColorScheme(string name, string design = null)
		{
			//Debug.EnableTracing(@class);
			Name = name;
			Design = design;

			Satchel.ColorPalette.AddNamedPalette(this);
		}

		public ColorScheme(IDictionary<string, object> resources, string name = null, string design = null) :
			this(name, design)
		{
			//Debug.EnableTracing(@class);

			if (resources != null) {
				Load(resources);
			}	
		}

		string name;
		public string Name {
			get => name;
			set {
				var oldName = name;
				name = value;
				Satchel.ColorPalette.OnNameChanged(this, oldName, value);
			}
		}
		public string Design { get; set; }

		//public virtual string DefaultVariant => null;

		public ColorCollection Colors { get; } = new ColorCollection();

		public IColorPalette ColorPalette { get; set; }

		//public static IColorPalette BindingPalette { get; set; }

		public void Load(IDictionary<string, object> resources)
		{
			//Colors.Load(dict, this, LoadProperty);
			ResourcesLoader.Load(resources, this, metadata, LoadValue);
			//BindingPalette = null;
		}

		static bool LoadValue(ColorScheme target, string key, object value)
		{
			//if (key == nameof(ColorPalette)) {
			//	//BindingPalette = 
			//	target.ColorPalette = value as IColorPalette;
			//	return true;
			//}

			return ColorCollection.LoadColor(target.Colors, key, value);
		}

		public virtual bool TryGetColor(string name, string variant, out Color color)
		{
			if (Colors.TryGetColor(name, variant, out color)) {
				return true;
			}

			if (ColorPalette != null) {
				var colorName = new ColorName(metadata.GetAsString(name));
				if (ColorPalette.TryGetColor(colorName, variant, out color))
					return true;
			}

			return false;
		}
	}
}
