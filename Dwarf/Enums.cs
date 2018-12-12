using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf
{
	public static class Enums
	{
		public static string GetName<T>(T value)
		{
			return Enum.GetName(typeof(T), value);
		}

		public static T Parse<T>(string name) where T : struct
		{
			var parsed = Enum.Parse(typeof(T), name);
			return (T)parsed;
		}

		public static bool TryParse<T>(string name, out T value) where T : struct
		{
			var parsed = Enum.Parse(typeof(T), name);
			if (parsed is T val) {
				value = val;
				return true;
			} 

			value = default(T);
			return false;
		}

		public static Dictionary<string, T> GetValueByNameDictionary<T>()
		{
			var type = typeof(T);
			var dict = new Dictionary<string, T>();
			var values = Enum.GetValues(type);
			foreach (var value in values) {
				var name = Enum.GetName(type, value);
				dict.Add(name, (T)value);
			};
			return dict;
		}
	}
}
