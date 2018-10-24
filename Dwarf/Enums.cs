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
