using System;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public interface IPainting
	{
		SKSize DefaultSize { get; }

		void Render(SKCanvas canvas, SKRect rect);

		SKBitmap ToBitmap();
		SKBitmap ToBitmap(int width, int height);
	}
}
