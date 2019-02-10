using System;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public class IntegerField : RangedField<int>
	{
		public IntegerField(bool required = false) : base(required) { }

		public IntegerField(int? minValue, int? maxValue, bool required = false) :
			base(minValue, maxValue, required)
		{
			//Debug.EnableTracing(@class);
		}

	}
}
