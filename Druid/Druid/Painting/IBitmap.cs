using System;
using Xamarin.Forms;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public interface IBitmap : IPicture
	{
		SKBitmap SKBitmap { get; }
		new BitmapInfo Info { get; }

		//IBitmap Recolor(Func<Color, Color> func);
		IBitmap Recolor(Color srcColor, Color newColor, bool keepAlpha);
		IBitmap Resize(int newWidth, int newHeight);
	}

	public class BitmapInfo : PictureInfo
	{
		/*public*/ const double DefaultResolution = 1; // TODO

		public BitmapInfo(int bitmapWidth, int bitmapHeight, double resolution = DefaultResolution, Color? color = null)
		{
			if (bitmapWidth < 0)
				bitmapWidth = 0;
			if (bitmapHeight < 0)
				bitmapHeight = 0;
			if (resolution <= 0)
				resolution = DefaultResolution;
			if (color == default(Color))
				color = null;

			BitmapSize = new SKSizeI(bitmapWidth, bitmapHeight);
			Resolution = resolution;
			Color = color;
			Width = bitmapWidth / resolution;
			Height = bitmapHeight / resolution;
		}

		public SKSizeI BitmapSize { get; set; }
		public double Resolution { get; set; } // DPI?
	}

	public static partial class Extensions
	{
		//public static IBitmap Recolor(this IBitmap bitmap, Color srcColor, Color newColor, bool keepAlpha = true)
		//{
		//	var source = bitmap?.SKBitmap;
		//	if (source == null || newColor == srcColor)
		//		return bitmap;

		//	if (keepAlpha) {
		//		return bitmap.Recolor((color) => {
		//			if (color.R == srcColor.R && color.G == srcColor.G && color.B == srcColor.B) {
		//				return new Color(newColor.R, newColor.G, newColor.B, color.A);
		//			} else {
		//				return color;
		//			}
		//		});
		//	} else {
		//		return bitmap.Recolor((color) => (color == srcColor) ? newColor : color);
		//	}
		//}

		public static IBitmap Resize(this IBitmap bitmap, Size newSize)
		{
			int newWidth = (int)Math.Round(newSize.Width);
			int newHeight = (int)Math.Round(newSize.Height);
			return bitmap.Resize(newWidth, newHeight);
		}
	}

}
