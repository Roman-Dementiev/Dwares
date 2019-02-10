using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public interface IField
	{
		bool isValid { get; }
	}

	public class Field<T> : Validatable<T>, IValueHolder<T>, ITextHolder
	{
		public Field(bool required = false)
		{
			IsRequired = required;
		}

		bool isRequired;
		bool IsRequired {
			get => isRequired;
			set => SetProperty(ref isRequired, value);
		}

		public override T Value {
			get => _value;
			set {
				if (SetProperty(ref _value, value)) {
					isValid = null;
					PropertiesChanged(nameof(IsValid), nameof(Text));
				}
			}
		}

		string text;
		public virtual string Text {
			get => text ?? _value?.ToString();
			set {
				if (SetProperty(ref text, value)) {
					isValid = null;
					PropertiesChanged(nameof(IsValid), nameof(Value));
				}
			}
		}

		public override Exception Validate()
		{
			if (text != null) {
				try {
					ConvertFromText();
				}
				catch (Exception exc) {
					return new ValidationError(ValidationMessages.InvalidType, exc);
				}
			}

			if (IsRequired && Strings.IsNullOrEmpty(Value)) {
				return new FieldIsRequiredError(MsgFieldIsRequired);
			}

			return base.Validate();
		}

		protected virtual void ConvertFromText()
		{
			var value = Convert.ChangeType(text, typeof(T));
			Value = (T)value;
		}

		string msgFieldIsRequired;
		public string MsgFieldIsRequired {
			get => msgFieldIsRequired ?? ValidationMessages.FieldIsRequired;
			set => msgFieldIsRequired = value;
		}

		string msgInvalidType;
		public string MsgInvalidType {
			get => msgInvalidType ?? ValidationMessages.InvalidType;
			set => msgInvalidType = value;
		}
	}

	//public class ClassField<T> : Field<T> where T: class, new()
	//{
	//	public ClassField()
	//	{
	//		Value = new T();
	//	}

	//	public ClassField(bool required) :
	//		base(required)
	//	{
	//		Value = new T();
	//	}

	//	public ClassField(string isRequiredMessage) :
	//		base(isRequiredMessage)
	//	{
	//		Value = new T();
	//	}
	//}
}
