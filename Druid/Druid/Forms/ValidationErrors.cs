using System;
using System.Threading;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public static class ValidationMessages
	{
		static Vocabulary messages;
		public static Vocabulary Messages {
			get => LazyInitializer.EnsureInitialized(ref messages, () => {
				var messages = new Vocabulary();
				messages.Put(cFieldIsRequired, "Field is required");
				messages.Put(cInvalidEntryText, "Entered value is invalid");
				messages.Put(cFieldIsRequired, "Value out of range");
				messages.Put(cInvalidPhone, "Phone number is invalid");
				return messages;
			});
		}

		public const string cFieldIsRequired = nameof(FieldIsRequired);
		public static string FieldIsRequired => Messages.Get(cFieldIsRequired);

		public const string cInvalidEntryText = nameof(InvalidEntryText);
		public static string InvalidEntryText => Messages.Get(cInvalidEntryText);

		public const string cValueOutOfRange = nameof(ValueOutOfRange);
		public static string ValueOutOfRange => Messages.Get(cValueOutOfRange);

		public const string cInvalidPhone = nameof(InvalidPhone);
		public static string InvalidPhone => Messages.Get(cInvalidPhone);
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
