using System;
using System.Reflection;
using SkiaSharp;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Painting
{
	public static class Bitmaps
	{
		public static SKBitmap LoadBitmap(Assembly assembly, string resourceName)
		{
			try {
				using (var stream = assembly.GetManifestResourceStream(resourceName)) {
					return SKBitmap.Decode(stream);
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}

		public static SKBitmap LoadBitmap(Type type, string resourceName)
			=> LoadBitmap(type.GetTypeInfo().Assembly, resourceName);

		public static SKBitmap LoadBitmap(PackageUnit package, string resourceName)
			=> LoadBitmap(package.Assembly, resourceName);

		public static SKBitmap LoadBitmap(ResourceId resourceId)
			=> LoadBitmap(resourceId.Assembly, resourceId.ResourceName);


		public static SKBitmap Recolor(this SKBitmap source, Func<SKColor, SKColor> func)
		{
			int width = source.Width;
			int height = source.Height;
			SKBitmap result = new SKBitmap(width, height);

			for (int row = 0; row < height; row++) {
				for (int col = 0; col < width; col++) {
					var color = func(source.GetPixel(col, row));

					result.SetPixel(col, row, color);
				}
			}

			return result;
		}

		public static SKBitmap Recolor(this SKBitmap source, SKColor srcColor, SKColor newColor, bool keepAlpha = true)
		{
			if (keepAlpha) {
				return Recolor(source, (color) => {
					if (color.Red == srcColor.Red && color.Green == srcColor.Green && color.Blue == srcColor.Blue) {
						return new SKColor(newColor.Red, newColor.Green, newColor.Blue, color.Alpha);
					} else {
						return color;
					}
				});
			} else {
				return Recolor(source, (color) => (color == srcColor) ? newColor : color);
			}
		}

		public static SKBitmap Resize(this SKBitmap source, int newWidth, int newHeight)
		{
			var result = new SKBitmap(newWidth, newHeight);
			using (var canvas = new SKCanvas(result)) {
				canvas.DrawBitmap(source, new SKRect(0, 0, newWidth, newHeight));
			}
			return result;
		}
	}
}
