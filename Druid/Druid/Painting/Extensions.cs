using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using SkiaSharp;
using Dwares.Dwarf;
using System.Collections.Generic;

namespace Dwares.Druid.Painting
{
	public struct FontMeasurement
	{
		public float FontHeight { get; set; }
		public float AboveBaseline { get; set; }
		public float BelowBaseline { get; set; }
		public float CharacterWidth { get; set; }

		public override string ToString() => Strings.Properties(this);
	}

	public static partial class Extensions
	{
		public static ClassRef @class => new ClassRef(typeof(Extensions));

		public static FontMeasurement MeasureFont(this SKPaint paint, PaintingOptions options)
		{
			paint.GetFontMetrics(out var metrics);

			var measurement = new FontMeasurement();

			if (options.AverageCharWidth) {
				measurement.CharacterWidth = metrics.AverageCharacterWidth;
			} else {
				measurement.CharacterWidth = metrics.MaxCharacterWidth;
			}

			if (options.RecomendedFontHeight) {
				measurement.FontHeight = metrics.Descent - metrics.Ascent;
				measurement.AboveBaseline = -metrics.Ascent;
				measurement.BelowBaseline = metrics.Descent;
			} else {
				measurement.FontHeight = metrics.Bottom - metrics.Top;
				measurement.AboveBaseline = -metrics.Top;
				measurement.BelowBaseline = metrics.Bottom;
			}

			if (options.AboveBaselineOnly) {
				measurement.FontHeight = measurement.AboveBaseline;
				measurement.BelowBaseline = 0;
			}

		
			Debug.Trace(@class, nameof(MeasureFont), "TextSize={0} => {1}", paint.TextSize, measurement);

			return measurement;
		}

		public static float FontHeight(this SKPaint paint, PaintingOptions options)
			=> MeasureFont(paint, options).FontHeight;
		public static float MaxFontHeight(this SKPaint paint, bool maxHeight)
			=> MeasureFont(paint, FontMeasurementOptions.Maximum).FontHeight;
		public static float RecommendedFontHeight(this SKPaint paint, bool maxHeight)
			=> MeasureFont(paint, FontMeasurementOptions.RecomendedFontHeight).FontHeight;

		public static float CharacterWidth(this SKPaint paint, PaintingOptions options)
			=> MeasureFont(paint, options).CharacterWidth;
		public static float MaxCharacterWidth(this SKPaint paint, bool maxWidth)
			=> MeasureFont(paint, FontMeasurementOptions.Maximum).CharacterWidth;
		public static float AverageharacterWidtht(this SKPaint paint, bool maxWidth)
			=> MeasureFont(paint, FontMeasurementOptions.AverageCharWidth).CharacterWidth;

		public static float AboveBaseline(this SKPaint paint, PaintingOptions options)
			=> MeasureFont(paint, options).AboveBaseline;
		public static float BelowBaseline(this SKPaint paint, PaintingOptions options)
			=> MeasureFont(paint, options).BelowBaseline;


		public static Dim<float> TextDimemsion(this SKPaint paint, string text)
		{
			var bounds = new SKRect();
			var length = paint.MeasureText(text, ref bounds);
			var result = new Dim<float>(bounds.Width, bounds.Height);
			Debug.Trace(@class, nameof(TextDimemsion), "TextSize={0}, TextDimension={1}", 
				paint.TextSize, result);
			return result;
		}

		public static float TextWidth(this SKPaint paint, string text)
		{
			var bounds = new SKRect();
			var length = paint.MeasureText(text, ref bounds);
			var result = bounds.Width;
			Debug.Trace(@class, nameof(TextWidth), "TextSize={0}, TextWidth={1}",
				paint.TextSize, result);
			return result;
		}

		public static float TextHeight(this SKPaint paint, string text)
		{
			var bounds = new SKRect();
			var length = paint.MeasureText(text, ref bounds);
			var result = bounds.Height;
			Debug.Trace(@class, nameof(TextHeight), "TextSize={0}, TextHeight={1}",
				paint.TextSize, result);
			return result;
		}

		public static float MaxTextWidth(this SKPaint paint, IEnumerable<string> list)
		{
			var result = 0f;
			var bounds = new SKRect();
			foreach (var text in list) {
				paint.MeasureText(text, ref bounds);
				if (bounds.Width > result)
					result = bounds.Height;
			}
			Debug.Trace(@class, nameof(MaxTextWidth), "TextSize={0}, MaxTextWidth={1}",
				paint.TextSize, result);
			return result;
		}

		public static void DrawRectAround(this SKCanvas canvas, SKRect rect, SKPaint paint, Extent<float>? offset = null)
		{
			if (offset is Extent<float> off) {
				rect.Left -= off.Left;
				rect.Top -= off.Top;
				rect.Right += off.Right;
				rect.Bottom += off.Left;
			}
			rect.Inflate(paint.StrokeWidth / 2, paint.StrokeWidth / 2);
			canvas.DrawRect(rect, paint);
		}

		public static float MaxTextHeight(this SKPaint paint, IEnumerable<string> list)
		{
			var result = 0f;
			var bounds = new SKRect();
			foreach (var text in list) {
				paint.MeasureText(text, ref bounds);
				if (bounds.Height > result)
					result = bounds.Height;
			}
			Debug.Trace(@class, nameof(MaxTextHeight), "TextSize={0}, MaxTextHeight={1}",
				paint.TextSize, result);
			return result;
		}

		//public static void DrawCenteredText(
		//	this SKCanvas canvas, 
		//	string text,
		//	SKRect rect,
		//	SKPaint paint,
		//	PaintingOptions options,
		//	bool fit = true, 
		//	bool clip = false
		//	)
		//{
		//	if (clip) {
		//		canvas.Save();
		//		canvas.ClipRect(rect);
		//	}
		//	if (fit) {
		//		var textFit = new TextFit();
		//		textFit.FitSize(ref paint, rect.Width, rect.Height, text);
		//	}

		//	var dim = paint.TextDimemsion(text);
			
		//	float x, y;
		//	x = rect.Left + (rect.Width - dim.Width) / 2;
		//	y = rect.Top + (rect.Height + dim.Height) / 2; // + paint.AboveBaseline(FontMeasurementOptions.Approximate);
		//	canvas.DrawText(text, x, y, paint);

		//	if (clip) {
		//		canvas.Restore();
		//	}
		//}

	}
}
