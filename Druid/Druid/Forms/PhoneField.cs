using System;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public class PhoneField<T> : Field<T>
	{
		const string InvalidPhoneMessage = "Phone number is invalid";

		public PhoneField(PhoneFormat format, string isInvalidPhoneMessage, string isRequiredMessage = null) :
			base(isRequiredMessage)
		{
			if (String.IsNullOrEmpty(isInvalidPhoneMessage))
				isInvalidPhoneMessage = InvalidPhoneMessage;

			rules.Add(new PhoneRule<T>(format, isInvalidPhoneMessage));
		}

		public PhoneField(string isInvalidPhoneMessage, string isRequiredMessage = null) :
			this(PhoneFormat.Default, isInvalidPhoneMessage, isRequiredMessage)
		{
		}
	}

	public class PhoneField : PhoneField<string>
	{
		public PhoneField(PhoneFormat format, string isInvalidPhoneMessage, string isRequiredMessage = null) :
			base(format, isInvalidPhoneMessage, isRequiredMessage)
		{
		}

		public PhoneField(string isInvalidPhoneMessage, string isRequiredMessage = null) :
			base(isInvalidPhoneMessage, isRequiredMessage)
		{
		}
	}
}
