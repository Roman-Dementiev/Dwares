using System;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf
{ 
	public static class Numbers
	{
		public const uint MaxDecimalPrecision = 28;

		public static T Zero<T>() where T : struct
		{
			return (T)Convert.ChangeType(0, typeof(T));
		}

		public static T One<T>() where T : struct
		{
			return (T)Convert.ChangeType(1, typeof(T));
		}

		public static decimal MinDecimal(uint precision = MaxDecimalPrecision)
		{
			if (precision > MaxDecimalPrecision)
				precision = MaxDecimalPrecision;

			decimal value = DivideNTimes(1m, precision, 10);
			return value;
		}

		public static decimal MultiplyNTimes(decimal value, uint N, decimal factor)
		{
			for (int i = 0; i < N; i++)
				value = value * factor;
			return value;
		}

		public static decimal DivideNTimes(decimal value, uint N, decimal divider)
		{
			for (int i = 0; i < N; i++)
				value = value / divider;
			return value;
		}
	}

	public struct FixedPointNumber : IValueHolder<decimal>, IComparable, IConvertible
	{
		public static FixedPointNumber Zero = new FixedPointNumber(0m, null);

		public FixedPointNumber(uint? decimalPlaces) : this(0, decimalPlaces) { }

		public FixedPointNumber(decimal value, uint? precision = null)
		{
			_value = value;
			Precision  = precision;
		}

		decimal _value;
		public decimal Value { 
			get => _value;
			set {
				if (Precision  == null) {
					_value = value;;
				} else {
					_value = Math.Round(value, (int)Decimals);
				}
			}
		}

		public uint? Precision  { get; }

		public uint Decimals {
			get => (Precision  == null || Precision  > Numbers.MaxDecimalPrecision) ? Numbers.MaxDecimalPrecision : (uint)Precision;
		}

		public override string ToString()
		{
			if (Precision  == null) {
				return Value.ToString();
			} else {
				var format = string.Format("F{0}", Precision);
				return Value.ToString(format);
			}
		}

		public string ToString(string format) => Value.ToString(format);
		public string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);

		public static implicit operator decimal(FixedPointNumber number) => number.Value;
		public static implicit operator FixedPointNumber(decimal value) => new FixedPointNumber(value);

		public int CompareTo(object obj)
		{
			decimal objValue;
			if (obj is FixedPointNumber fp) {
				objValue = fp.Value;
			} else if (obj is IConvertible convertible) {
				objValue = Convert.ToDecimal(convertible);
			} else {
				throw new ArgumentException();
			}

			return Value.CompareTo(objValue);
;		}

		public TypeCode GetTypeCode() => TypeCode.Decimal;
		public decimal ToDecimal(IFormatProvider provider) => Value;
		public bool ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value, provider);
		public byte ToByte(IFormatProvider provider) => Convert.ToByte(Value, provider);
		public char ToChar(IFormatProvider provider) => Convert.ToChar(Value, provider);
		public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Value, provider);
		public double ToDouble(IFormatProvider provider) => Convert.ToDouble(Value, provider);
		public short ToInt16(IFormatProvider provider) => Convert.ToInt16(Value, provider);
		public int ToInt32(IFormatProvider provider) => Convert.ToInt32(Value, provider);
		public long ToInt64(IFormatProvider provider) => Convert.ToInt64(Value, provider);
		public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(Value, provider);
		public float ToSingle(IFormatProvider provider) => Convert.ToSingle(Value, provider);
		public string ToString(IFormatProvider provider) => Convert.ToString(Value, provider);
		public object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);
		public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value, provider);
		public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value, provider);
		public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value, provider);
	}
}
