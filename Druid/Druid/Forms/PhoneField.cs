using System;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public class PhoneField<T> : Field<T>
	{
		public PhoneField(bool required = false) : this(PhoneFormat.Default, required) { }

		public PhoneField(PhoneFormat format, bool required = false) :
			base(required)
		{
			rules.Add(new PhoneRule(format, ValidationMessages.InvalidPhone));
		}

	}

	public class PhoneField : PhoneField<string>
	{
		public PhoneField(bool required = false) : base(required) { }
		public PhoneField(PhoneFormat format, bool required = false) : base(format, required) { }
	}
}
