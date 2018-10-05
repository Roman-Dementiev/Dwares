using System;
using SkiaSharp;

namespace Dwares.Druid.Painting
{
	public class BaseTextStyle : PaintStyle
	{
		protected BaseTextStyle(SKPaintStyle paintStyle)
		{
			SKPaintStyle = paintStyle;
		}

		public override SKPaintStyle SKPaintStyle { get; }

		public string FamilyName { get; }
		public float? TextSize { get; set; }
		public SKTextAlign? TextAlign { get; set; }
		public SKTypefaceStyle? TypefaceStyle { get; set; }
		public SKFontStyleWeight? FontStyleWeight { get; set; }
		public SKFontStyleWidth? FontStyleWidth { get; set; }
		public SKFontStyleSlant? FontSlant { get; set; }
		//TODO
		//public float TextScaleX { get; set; } 
		//public float TextSkewX { get; set; }

		protected override void Prepare(SKPaint paint)
		{
			base.Prepare(paint);

			if (FamilyName != null) {
				if (FontStyleWeight != null || FontStyleWidth != null || FontSlant != null) {
					paint.Typeface = SKTypeface.FromFamilyName(
						FamilyName,
						FontStyleWeight ?? SKFontStyleWeight.Normal,
						FontStyleWidth ?? SKFontStyleWidth.Normal,
						FontSlant ?? SKFontStyleSlant.Upright);
				} else {
					paint.Typeface = SKTypeface.FromFamilyName(
						FamilyName, 
						TypefaceStyle ?? SKTypefaceStyle.Normal);
				}
			}
			if (TextSize != null) {
				paint.TextSize = (int)TextSize;
			}
			if (TextAlign != null) {
				paint.TextAlign = (SKTextAlign)TextAlign;
			}
		}
	}

	public class TextStyle : BaseTextStyle
	{
		public TextStyle() : base(SKPaintStyle.Fill) { }
	}

	public class TextStrokeStyle : BaseTextStyle
	{
		public TextStrokeStyle() : base(SKPaintStyle.Stroke) { }
	}
}
