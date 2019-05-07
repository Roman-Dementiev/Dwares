using System;
using System.IO;
using System.Threading.Tasks;
using SkiaSharp;

namespace AssetWerks
{
	public static class Skia
	{
		public static SKPaint FillPaint(SKColor color, bool isAntialias = true)
		{
			return new SKPaint {
				Style = SKPaintStyle.Fill,
				Color = color,
				IsAntialias = isAntialias
			};
		}

		static bool IsGray(int r, int g, int b, int a, int tolerance)
		{
			if (a <= tolerance)
				return true;

			return Math.Abs(r-b) <= tolerance && Math.Abs(r-g) <= tolerance && Math.Abs(g-b) <= tolerance;
		}

		public static bool IsGrayscale(SKBitmap bitmap, int tolerance=1)
		{
			if (bitmap == null)
				return false;

			int width = bitmap.Width;
			int height = bitmap.Height;

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					var pixel = bitmap.GetPixel(x, y);
					if (!IsGray(pixel.Red, pixel.Green, pixel.Blue, pixel.Alpha, tolerance)) {
						return false;
					}
				}
			}

			return true;
		}

		static byte ApplyColor(int src, int color)
			=> (byte)((255-src) * color / 255);

		public static SKImage ApplyColor(SKImage image, SKColor color)
		{
			int width = image.Width;
			int height = image.Height;
			var bitmap = new SKBitmap(width, height);

			using (var pixmap = image.PeekPixels()) {
				for (int y = 0; y < height; y++) {
					for (int x = 0; x < width; x++) {
						var pixel = pixmap.GetPixelColor(x, y);
						if (pixel.Alpha > 0) {
							byte r = ApplyColor(pixel.Red, color.Red);
							byte g = ApplyColor(pixel.Green, color.Green);
							byte b = ApplyColor(pixel.Blue, color.Blue);
							byte a = pixel.Alpha;

							bitmap.SetPixel(x, y, new SKColor(r, g, b, a));
						}
					}
				}
			}

			return SKImage.FromBitmap(bitmap);
		}
	}
}
