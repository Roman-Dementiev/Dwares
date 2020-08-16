using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public enum PhoneType
	{
		Default,
		Mobile,
		Home,
		Work
	}

	public struct PhoneNumber
	{
		public const char chCountryCode = 'c';
		public const char chAreaCode = 'a';
		public const char chDigit = 'd';

		public PhoneNumber(string number, PhoneType type = PhoneType.Default)
		{
			Number = number;
			PhoneType = type;
		}

		// TODO
		public bool IsValid => !string.IsNullOrEmpty(Number);

		public string Number { get; set; }
		public PhoneType PhoneType { get; set; }

		//public int CountryCode => ParseText(Number).CountryCode;
		//public int AreaCode => ParseText(Number).AreaCode;
		//public int LocalNumber => ParseText(Number).LocalNumber;
		//public int Extension => ParseText(Number).Extension;

		public override string ToString() => Number;

		public static implicit operator PhoneNumber(string number) 
			=> new PhoneNumber(number);
		
		public static implicit operator string(PhoneNumber number)
			=> number.ToString();

		public static PhoneNumber Parse(object number)
		{
			if (number is PhoneNumber) {
				return (PhoneNumber)number;
			} else {
				return new PhoneNumber(number.ToString());
			}
		}

		// TODO
		//public static SPhoneNumber ParseText(string text)
		//	=> throw new NotImplementedException();
	}

	//public struct SPhoneNumber
	//{
	//	public int CountryCode { get; set; }
	//	public int AreaCode { get; set; }
	//	public int LocalNumber { get; set; }
	//	public int Extension { get; set; }
	//}
}
