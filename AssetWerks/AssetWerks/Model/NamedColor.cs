using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using SkiaSharp;
using Windows.UI;

namespace AssetWerks.Model
{
	public class NamedColor : TitleHolder
	{
		public NamedColor(string title, SKColor color) :
			base(title)
		{
			Color = color;
		}

		public NamedColor(string title, byte r, byte g, byte b, byte a):
			base(title)
		{
			Color = new SKColor(r, g, b, a);
		}

		public SKColor Color { get; }

		static List<NamedColor> list;
		public static List<NamedColor> List {
			get => LazyInitializer.EnsureInitialized(ref list, () => { 
				var list = new List<NamedColor>();
				Init(list);
				return list;
				});
		}

		static void Init(List<NamedColor> list)
		{
			var colors = typeof(Colors);
			var properties = colors.GetRuntimeProperties();
			foreach (var propertyInfo in properties) {
				//System.Diagnostics.Debug.WriteLine(propertyInfo.Name);
				var value = propertyInfo.GetMethod.Invoke(colors, null);
				if (value is Color color) {
					list.Add(new NamedColor(propertyInfo.Name, color.R, color.G, color.B, color.A));
				}
			}
		}
	}
}
