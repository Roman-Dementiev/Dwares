using System;
using System.Collections;
using System.Collections.Generic;
using Dwares.Dwarf.Runtime;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf
{
	public static class Strings
	{
		const string DefaultNamedValuesFormat = "{0}={1}";
		const string DefaultNamedValuesSeparator = ", ";
		const string DefaultUnnamedValuesFormat = "{0}";
		const string DefaultUnnamedValuesSeparator = ", ";
		const string DefaultProperiesPrefix = "{";
		const string DefaultProperiesSuffix = "}";

		public static List<string> EmptyList = new List<string>();
		public static string[] EmptyArray = new string[0];

		public static bool IsNullOrEmptyString(object obj)
		{
			if (obj == null)
				return true;

			if (obj is string str)
				return str.Length == 0;

			return false;
		}

		public static bool IsNullOrEmpty(object obj, bool whitespaceIsEmpty)
		{
			if (obj == null)
				return true;

			var str = obj.ToString();
			if (whitespaceIsEmpty) {
				return String.IsNullOrWhiteSpace(str);
			} else {
				return String.IsNullOrEmpty(str);
			}
		}

		public static bool IsNullOrEmpty(object obj) => IsNullOrEmpty(obj, false);
		public static bool IsNullOrBlank(object obj) => IsNullOrEmpty(obj, true);

		public static string CapitalizeFirstLetter(string str)
		{
			if (!String.IsNullOrEmpty(str) && Char.IsLower(str[0])) {
				return Char.ToUpper(str[0]) + str.Substring(1);
			} else {
				return str;
			}
		}

		public static string RepleceEol(string str, string newEol = null)
		{
			if (String.IsNullOrEmpty(str))
				return str;

			if (newEol == null)
				newEol = Environment.NewLine;

			return RegEx.Eol.Replace(str, newEol);
		}

		public static int CompareLines(string lines1, string lines2, StringComparison comparison = default(StringComparison))
		{
			string str1 = RepleceEol(lines1);
			string str2 = RepleceEol(lines2);
			return String.Compare(str1, str2, comparison);
		}

		public static string JoinNonEmpty(IEnumerable parts, string separator, Func<object, string> toString, string prefix = null, string suffix = null)
		{
			if (separator == null)
				separator = String.Empty;

			string result = null;
			foreach (var part in parts) {
				var str = toString(part);
				if (String.IsNullOrEmpty(str))
					continue;

				if (result == null) {
					result = prefix ?? String.Empty;
				} else {
					result += separator;
				}
				result += str;
			}

			if (suffix != null) {
				result += suffix;
			}
			return result;
		}

		public static string JoinNonEmptyParts(IEnumerable parts, string separator, string prefix = null, string suffix = null)
		{
			return JoinNonEmpty(parts, separator, (value) => value?.ToString(), prefix, suffix);
		}

		public static string JoinNonEmpty(string separator, params object[] parts) => JoinNonEmptyParts(parts, separator);

		//public static string JoinNonEmpty(string separator, params object[] values)
		//{
		//	var list = new List<string>();
		//	foreach (var value in values) {
		//		if (!IsNullOrEmptyString(value)) {
		//			list.Add(value.ToString());
		//		}
		//	}

		//	return String.Join(separator, list);
		//}

		public static string NamedValues(
			IEnumerable<string> names,
			IEnumerable values,
			string separator = null,
			string format = null,
			string prefix = null,
			string suffix = null,
			bool skipVoid = true,
			bool skipNull = false)
		{
			if (separator == null)
				separator = DefaultNamedValuesSeparator;
			if (String.IsNullOrEmpty(format))
				format = DefaultNamedValuesFormat;

			string text = null;
			Extras.ForEachPair(names, values, (name, value) => {
				if (value == null && skipNull)
					return;
				if (value == typeof(void) && skipVoid)
					return;

				if (text == null) {
					text = prefix ?? String.Empty;
				} else {
					text += separator;
				}
				text += String.Format(format, name, value);
			});

			if (!String.IsNullOrEmpty(suffix))
				text += suffix;

			return text;
		}

		public static string NamedValues(
			IEnumerable namesAndValues,
			string separator = null,
			string format = null,
			string prefix = null,
			string suffix = null,
			bool skipVoid = true,
			bool skipNull = false)
		{
			if (separator == null)
				separator = DefaultNamedValuesSeparator;
			if (String.IsNullOrEmpty(format))
				format = DefaultNamedValuesFormat;

			string text = null;
			var it = namesAndValues.GetEnumerator();
			while (it.MoveNext()) {
				object value = it.Current;
				string name = value as string;
				if (name != null) {
					if (it.MoveNext()) {
						value = it.Current;
					} else
						break;
				}

				if (value == null && skipNull)
					continue;
				if (value == typeof(void) && skipVoid)
					continue;

				if (text == null) {
					text = prefix ?? String.Empty;
				} else {
					text += separator;
				}
				text += String.Format(format, name, value);
			}

			if (!String.IsNullOrEmpty(suffix))
				text += suffix;

			return text;
		}

		public static string UnnamedValues(
			IEnumerable values,
			string separator = null,
			string format = null,
			string prefix = null,
			string suffix = null,
			bool skipVoid = true,
			bool skipNull = false)
		{
			if (separator == null)
				separator = DefaultUnnamedValuesSeparator;
			if (String.IsNullOrEmpty(format))
				format = DefaultUnnamedValuesFormat;

			string text = null;
			foreach (var value in values) {
				if (value == null && skipNull)
					continue;
				if (value == typeof(void) && skipVoid)
					continue;

				if (text == null) {
					text = prefix ?? String.Empty;
				} else {
					text += separator;
				}
				text += String.Format(format, value);
			}

			if (!String.IsNullOrEmpty(suffix))
				text += suffix;

			return text;
		}

		public static string ReadableProperties(
			object target,
			string separator = null,
			string format = null,
			string prefix = DefaultProperiesPrefix,
			string suffix = DefaultProperiesSuffix,
			bool skipNull = false)
		{
			var names = Reflection.GetPropertyNames(target, isReadable: true);
			return Properties(target, names, separator, format, prefix, suffix, skipNull);
		}

		public static string Properties(
			object target,
			IEnumerable<string> names = null,
			string separator = null,
			string format = null,
			string prefix = DefaultProperiesPrefix,
			string suffix = DefaultProperiesSuffix,
			bool skipNull = false)
		{
			if (names == null) {
				names = Reflection.GetPropertyNames(target, isReadable: true, isExplicit: false);
			}

			var values = new List<object>();
			foreach (var name in names) {
				var value = Reflection.GetPropertyValue(target, name, true);
				values.Add(value);
			}

			return NamedValues(names, values, separator, null, prefix, suffix, skipVoid: false, skipNull: skipNull);
		}

		public static string Properties(object target)
		{
			return Properties(target, (IEnumerable<string>)null);
		}

		public static string Properties(object target, params string[] names)
		{
			return Properties(target, (IEnumerable<string>)names);
		}

		public static string NotEmptyProperties(object target, params string[] names)
		{
			return Properties(names, (IEnumerable<string>)names, skipNull: true);
		}

		public static string[] CharsAsStrings(this string str)
		{
			if (str == null)
				return null;

			var length = str.Length;
			var list = new string[length];
			for (int i = 0; i < length; i++) {
				list[i] = str[i].ToString();
			}
			return list;
		}

		public static string FirstPart(string str, char[] separators)
		{
			if (str != null) {
				var split = str.Split(separators);
				if (split != null && split.Length > 0) {
					return split[0];
				}
			}
			return null;
		}

		public static string FirstLine(string str) => FirstPart(str, lineEndings);
		public static string FirstWord(string str) => FirstPart(str, wordEndings);

		static char[] lineEndings = new char[] { '\n', '\r' }; // TODO
		static char[] wordEndings = new char[] { ' ', '\t', '\n', '\r' }; // TODO
	}
}
 