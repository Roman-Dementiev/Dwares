//using System;
//using Dwares.Dwarf;


//namespace Dwares.Dwarf.Toolkit
//{
//	public struct Currency : IValueHolder<decimal>
//	{
//		public Currency(decimal value)
//		{
//			Value = value;
//		}

//		public decimal Value { get; set; }

//		public override string ToString()
//		{
//			return Value.ToString("C");
//		}

//		public static Currency ToCurrency(object source)
//		{
//			if (source is Currency currency) {
//				return currency;
//			}

//			if (source is IConvertible convertible) {
//				var value = Convert.ToDecimal(source);
//				return new Currency(value);
//			}

//			throw new ArgumentException("Invalid argument in Currency.ToCurrency()", nameof(source));
//		}
//	}
//}
