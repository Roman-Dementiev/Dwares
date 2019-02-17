using System;
using Dwares.Dwarf;


namespace Dwares.Dwarf
{
	public static class Guard
	{
		public static void ArgumentNotNull(object arg, string name, string message = null)
		{
			if (arg != null)
				return;

			if (string.IsNullOrEmpty(message)) {
				throw new ArgumentNullException(name);
			} else {
				throw new ArgumentNullException(name, message);
			}
		}

		public static void ArgumentNotEmpty(object arg, string name, string message = null)
		{
			if (!string.IsNullOrEmpty(arg?.ToString()))
				return;
			
			if (string.IsNullOrEmpty(message)) {
				throw  new ArgumentNullException(name);
			}  else {
				throw new ArgumentNullException(name, message);
			}
		}

		public static void ArgumentIsInRange(object arg, bool inRange, string name, string message = null)
		{
			if (inRange)
				return;

			if (string.IsNullOrEmpty(message)) {
				throw new ArgumentOutOfRangeException(name);
			}
			else {
				throw new ArgumentOutOfRangeException(name, message);
			}
		}

		public static void ArgumentIsInRange(int arg, int minValue, int maxValue, string name, string message = null)
			=> ArgumentIsInRange(arg, arg >= minValue && arg <= maxValue, name, message);
	}
}
