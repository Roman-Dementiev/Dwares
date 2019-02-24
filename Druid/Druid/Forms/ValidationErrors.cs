using System;
using System.Threading;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Forms
{
	public static class ValidationMessages
	{
		static Vocabulary defaultMmessages;
		public static Vocabulary DefaultMessages {
			get => LazyInitializer.EnsureInitialized(ref defaultMmessages, () => {
				var messages = new Vocabulary();
				messages.Put(cValidationError, "Validation error");
				messages.Put(cFieldIsRequired, "Field is required");
				messages.Put(cInvalidEntryText, "Entered value is invalid");
				messages.Put(cValueOutOfRange, "Value is out of range");
				messages.Put(cMustBePositive, "Value must be a positive number");
				messages.Put(cMustBeNonNegative, "Value must be a non negative number");
				//messages.Put(cInvalidPhone, "Phone number is invalid");
				return messages;
			});
		}

		static Vocabulary nessageFormats;
		public static Vocabulary MessageFormats {
			get => LazyInitializer.EnsureInitialized(ref nessageFormats, () => {
				var messages = new Vocabulary();
				messages.Put(cValidationError, "Validation error");
				messages.Put(cFieldIsRequired, "{0} is required");
				messages.Put(cInvalidEntryText, "Entered {0} is invalid");
				messages.Put(cValueOutOfRange, "{0} is out of range");
				messages.Put(cMustBePositive, "{0} must be a positive number");
				messages.Put(cMustBeNonNegative, "{0} must be a non negative number");
				//messages.Put(cInvalidPhone, "{0} is invalid");
				return messages;
			});
		}

		public const string cValidationError= nameof(ValidationError);
		public static string ValidationError => DefaultMessages.Get(cValidationError);

		public const string cFieldIsRequired = nameof(FieldIsRequired);
		public static string FieldIsRequired => DefaultMessages.Get(cFieldIsRequired);

		public const string cInvalidEntryText = nameof(InvalidEntryText);
		public static string InvalidEntryText => DefaultMessages.Get(cInvalidEntryText);

		public const string cValueOutOfRange = nameof(ValueOutOfRange);
		public static string ValueOutOfRange => DefaultMessages.Get(cValueOutOfRange);

		public const string cMustBePositive = nameof(MustBePositive);
		public static string MustBePositive => DefaultMessages.Get(cMustBePositive);

		public const string cMustBeNonNegative = nameof(MustBeNonNegative);
		public static string MustBeNonNegative => DefaultMessages.Get(cMustBeNonNegative);

		//public const string cInvalidPhone = nameof(InvalidPhone);
		//public static string InvalidPhone => DefaultMessages.Get(cInvalidPhone);

		public static string GetMessage(string messageCode, string fieldName = null)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				if (!DefaultMessages.ContainsKey(messageCode))
					messageCode = cValidationError;

				return DefaultMessages.Get(messageCode);
			}
			else {
				if (!MessageFormats.ContainsKey(messageCode))
					messageCode = cValidationError;
			
				var format = MessageFormats.Get(messageCode);
				return string.Format(format, fieldName);
			}
		}
	}

	public class ValidationError : Exception
	{
		public ValidationError() : base(ValidationMessages.ValidationError) { }
		public ValidationError(string message) : base(message) { }
		public ValidationError(string message, Exception innerException) : base(message, innerException) { }
	}

	public class FieldIsRequiredError : ValidationError
	{
		public FieldIsRequiredError() : base(ValidationMessages.FieldIsRequired) { }
		public FieldIsRequiredError(string message) : base(message) { }
		public FieldIsRequiredError(string message, Exception innerException) : base(message, innerException) { }
	}

	public class FieldValueOutOfRangeError : ValidationError
	{
		public FieldValueOutOfRangeError() : base(ValidationMessages.ValueOutOfRange) { }
		public FieldValueOutOfRangeError(string message) : base(message) { }
		public FieldValueOutOfRangeError(string message, Exception innerException) : base(message, innerException) { }
	}

}
