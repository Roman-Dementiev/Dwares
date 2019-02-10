using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class RangedField<T> : Field<T> where T : struct, IComparable
	{
		//static ClassRef @class = new ClassRef(typeof(NumberField));

		public RangedField(bool required = false) :
			base(required)
		{
			//Debug.EnableTracing(@class);
		}

		public RangedField(T? minValue, T? maxValue, bool required = false) :
			base(required)
		{
			//Debug.EnableTracing(@class);
			MinValue = minValue;
			MaxValue = maxValue;
		}

		public T? MinValue { get; set; }
		public T? MaxValue { get; set; }

		public override Exception Validate()
		{
			var error = base.Validate();
			if (error == null) {
				if (!CheckRange()) {
					error = new FieldValueOutOfRangeError();
				}
			}
			return error;
		}

		protected virtual bool CheckRange()
		{
			if (MinValue != null && Value.CompareTo(MinValue) < 0)
				return false;

			if (MaxValue != null && Value.CompareTo(MaxValue) > 0)
				return false;

			return true;
		}
	}
}
