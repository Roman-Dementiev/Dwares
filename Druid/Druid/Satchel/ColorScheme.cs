using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Satchel
{
	public class ColorScheme : ColorPalette
	{
		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

		//public ColorScheme() { }
		public ColorScheme(ResourceDictionary resources) : 
			base(resources)
		{
			Load(resources);
		}

		//public ColorScheme(string name, string design = null)
		//{
		//	//Debug.EnableTracing(@class);

		//	Name = name;
		//	Design = design;

		//	//Satchel.ColorPalette.AddNamedPalette(this);
		//}

		//public ColorScheme(IDictionary<string, object> resources, string name = null, string design = null) :
		//	this(name, design)
		//{
		//	if (resources != null) {
		//		Load(resources);
		//	}	
		//}

		//string name;
		//public string Name {
		//	get => name;
		//	set {
		//		if (value != name) {
		//			var oldName = name;
		//			name = value;
		//			//Satchel.ColorPalette.OnNameChanged(this, oldName, value);
		//		}
		//	}
		//}
		//public string Design { get; set; }

		//public virtual string DefaultVariant => null;

		//public ColorCollection Colors { get; } = new ColorCollection();

		//public static IColorPalette BindingPalette { get; set; }

		public void Load(IDictionary<string, object> resources)
		{
			ResourcesLoader.Load(resources, this);
			//BindingPalette = null;
		}

		//static bool LoadValue(ColorScheme target, string key, object value)
		//{
		//	//if (key == nameof(ColorPalette)) {
		//	//	//BindingPalette = 
		//	//	target.ColorPalette = value as IColorPalette;
		//	//	return true;
		//	//}

		//	return ColorCollection.LoadColor(target.Colors, key, value);
		//}

		//public virtual bool TryGetColor(string name, string variant, out Color color)
		//{
		//	if (Colors.TryGetColor(name, variant, out color)) {
		//		return true;
		//	}

		//	if (ColorPalette != null) {
		//		var colorName = new ColorName(metadata.GetAsString(name));
		//		if (!string.IsNullOrEmpty(colorName)) {
		//			if (ColorPalette.TryGetColor(colorName, variant, out color))
		//				return true;
		//		} else {
		//			if (ColorPalette.TryGetColor(name, variant, out color))
		//				return true;
		//		}


		//	}

		//	return false;
		//}

		//IColorPalette colorPalette;
		//public IColorPalette ColorPalette { 
		//	get {
		//		if (colorPalette == null) {
		//			colorPalette = ByDesign(Design);
		//		}
		//		return colorPalette;
		//	} 

		//	set {
		//		colorPalette = value;
		//	}
		//}

		public ColorPalette ColorPalette { get; set; }

		protected override bool TryGetColor(object value, out Color color)
		{
			if (ColorPalette != null) {
				if (value is string name) {
					return ColorPalette.TryGetColor(new ColorName(name), out color);
				}
				else if (value is ColorName colorName) {
					return ColorPalette.TryGetColor(colorName, out color);
				}
			} 

			return base.TryGetColor(value, out color);
		}


		public Color Primary {
			get => GetColor(nameof(Primary));
		}

		public Color PrimaryLight {
			get => GetColor(nameof(PrimaryLight));
		}

		public Color PrimaryDark {
			get => GetColor(nameof(PrimaryDark));
		}

		public Color Secondary {
			get => GetColor(nameof(Secondary));
		}

		public Color SecondaryLight {
			get => GetColor(nameof(SecondaryLight));
		}

		public Color SecondaryDark {
			get => GetColor(nameof(SecondaryDark));
		}

		public Color Background {
			get => GetColor(nameof(Background));
		}

		public Color Surface {
			get => GetColor(nameof(Surface));
		}

		public Color Error {
			get => GetColor(nameof(Error));
		}

		public Color OnPrimary {
			get => GetColor(nameof(OnPrimary));
		}

		public Color OnSecondary {
			get => GetColor(nameof(OnSecondary));
		}

		public Color OnSurface {
			get => GetColor(nameof(OnSurface));
		}

		public Color OnError {
			get => GetColor(nameof(OnError));
		}

		public Color Accent {
			get => GetColor(nameof(Accent));
		}
		public Color Inverted {
			get => GetColor(nameof(Inverted));
		}
		public Color OnInverted {
			get => GetColor(nameof(OnInverted));
		}
	}
}
