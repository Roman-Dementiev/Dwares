using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	//public class FixedPointField : NumberField<FixedPointNumber>
	//{
	//	public FixedPointField(string name) : this(name, NumberPolarity.Any, null) { }

	//	public FixedPointField(string name, NumberPolarity polarity) : this(name, polarity, null) { }

	//	public FixedPointField(string name, uint? precision) : this(name, NumberPolarity.Any, precision) { }

	//	public FixedPointField(string name, NumberPolarity polarity, uint? precision) :
	//		base(name)
	//	{
	//		Polarity = polarity;
	//		Value = new FixedPointNumber(precision);

	//		switch (polarity)
	//		{
	//		case NumberPolarity.Positive:
	//			LowBound = FixedPointNumber.Zero;
	//			if (name != null) {
	//				//MsgValueOutOfRange =
	//				MsgInvalidEntryText = GetMessage(null, ValidationMessages.cMustBePositive);
	//			}
	//			break;

	//		case NumberPolarity.NonNegative:
	//			MinValue = FixedPointNumber.Zero;
	//			if (name != null) {
	//				//MsgValueOutOfRange =
	//				MsgInvalidEntryText = GetMessage(null, ValidationMessages.cMustBeNonNegative);
	//			}
	//			break;

	//		case NumberPolarity.Negative:
	//			HighBound = FixedPointNumber.Zero;
	//			break;

	//		case NumberPolarity.NonPositive:
	//			MaxValue = FixedPointNumber.Zero;
	//			break;
	//		}
	//	}

	//	public uint? DecimalPoints { 
	//		get => Value?.Precision ;
	//	}

	//	public static implicit operator decimal(FixedPointField field)
	//	{
	//		if (field.Value != null) {
	//			return field.Value.Value;
	//		} else {
	//			return default(decimal);
	//		}
	//	}
	//}

	//public class PositiveDecimalField : FixedPointField
	//{
	//	public PositiveDecimalField(string name) : this(name, null) { }

	//	public PositiveDecimalField(string name, uint? decimalPoints) :
	//		base(name, NumberPolarity.Positive, decimalPoints)
	//	{
	//	}
	//}

	//public class NonNegativeDecimalField : FixedPointField
	//{
	//	public NonNegativeDecimalField(string name) : this(name, null) { }

	//	public NonNegativeDecimalField(string name, uint? decimalPoints) :
	//		base(name, NumberPolarity.NonNegative, decimalPoints)
	//	{
	//	}
	//}
}
