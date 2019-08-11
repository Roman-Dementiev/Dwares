using System;
using Xamarin.Forms;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public interface IPicture
	{
		double DefaultWidth { get; }
		double DefaultHeight { get; }
		Size? DefaultSize { get; }
		Color? DefaultColor { get; }

		PictureInfo Info { get; }

		void Render(SKCanvas canvas, SKRect rect);

		SKBitmap ToBitmap();
		SKBitmap ToBitmap(int width, int height);
	}

	public class PictureInfo
	{
		public double Width { get; set; }
		public double Height { get; set; }
		public Color? Color { get; set; }
	}
}
