using System;
using System.Collections;
using SkiaSharp;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using System.Collections.Generic;

namespace Dwares.Druid.Painting
{
	public class TextFit : Fitter<SKPaint>
	{
		public static ClassRef @class => new ClassRef(typeof(TextFit));

		public float? MinTextSize { get; set; }
		public float? MaxTextSize { get; set; }
		public float Step { get; set; } = 1;
		public uint Options { get; set; }

		public TextFit(FitMode mode = FitMode.Fill, uint options = FontMeasurementOptions.Maximum) : 
			base(mode)
		{
			Options = options;

			//Debug.EnableTracing(@class);
			//Debug.EnableTracing(Extensions.@class);
		}

		protected override bool Shrink(ref SKPaint paint)
		{
			float minTextSize = MinTextSize ?? 1;
			if (paint.TextSize <= minTextSize)
				return false;

			paint.TextSize = paint.TextSize - Step;
			return true;

		}

		protected override bool TryExpand(SKPaint paint, out SKPaint newPaint)
		{
			float maxTextSize = MaxTextSize ?? float.MaxValue;
			newPaint = paint.Clone();
			newPaint.TextSize = paint.TextSize + Step;
			if (newPaint.TextSize > maxTextSize) {
				newPaint.Dispose();
				newPaint = null;
				return false;
			}

			return true;
		}

		public bool FitFontHeight(ref SKPaint paint, float height)
		{
			return Fit(ref paint, (_paint) => ProbeValue(_paint.FontHeight(Options), height));

		}

		public bool FitCharWidth(ref SKPaint paint, float width)
		{
			return Fit(ref paint, (_paint) => ProbeValue(_paint.CharacterWidth(Options), width));
		}

		//public bool FitTextLength(SKPaint paint, float length, string text)
		//{
		//	return Fit(ref paint, (_paint) => ProbeValue(_paint.MeasureText(text), length));
		//}

		//public bool FitTextLength(ref SKPaint paint, float length, params string[] @params)
		//{
		//	return Fit(ref paint, (_paint) => ProbeValue(_paint.MaxTextLength(@params), length));
		//}

		//public bool FitTextLength(ref SKPaint paint, float length, IEnumerable list)
		//{
		//	return Fit(ref paint, (_paint) => ProbeValue(_paint.MaxTextLength(list), length));
		//}

		public bool FitWidth(ref SKPaint paint, float width, string text)
		{
			return Fit(ref paint, (_paint) => ProbeValue(_paint.TextWidth(text), width));
		}

		public bool FitHeight(ref SKPaint paint, float height, string text)
		{
			return Fit(ref paint, (_paint) => ProbeValue(_paint.TextHeight(text), height));
		}

		public bool FitMaxWidth(ref SKPaint paint, float width, IEnumerable<string> list)
		{
			return Fit(ref paint, (_paint) => ProbeValue(_paint.MaxTextWidth(list), width));
		}

		public bool FitMaxHeight(ref SKPaint paint, float height, IEnumerable<string> list)
		{
			return Fit(ref paint, (_paint) => ProbeValue(_paint.MaxTextHeight(list), height));
		}

		public static ProbeResult ProbeSize(SKPaint paint, float width, float height, string text)
		{
			var textDim = paint.TextDimemsion(text);
			if (textDim.Width > width || textDim.Height > height)
				return ProbeResult.NeedShrink;
			if (textDim.Width < width && textDim.Height < height)
				return ProbeResult.CanExpand;
			return ProbeResult.Ok;
		}

		public bool FitSize(ref SKPaint paint, float width, float height, string text)
		{
			return Fit(ref paint, (_paint) => ProbeSize(_paint, width, height, text));
		}

		public bool FitSize(ref SKPaint paint, SKSize size, string text) 
		{
			return Fit(ref paint, (_paint) => ProbeSize(_paint, size.Width, size.Height, text));
		}
	}
}
