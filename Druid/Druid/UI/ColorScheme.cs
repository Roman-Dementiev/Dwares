//using System;
//using System.Collections.Generic;
//using Dwares.Dwarf;
//using Dwares.Druid.Satchel;
//using Xamarin.Forms;


//namespace Dwares.Druid.UI
//{
//	//[ContentProperty("Colors")]
//	public class ColorScheme : ResourceDictionary, IColorPalette
//	{
//		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

//		public ColorScheme()
//		{
//			//Debug.EnableTracing(@class);

//			//Metadata = new Metadata();
//			Colors = new ColorCollection();
//		}

//		string name;
//		public string Name {
//			get => name;
//			set {
//				if (value != name) {
//					ColorPalette.OnNameChanging(this, name, value);
//					name = value;
//				}
//			}
//		}
//		public string Design { get; set; }

//		//public Metadata Metadata { get; }

//		public ColorCollection Colors { get; }
		
//		public IColorPalette Palette { get; set; }


//		public bool TryGetColor(string name, string variant, out Color color)
//		{
//			return Colors.TryGetColor(name, variant, out color);
//		}
//	}
//}
