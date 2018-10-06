using System;
using System.Text.RegularExpressions;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Validation
{
	public interface IValidationRule<T>
	{
		string ValidationMessage { get; set; }
		bool IsValid(T value);
	}

	public abstract class ValidationRule<T> : IValidationRule<T>
	{
		public ValidationRule() { }
		public ValidationRule(string message)
		{
			ValidationMessage = message;
		}

		public string ValidationMessage { get; set; }
		public virtual bool IsValid(T value)
		{
			var str = value?.ToString();
			return Validate(str);
		}

		public abstract bool Validate(string value);
	}

	public class IsNotNullOrEmptyRule<T> : ValidationRule<T>
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

		public override bool Validate(string value)
		{
			if (WhitespaceIsValid) {
				return !String.IsNullOrEmpty(value);
			} else {
				return !String.IsNullOrWhiteSpace(value);
			}
		}
	}

	public abstract class BaseRegExRule<T> : ValidationRule<T>
	{
		public BaseRegExRule() { }
		public BaseRegExRule(string message) : base(message) { }

		public override bool Validate(string value)
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

	public class RegExRule<T> : BaseRegExRule<T>
	{
		public RegExRule() { }
		public RegExRule(Regex regex, string message) :
			base(message)
		{
			Regex = regex;
		}

		public override Regex Regex { get; set; }
	}

	public class EmailRule<T> : BaseRegExRule<T>
	{
		public EmailRule() { }
		public EmailRule(string message) : base(message) { }

		public override Regex Regex => RegEx.Email;
	}

	public class PhoneRule<T> : BaseRegExRule<T>
	{
		public PhoneRule() { }
		public PhoneRule(string message) : base(message) { }
		
		public PhoneRule(PhoneFormat format, string message) : 
			base(message)
		{
			PhoneFormat = format;
		}

		public PhoneFormat PhoneFormat { get; set; }

		public override Regex Regex => RegEx.PhoneRegex(PhoneFormat);
	}
}
