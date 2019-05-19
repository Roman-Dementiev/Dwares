using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public interface IField
	{
		bool IsValid { get; }
		Exception Error { get; }
		// TODO
	}

	public class Field<T> : IField, IValueHolder<T>, ITextHolder
	{
		public Field(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
		public bool IsRequired { get; set; }
		public Exception Error { get; protected set; }

		protected bool? isValid = null;
		public virtual bool IsValid {
			get {
				if (isValid == null) {
					Error = Validate();
					isValid = Error == null;
				}
				return isValid == true;
			}
		}

		T _value;
		public T Value {
			get => _value;
			set {
				if (!Equals(value, _value)) {
					_value = value;
					isValid = null;
				}
			}
		}

		protected string text;
		public virtual string Text {
			get => text ?? Value?.ToString();
			set {
				if (value != text) {
					text = value;
					isValid = null;

					if (text != null) {
						try { 
							ConvertFromText(value);
						} catch {
							Value = default(T);
						}
					}
				}
			}
		}

		public static implicit operator T(Field<T> field)
		{
			return field.Value;
		}

		//public static explicit operator Field<T>(T value)
		//{
		//	return new Field<T>() { Value = value };
		//}

		public virtual Exception Validate()
		{
			if (text != null) {
				try {
					ConvertFromText(text);
				}
				catch (Exception exc) {
					return new ValidationError(MsgInvalidEntryText, exc);
				}
			}

			if (IsRequired && IsUnset()) {
				return new FieldIsRequiredError(MsgFieldIsRequired);
			}

			return null;
		}

		protected virtual void ConvertFromText(string text)
		{
			var value = Convert.ChangeType(text, typeof(T));
			Value = (T)value;
		}

		protected virtual bool IsUnset()
		{
			return Strings.IsNullOrEmpty(Value);
		}

		protected string GetMessage(string message, string messageCode)
		{
			if (string.IsNullOrEmpty(message)) {
				return ValidationMessages.GetMessage(messageCode, Name);
			}
			return message;
		}

		string msgFieldIsRequired;
		public string MsgFieldIsRequired {
			get => GetMessage(msgFieldIsRequired, ValidationMessages.cFieldIsRequired);
			set {
				msgFieldIsRequired = value;
				IsRequired = !string.IsNullOrEmpty(value);
			}
		}

		string msgInvalidEntryText;
		public string MsgInvalidEntryText {
			get => GetMessage(msgInvalidEntryText, ValidationMessages.cInvalidEntryText);
			set => msgInvalidEntryText = value;
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
