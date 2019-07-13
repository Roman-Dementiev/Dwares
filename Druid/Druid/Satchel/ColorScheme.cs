using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Satchel
{
	public class ColorScheme : BindableObject, IColorPalette
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

		public ColorScheme(IDictionary<string, object> dict, string name = null, string design = null) :
			this(name, design)
		{
			//Debug.EnableTracing(@class);

			if (dict != null) {
				Load(dict);
			}
		}

		public string Name { get; }
		public string Design { get; }

		public ColorCollection Colors { get; } = new ColorCollection();

		public virtual string DefaultVariant => null;


		IColorPalette colorPalette;
		public IColorPalette ColorPalette {
			get => colorPalette;
			set {
				if (value != colorPalette) {
					OnPropertyChanging();
					if (BindingContext == null || BindingContext == colorPalette) {
						BindingContext = colorPalette = value;
					} else {
						colorPalette = value;
					}
					OnPropertyChanged();
				}
			}
		}


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
				if (!string.IsNullOrEmpty(colorName) && ColorPalette.TryGetColor(colorName, variant ?? DefaultVariant, out color)) {
					return true;
				}
			}

			return false;
		}
	}
}
