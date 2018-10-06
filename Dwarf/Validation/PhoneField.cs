using System;
using System.Collections.Generic;
using System.Text;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Validation
{
	public class PhoneField<T> : CustomField<T>
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
