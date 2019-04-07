using System;
using Dwares.Dwarf;


namespace Dwares.Dwarf
{
	public static class Guard
	{
		public static T ArgumentNotNull<T>(T arg, string name, string message = null)
		{
			if (arg == null) {
				if (string.IsNullOrEmpty(message)) {
					throw new ArgumentNullException(name);
				} else {
					throw new ArgumentNullException(name, message);
				}
			}
			return arg;
		}

		//public static void ArgumentNotEmpty(object arg, string name, string message = null)
		//{
		//	if (!string.IsNullOrEmpty(arg?.ToString()))
		//		return;
			
		//	if (string.IsNullOrEmpty(message)) {
		//		throw  new ArgumentNullException(name);
		//	}  else {
		//		throw new ArgumentNullException(name, message);
		//	}
		//}

		public static T ArgumentNotEmpty<T>(T arg, string name, string message = null)
		{			if (string.IsNullOrEmpty(arg?.ToString())) {

				if (string.IsNullOrEmpty(message)) {
					throw new ArgumentNullException(name);
				} else {
					throw new ArgumentNullException(name, message);
				}
			}
			return arg;
		}

		public static void ArgumentIsValid(string name, bool condition, string message)
		{
			if (!condition)
				throw new ArgumentException(message, name);
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


		public static Exception Verify(bool condition, string errorMessage, bool throwIfError = true)
		{
			Debug.Assert(condition, errorMessage);
			if (condition)
				return null;

			var exc = new ProgramError(errorMessage);
			if (throwIfError) {
				throw exc;
			} else {
				return exc;
			}
		}
	}
}
