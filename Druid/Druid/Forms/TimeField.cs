using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Forms
{
	public class TimeField : Field<TimeSpan>
	{
		public TimeField() { }

		public TimeField(bool required) : base(required) { }

		public TimeField(string isRequiredMessage) : base(isRequiredMessage) { }

	}
}
