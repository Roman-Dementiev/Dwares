using System;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Validation
{
	public class CustomField<T> : Validatable<T>, IValueHolder<T>
	{
		public const string IsRequiredMessage = "Field is Required";

		public CustomField() { }

		public CustomField(bool required)
		{
			if (required) {
				SetRequired(true);
			}
		}

		public CustomField(string isRequiredMessage)
		{
			if (isRequiredMessage != null) {
				SetRequired(true, isRequiredMessage);
			}
		}

		ValidationRule<T> isRequiredRule;

		bool IsRequired {
			get => isRequiredRule != null;
			set => SetRequired(false);
		}

		void SetRequired(bool required, string message = null)
		{
			if (required) {
				if (isRequiredRule != null) {
					if (message != null) {
						if (message.Length == 0) message = IsRequiredMessage;
						isRequiredRule.ValidationMessage = message;
					}
				} else {
					if (String.IsNullOrEmpty(message))
						message = IsRequiredMessage;
					isRequiredRule = new IsNotNullOrEmptyRule<T>(message);
					AddRule(isRequiredRule);
				}
			}
			else if (isRequiredRule != null) {
				RemoveRule(isRequiredRule);
				isRequiredRule = null;
			}
		}

		protected override bool DoValidation(bool allRules)
		{
			if (Strings.IsNullOrEmpty(Value) && !IsRequired)
				return true;

			return base.DoValidation(allRules);
		}
	}
}
