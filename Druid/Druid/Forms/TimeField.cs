using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class TimeField : RangedField<TimeSpan>
	{
		public TimeField(string name) : base(name) { }

		public DateTime DateTime {
			get{
				if (Value != null) {
					return DateTime.Today.Add((TimeSpan)Value);
				} else {
					return default(DateTime);
				}
			} 
		}
	}
}
