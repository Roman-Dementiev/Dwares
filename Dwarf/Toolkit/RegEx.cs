using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace Dwares.Dwarf.Toolkit
{
	public enum PhoneFormat
	{
		Default,
		USAOnly,
		Extended
	}

	public static class RegEx
	{
		const string EolPattern = "(\r?\n|\r)";
		const string EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
		const string PhonePattern = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
		const string USAPhonePattern = @"^(\+0?1\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
		const string ExtPhonePattern = @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$";

		static Regex eol;
		public static Regex Eol => LazyInit(ref eol, EolPattern);

		static Regex email;
		public static Regex Email => LazyInit(ref email, EmailPattern);

		static Regex phone;
		public static Regex Phone => LazyInit(ref phone, PhonePattern);

		static Regex usaPhone;
		public static Regex USAPhone => LazyInit(ref usaPhone, USAPhonePattern);

		static Regex extPhone;
		public static Regex ExtPhone => LazyInit(ref extPhone, ExtPhonePattern);

		public static bool IsValidPattern(string pattern)
		{
			Regex regex;
			return IsValidPattern(pattern, out regex);
		}

		public static bool IsValidPattern(string pattern, out Regex regex)
		{
			try {
				regex = new Regex(pattern);
				return true;
			}
			catch (Exception ex) {
				regex = null;
				return false;
			}
		}

		public static Regex PhoneRegex(PhoneFormat format)
		{
			switch (format)
			{
			case PhoneFormat.USAOnly:
				return USAPhone;
			
			case PhoneFormat.Extended:
				return ExtPhone;
			
			default:
				return Phone;
			}
		}

		static Regex LazyInit(ref Regex regex, string pattern)
		{
			return LazyInitializer.EnsureInitialized(ref regex, () => new Regex(pattern));
		}

	}
}
