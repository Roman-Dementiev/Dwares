using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetWerks
{
	public static class Debug
	{
		public static void Print(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static void ExceptionCaught(Exception exc)
		{
			Print($"ExceptionCaught: {exc.Message}");
		}
	}
}
