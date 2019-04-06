using System;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Forms
{
	public class DateField : RangedField<DateTime>
	{
		public DateField(string name) : base(name) { }
	}

	public class DateOnlyField : RangedField<DateOnly>
	{
		public DateOnlyField(string name) : base(name) { }
	}
}
