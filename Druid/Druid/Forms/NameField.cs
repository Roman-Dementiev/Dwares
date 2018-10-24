using System;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Forms
{
	public class NameField<TName> : ClassField<TName> where TName : GeneralName, new()
	{
		public NameField()
		{
		}

		public NameField(bool required) : base(required) { }

		public NameField(string isRequiredMessage) : base(isRequiredMessage) { }

		public static implicit operator string(NameField<TName> field)
		{
			return field.Value;
		}
	}

	public class GeneralNameField : NameField<GeneralName>
	{
		public GeneralNameField() { }

		public GeneralNameField(bool required) : base(required) { }

		public GeneralNameField(string isRequiredMessage) : base(isRequiredMessage) { }
	}

	public class PersonNameField: NameField<PersonName>
	{
		public PersonNameField() { }

		public PersonNameField(bool required) : base(required) { }

		public PersonNameField(string isRequiredMessage) : base(isRequiredMessage) { }
	}
}
