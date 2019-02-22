using System;

namespace Dwares.Dwarf
{
	public class DwarfException : Exception
	{
		//static ClassRef @class = new ClassRef(typeof(DwarfException));
		public static string UnknownError { get; set; } = "Unknown error";
		public static string MessageFormat { get; set; } = "{0}: {1}";

		public string Error { get; }
		public string Details { get; }

		public DwarfException() :
			this(null)
		{
			//Debug.EnableTracing(@class);
		}

		public DwarfException(string message) : base(message)
		{
			Error = message;
			Details = null;
		}

		public DwarfException(string error, string details) :
			base(FormatMessage(error, details))
		{
			Error = error;
			Details = details;
		}

		public DwarfException(string error, Exception innerException) :
			base(FormatMessage(error, innerException?.Message), innerException)
		{
			Error = error;
			Details = innerException?.Message;
		}

		public static string FormatMessage(string error, string details)
		{
			if (string.IsNullOrEmpty(error))
				error = UnknownError;

			if (string.IsNullOrEmpty(details)) {
				return error;
			} else {
				return string.Format(MessageFormat, error, details);
			}
		}
	}

	public class UserError : Exception
	{
		public UserError(string message) : base(message) { }
		public UserError(string format, params string[] args) : base(string.Format(format, args)) { }
	}

	public class ProgramError : Exception
	{
		public ProgramError(string message) : base(message) { }
		public ProgramError(string format, params string[] args) : base(string.Format(format, args)) { }
	}

}

