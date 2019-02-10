using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Forms
{
	public class TimeField : RangedField<TimeSpan>
	{
		public TimeField(bool required = false) : base(required) { }

	}
}
