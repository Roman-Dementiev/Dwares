using System;
using Dwares.Dwarf;


namespace Dwares.Dwarf
{
	public static class Dw
	{
		public static string ToString(object obj)
		{
			if (obj == null) {
				return "null";
			} else if (obj.GetType() == typeof(string)) {
				return $"\"{obj}\"";
			} else {
				return $"{obj}";
			}
		}
	}
}
