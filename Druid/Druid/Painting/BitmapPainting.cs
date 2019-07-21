using System;
using Dwares.Dwarf;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public class BitmapPainting : APainting
	{
		//static ClassRef @class = new ClassRef(typeof(ButmapPainting));

		public BitmapPainting(SKBitmap bitmap)
		{
			//Debug.EnableTracing(@class);

			Guard.ArgumentNotNull(bitmap, nameof(bitmap));
			DefaultSize = new SKSizeI(bitmap.Width, bitmap.Height);
			Bitmap = bitmap;
		}

		public SKBitmap Bitmap { get; }

		public override void Render(SKCanvas canvas, SKRect rect)
		{
			canvas.DrawBitmap(Bitmap, rect);
		}

		//public override SKBitmap ToBitmap()
		//{
		//	return Bitmap;
		//}

		public override SKBitmap ToBitmap(int width, int height)
		{
			if (width == Bitmap.Width && height == Bitmap.Height) {
				return Bitmap;
			} else {
				return base.ToBitmap(width, height);
			}
		}
	}
}
