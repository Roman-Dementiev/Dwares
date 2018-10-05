using System;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public class FillStyle: PaintStyle
	{
		//TODO

		public override SKPaintStyle SKPaintStyle => SKPaintStyle.Fill;

		protected override void Prepare(SKPaint paint)
		{
			base.Prepare(paint);

			//paint.StrokeWidth = 0;
			//TODO
		}
	}

}
