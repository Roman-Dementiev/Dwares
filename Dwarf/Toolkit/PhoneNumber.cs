using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
		//public const char chCountryCode = 'c';
		//public const char chAreaCode = 'a';
		//public const char chDigit = 'd';

		public PhoneNumber(string number, PhoneType type = PhoneType.Default)
		{
			Number = number;
			PhoneType = type;
		}

		// TODO
		public bool IsValid {
			//get => !string.IsNullOrEmpty(Number);
			get => IsValidNumber(Number);
		}

		public string Number { get; set; }
		public PhoneType PhoneType { get; set; }

		public override string ToString() => Number;

		public static implicit operator PhoneNumber(string number) 
			=> new PhoneNumber(number);
		
		public static implicit operator string(PhoneNumber number)
			=> number.ToString();

		//public static PhoneNumber Parse(object number)
		//{
		//	if (number is PhoneNumber) {
		//		return (PhoneNumber)number;
		//	} else {
		//		return new PhoneNumber(number.ToString());
		//	}
		//}

		public static bool IsValidNumber(string number, bool usaOnly = false)
		{
			if (string.IsNullOrEmpty(number))
				return false;

			if (usaOnly) {
				return RegEx.UsaPhone.IsMatch(number);
			} else {
				return RegEx.Phone.IsMatch(number);
			}
		}

		//public static bool IsUsaPhone(string number)
		//{
		//	Match match = RegEx.Phone.Match(number);
		//}

		public static string Normalize(string number, bool usaOnly, bool addCountryCode = false)
		{
			if (string.IsNullOrEmpty(number))
				return string.Empty;

			if (RegEx.UsaPhone.IsMatch(number)) {
				number = RegEx.UsaPhone.Replace(number, "($2) $3-$4");
				if (addCountryCode) {
					number = "+1 " + number;
				}
			}
			else if (!usaOnly && RegEx.Phone.IsMatch(number)) {
				number = RegEx.UsaPhone.Replace(number, "$1 $2-$3-$4");
			}
			return number;
		}
	}

}
