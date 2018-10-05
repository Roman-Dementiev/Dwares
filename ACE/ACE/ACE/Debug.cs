using System;


namespace ACE
{
	public static class Debug
	{
		public static void Print(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static void Print(string format, object arg0)
		{
			var message = String.Format(format, arg0);
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static void Print(string format, params object[] args)
		{
			var message = String.Format(format, args);
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static void ExceptionCaught(Exception ex)
		{
			Print("Exception caught: {0}", ex.Message);
		}
	}
}
