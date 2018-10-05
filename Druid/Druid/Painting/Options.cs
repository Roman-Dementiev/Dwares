using System;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Painting
{
	public struct FontMeasurementOptions
	{
		public const uint Maximum = 0;
		public const uint RecomendedFontHeight = 1;
		public const uint AverageCharWidth = 2;
		public const uint AboveBaselineOnly = 4;
		public const uint Approximate = RecomendedFontHeight | AverageCharWidth;
	}

	public struct PaintingOptions
	{
		public PaintingOptions(uint value)
		{
			options = value;
		}

		uint options;
		public uint Value {
			get => options;
			set => options = value;
		}

		public bool MaximumFontMeasurement {
			get => Flags.AllAreOff(options, FontMeasurementOptions.Approximate);
			//set => Flags.Turn(ref options, FontMeasurementOptions.Approximate, !value);
		}
		public bool ApproximateFontMeasurement {
			get => Flags.AllAreOn(options, FontMeasurementOptions.Approximate);
			//set => Flags.Turn(ref options, FontMeasurementOptions.Approximate, value);
		}
		public bool RecomendedFontHeight {
			get => Flags.AnyIsOn(Value, FontMeasurementOptions.RecomendedFontHeight);
			set => Flags.Turn(ref options, FontMeasurementOptions.RecomendedFontHeight, value);
		}
		public bool AverageCharWidth {
			get => Flags.AnyIsOn(Value, FontMeasurementOptions.AverageCharWidth);
			set => Flags.Turn(ref options, FontMeasurementOptions.AverageCharWidth, value);
		}
		public bool AboveBaselineOnly {
			get => Flags.AnyIsOn(Value, FontMeasurementOptions.AboveBaselineOnly);
			set => Flags.Turn(ref options, FontMeasurementOptions.AboveBaselineOnly, value);
		}

		public static implicit operator PaintingOptions(uint value) => new PaintingOptions(value);
	}
}
