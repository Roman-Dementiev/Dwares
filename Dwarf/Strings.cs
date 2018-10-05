using System;
using System.Collections;
using System.Collections.Generic;
using Dwares.Dwarf.Runtime;


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

		public static bool IsNullOrEmptyString(object obj)
		{
			if (obj == null)
				return true;

			if (obj is string str)
				return str.Length == 0;

			return false;
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
			//if (separator == null)
			//	separator = String.Empty;

			//string result = null;
			//foreach (var part in parts)
			//{
			//	var str = part?.ToString();
			//	if (String.IsNullOrEmpty(str))
			//		continue;

			//	if (result == null ) {
			//		result = prefix ?? String.Empty;
			//	} else {
			//		result += separator;
			//		result += str;
			//	}
			//}

			//if (suffix != null) {
			//	result += suffix;
			//}
			//return result;

			return JoinNonEmpty(parts, separator, (value) => value?.ToString(), prefix, suffix);
		}

		public static string JoinNonEmpty(string separator, params object[] parts) => JoinNonEmptyParts(parts, separator);

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
			string suffix = DefaultProperiesSuffix)
		{
			var names = Reflection.GetPropertyNames(target, isReadable: true);
			return Properties(target, names, separator, format, prefix, suffix);
		}

		public static string Properties(
			object target,
			IEnumerable<string> names = null,
			string separator = null,
			string format = null,
			string prefix = DefaultProperiesPrefix,
			string suffix = DefaultProperiesSuffix)
		{
			if (names == null) {
				names = Reflection.GetPropertyNames(target, isReadable: true, isExplicit: false);
			}

			var values = new List<object>();
			foreach (var name in names) {
				var value = Reflection.GetPropertyValue(target, name, true);
				values.Add(value);
			}

			return NamedValues(names, values, separator, null, prefix, suffix);
		}

		public static string Properties(object target, params string[] names)
		{
			return Properties(target, (IEnumerable<string>)names);
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
	}
}
