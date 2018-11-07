using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Forms
{
	public class DateField : Field<DateTime>
	{
		public DateField() { }

		public DateField(bool required) : base(required) { }

		public DateField(string isRequiredMessage) : base(isRequiredMessage) { }
	}
}
