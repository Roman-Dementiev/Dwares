using System;
using System.Text.RegularExpressions;

namespace Dwares.Dwarf.Toolkit
{
	//public enum PhoneFormat
	//{
	//	Default,
	//	USAOnly,
	//	Extended
	//}

	public static class RegEx
	{
		const string EolPattern = "(\r?\n|\r)";
		const string EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
		const string PhonePattern = @"^(\+\d{1,2}\s)?\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$";
		const string UsaPhonePattern = @"^(\+0?1\s)?\(?(\d{3})\)?[\s.-]?(\d{3})[\s.-]?(\d{4})$";
		//const string ExtPhonePattern = @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$";

		public static Regex Eol {
			get => eol ??= new Regex(EolPattern, RegexOptions.Compiled);
		}
		static Regex eol;

		public static Regex Email {
			get => email ??= new Regex(EmailPattern, RegexOptions.Compiled);
		}
		static Regex email;

		public static Regex Phone {
			get => phone ??= new Regex(PhonePattern, RegexOptions.Compiled);
		}
		static Regex phone;

		public static Regex UsaPhone {
			get => usaPhone ??= new Regex(UsaPhonePattern, RegexOptions.Compiled);
		}
		static Regex usaPhone;

		//public static Regex ExtPhone {
		//	get => extPhone ??= new Regex(ExtPhonePattern, RegexOptions.Compiled);
		//}
		//static Regex extPhone;

		public static bool IsValidPattern(string pattern)
		{
			Regex regex;
			return IsValidPattern(pattern, out regex, false);
		}

		public static bool IsValidPattern(string pattern, out Regex regex, bool compile = true)
		{
			try {
				regex = new Regex(pattern, compile ? RegexOptions.Compiled : RegexOptions.None);
				return true;
			}
			catch (Exception exc) {
				regex = null;
				return false;
			}
		}

		//public static Regex PhoneRegex(PhoneFormat format)
		//{
		//	switch (format)
		//	{
		//	case PhoneFormat.USAOnly:
		//		return USAPhone;
			
		//	case PhoneFormat.Extended:
		//		return ExtPhone;
			
		//	default:
		//		return Phone;
		//	}
		//}
	}
}
