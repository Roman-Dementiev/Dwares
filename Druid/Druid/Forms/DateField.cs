using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Forms
{
	public class DateField : RangedField<DateTime>
	{
		public DateField(bool required = false) : base(required) { }

	}
}
