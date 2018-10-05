using System;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public class StrokeStyle: PaintStyle
	{
		public float? Width { get; set; }
		public float? Miter { get; set; }
		public SKStrokeCap? Cap { get; set; }
		public SKStrokeJoin? Join { get; set; }
		//TODO

		public override SKPaintStyle SKPaintStyle => SKPaintStyle.Stroke;

		protected override void Prepare(SKPaint paint)
		{
			base.Prepare(paint);
		
			if (Width != null) {
				paint.StrokeWidth = (float)Width;
			}
			if (Miter != null) {
				paint.StrokeMiter = (float)Miter;
			}
			if (Cap != null) {
				paint.StrokeCap = (SKStrokeCap)Cap;
			}
			if (Join != null) {
				paint.StrokeJoin = (SKStrokeJoin)Join;
			}
		}
	}
}
