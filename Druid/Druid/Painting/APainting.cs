using System;
using Dwares.Dwarf;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public abstract class APainting : IPainting
	{
		//static ClassRef @class = new ClassRef(typeof(APainting));

		public APainting()
		{
			//Debug.EnableTracing(@class);
		}

		public APainting(SKSizeI defaultSize)
		{
			//Debug.EnableTracing(@class);
			DefaultSize = defaultSize;
		}

		public SKSize DefaultSize { get; set; }

		public abstract void Render(SKCanvas canvas, SKRect rect);

		public virtual SKBitmap ToBitmap()
		{
			return ToBitmap((int)DefaultSize.Width, (int)DefaultSize.Height);
		}

		public virtual SKBitmap ToBitmap(int width, int height)
		{
			var bitmap = new SKBitmap(width, height);
			using (var canvas = new SKCanvas(bitmap)) {
				Render(canvas, new SKRect(0, 0, width, height));
			}
			return bitmap;
		}
	}
}
