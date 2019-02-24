using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public enum NumberPolarity
	{
		Any,
		Positive,
		NonNegative,
		Negative,
		NonPositive
	}

	public class NumberField<T> : RangedField<T> where T : struct, IComparable
	{
		public NumberField(string name) : this(name, NumberPolarity.Any) { }

		public NumberField(string name, NumberPolarity polarity) :
			base(name)
		{
			Polarity = polarity;

			switch (polarity)
			{
			case NumberPolarity.Positive:
				LowBound = Numbers.Zero<T>();
				if (name != null) {
					//MsgValueOutOfRange =
					MsgInvalidEntryText = GetMessage(null, ValidationMessages.cMustBePositive);
				}
				break;

			case NumberPolarity.NonNegative:
				MinValue = Numbers.Zero<T>();
				if (name != null) {
					//MsgValueOutOfRange =
					MsgInvalidEntryText = GetMessage(null, ValidationMessages.cMustBeNonNegative);
				}
				break;

			case NumberPolarity.Negative:
				HighBound = Numbers.Zero<T>();
				break;

			case NumberPolarity.NonPositive:
				MaxValue = Numbers.Zero<T>();
				break;
			}
		}

		public NumberPolarity Polarity { get; protected set; }
	}

	public class PositiveNymbeFrield<T> : NumberField<T> where T : struct, IComparable
	{
		public PositiveNymbeFrield(string name) : 
			base(name, NumberPolarity.Positive)
		{
		}
	}

	public class NonNegativeNumberField<T> : NumberField<T> where T : struct, IComparable
	{
		public NonNegativeNumberField(string name) :
			base(name, NumberPolarity.NonNegative)
		{
		}
	}
}
