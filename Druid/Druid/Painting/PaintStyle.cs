using System;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public abstract class PaintStyle
	{
		public static bool DefaultAntialis { get; set; } = true;
		public static bool DefaultDither { get; set; } = false;

		public abstract SKPaintStyle SKPaintStyle { get; }

		public SKColor? Color { get; set; }
		public bool? Antialias { get; set; }
		public bool? Dither { get; set; }

		public virtual SKPaint GetPaint()
		{
			var paint = new SKPaint() { Style = SKPaintStyle };
			Prepare(paint);
			return paint;
		}

		protected virtual void Prepare(SKPaint paint)
		{
			paint.Color = Color ?? SKColors.Transparent;
			paint.IsAntialias = Antialias ?? DefaultAntialis;
			paint.IsDither = Dither ?? DefaultDither;
		}

		public static implicit operator SKPaint(PaintStyle style) => style?.GetPaint();
	}
}
