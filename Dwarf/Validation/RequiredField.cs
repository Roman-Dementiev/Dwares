using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Validation
{
	public class RequiredField<T>: CustomField<T>
	{
		public RequiredField() : base(true) { }

		public RequiredField(string isRequiredMessage) : 
			base(isRequiredMessage ?? IsRequiredMessage)
		{
		}
	}

	//public class RequiredField : RequiredField<string>
	//{
	//	public RequiredField() : base(true) { }
	//	public RequiredField(string isRequiredMessage) base(isRequiredMessage) {}
	//}
}
