using System;
using System.Text.RegularExpressions;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Validation
{
	public interface IValidationRule
	{
		//string ValidationMessage { get; set; }
		bool IsValid<T>(T value, out string message);
	}

	public abstract class ValidationRule : IValidationRule
	{
		public ValidationRule() { }
		public ValidationRule(string message)
		{
			ValidationMessage = message;
		}

		public string ValidationMessage { get; set; }

		public virtual bool IsValid<T>(T value, out string message)
		{
			var str = value?.ToString();

			string _message = null;
			if (Validate(str, ref _message)) {
				message = null;
				return true;
			} else {
				message = _message ?? ValidationMessage;
				return false;
			}
		}

		public abstract bool Validate(string value, ref string message);
	}

	public class IsNotNullOrEmptyRule : ValidationRule
	{
		public IsNotNullOrEmptyRule(bool whitespaceIsValid = false)
		{
			WhitespaceIsValid = whitespaceIsValid;
		}

		public IsNotNullOrEmptyRule(string message, bool whitespaceIsValid = false) :
			base(message)
		{
			WhitespaceIsValid = whitespaceIsValid;
		}

		public bool WhitespaceIsValid { get; set; }

		public override bool Validate(string value, ref string message)
		{
			if (WhitespaceIsValid) {
				return !String.IsNullOrEmpty(value);
			} else {
				return !String.IsNullOrWhiteSpace(value);
			}
		}
	}

	public class IntegerRule : ValidationRule
	{
		public IntegerRule(string message) : this(null, null, message, null) { }

		public IntegerRule(int? minValue, int? maxValue, string message, string outOfRange) :
			base(message)
		{
			MinValue = minValue;
			MaxValue = maxValue;
			OutOfRange = outOfRange;
		}

		public int? MinValue { get; set; }
		public int? MaxValue { get; set; }
		public string OutOfRange { get; set; }

		public override bool Validate(string value, ref string message)
		{
			int num;
			if (!int.TryParse(value, out num))
				return false;

			if ((MinValue != null && num < MinValue) || (MaxValue != null && num > MaxValue)) {
				message = OutOfRange;
				return false;
			}

			return true;
		}
	}


	public abstract class BaseRegExRule : ValidationRule
	{
		public BaseRegExRule() { }
		public BaseRegExRule(string message) : base(message) { }

		public override bool Validate(string value, ref string message)
		{
			if (value == null)
				return false;

			Match match = Regex.Match(value);
			return match.Success;
		}

		public virtual Regex Regex {
			get => throw new NotImplementedException(nameof(Regex));
			set => throw new NotImplementedException(nameof(Regex));
		}
	}

	public class RegExRule : BaseRegExRule
	{
		public RegExRule() { }
		public RegExRule(Regex regex, string message) :
			base(message)
		{
			Regex = regex;
		}

		public override Regex Regex { get; set; }
	}

	public class EmailRule : BaseRegExRule
	{
		public EmailRule() { }
		public EmailRule(string message) : base(message) { }

		public override Regex Regex => RegEx.Email;
	}

	//public class PhoneRule : BaseRegExRule
	//{
	//	public PhoneRule() { }
	//	public PhoneRule(string message) : base(message) { }
		
	//	public PhoneRule(PhoneFormat format, string message) : 
	//		base(message)
	//	{
	//		PhoneFormat = format;+
	//	}

	//	public PhoneFormat PhoneFormat { get; set; }

	//	public override Regex Regex => RegEx.PhoneRegex(PhoneFormat);
	//}
}
