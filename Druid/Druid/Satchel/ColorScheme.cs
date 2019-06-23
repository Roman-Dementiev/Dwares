//using System;
//using System.Collections.Generic;
//using Xamarin.Forms;
//using Dwares.Dwarf;


//namespace Dwares.Druid.Satchel
//{
//	public class ColorScheme
//	{
//		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

//		static Dictionary<string, ColorScheme> namedSchemes = new Dictionary<string, ColorScheme>();
//		Dictionary<string, Color> colors = new Dictionary<string, Color>();

//		public ColorScheme(string name = null)
//		{
//			//Debug.EnableTracing(@class);

//			Name = name;

//			if (!string.IsNullOrEmpty(name)) {
//				namedSchemes[name] = this;
//			}
//		}

//		public ColorScheme(ResourceDictionary dict, string name = null) :
//			this(name)
//		{
//			foreach (var pair in dict) {
//				if (pair.Value is Color color) {
//					AddColor(pair.Key, color);
//				}
//			}
//		}

//		public string Name { get; }

//		public ColorScheme ByName(string name)
//		{
//			ColorScheme scheme;
//			if (!string.IsNullOrEmpty(name) && namedSchemes.TryGetValue(name, out scheme)) {
//				return scheme;
//			} else {
//				return null;
//			}
//		}

//		public void AddColor(string key, Color color)
//		{
//			colors[key] = color;
//		}

//		public Color GetColor(string key)
//		{
//			if (!string.IsNullOrEmpty(key) && colors.ContainsKey(key)) {
//				return colors[key];
//			} else {
//				return Color.Transparent;
//			}
//		}

//		public Color BaseColor {
//			get => GetColor("BaseColor");
//		}
//		public Color MainColor {
//			get {
//				var color = GetColor("MainColor");
//				Debug.Print($"ColorScheme.MainColor={color}");
//				return color;
//			}
//		}
//		public Color AltColor {
//			get => GetColor("AltColor");
//		}

//		public Color Tint1 => GetTintColor(MainColor, BaseColor, 0.1);
//		public Color Tint2 => GetTintColor(MainColor, BaseColor, 0.1);
//		public Color Tint3 => GetTintColor(MainColor, BaseColor, 0.1);
//		public Color Tint4 => GetTintColor(MainColor, BaseColor, 0.2);
//		public Color Tint5 => GetTintColor(MainColor, BaseColor, 0.3);
//		public Color Tint6 => GetTintColor(MainColor, BaseColor, 0.4);
//		public Color Tint7 => GetTintColor(MainColor, BaseColor, 0.5);
//		public Color Tint8 => GetTintColor(MainColor, BaseColor, 0.6);
//		public Color Tint9 => GetTintColor(MainColor, BaseColor, 0.7);

//		public static Color GetTintColor(Color c1, Color c2,  double tint)
//		{
//			Debug.Assert(tint >= 0 && tint <= 1);

//			return new Color(
//				GetTint(c1.R, c2.R, tint),
//				GetTint(c1.G, c2.G, tint),
//				GetTint(c1.B, c2.B, tint),
//				GetTint(c1.A, c2.A, tint) // TODO
//				);
//		}

//		static double GetTint(double c1, double c2, double tint)
//		{
//			double t = c1 * tint + c2 * (1 - tint);
//			//if (t < 0) return 0;
//			//if (t > 1) return 1;
//			Debug.Assert(t >= 0 && t <= 1);
//			return t;
//		}
//	}
//}
