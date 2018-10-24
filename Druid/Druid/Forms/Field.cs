using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public class Field<T> : Validatable<T>, IValueHolder<T>
	{
		public const string IsRequiredMessage = "Field is Required";

		public Field() { }

		public Field(bool required)
		{
			if (required) {
				SetRequired(true);
			}
		}

		public Field(string isRequiredMessage)
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

		public void SetRequired(bool required, string message = null)
		{
			if (required) {
				if (isRequiredRule != null) {
					if (message != null) {
						if (message.Length == 0)
							message = IsRequiredMessage;
						isRequiredRule.ValidationMessage = message;
					}
				} else {
					if (String.IsNullOrEmpty(message))
						message = IsRequiredMessage;
					isRequiredRule = new IsNotNullOrEmptyRule<T>(message);
					AddRule(isRequiredRule);
				}
			} else if (isRequiredRule != null) {
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

	public class ClassField<T> : Field<T> where T: class, new()
	{
		public ClassField()
		{
			Value = new T();
		}

		public ClassField(bool required) :
			base(required)
		{
			Value = new T();
		}

		public ClassField(string isRequiredMessage) :
			base(isRequiredMessage)
		{
			Value = new T();
		}
	}
}
