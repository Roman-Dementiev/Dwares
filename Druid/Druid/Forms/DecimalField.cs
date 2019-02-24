using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class DecimalField : NumberField<decimal>
	{
		public DecimalField(string name) : this(name, NumberPolarity.Any) { }

		public DecimalField(string name, NumberPolarity polarity, uint? precision = null) :
			base(name, polarity)
		{
			Precision = precision;
		}

		public uint? Precision { get; }
	}

	public class PositiveDecimalField : DecimalField
	{
		public PositiveDecimalField(string name) : this(name, null) { }

		public PositiveDecimalField(string name, uint? decimalPoints) :
			base(name, NumberPolarity.Positive, decimalPoints)
		{
		}
	}

	public class NonNegativeDecimalField : DecimalField
	{
		public NonNegativeDecimalField(string name) : this(name, null) { }

		public NonNegativeDecimalField(string name, uint? decimalPoints) :
			base(name, NumberPolarity.NonNegative, decimalPoints)
		{
		}
	}
}
