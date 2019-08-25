using System;
using Xamarin.Forms;


namespace Dwares.Druid
{
	public static partial class Extensions
	{
		public static string GetString(this ResourceDictionary resources, string key, bool convert = false, string defaultValue = null)
		{
			if (resources.TryGetValue(key, out object value) && value != null) {
				if (value is string str)
					return str;
				if (convert)
					return value.ToString();
			}
			return defaultValue;
		}

		public static bool GetBoolean(this ResourceDictionary resources, string key, bool convert = false, bool defaultValue = default)
		{
			if (resources.TryGetValue(key, out object value) && value != null) {
				if (value is Boolean b)
					return b;
				if (convert && value is IConvertible convertible)
					return Convert.ToBoolean(convertible);
			}
			return defaultValue;
		}

	}
}
