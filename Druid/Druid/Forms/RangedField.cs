using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class RangedField<T> : NullableField<T> where T : struct, IComparable
	{
		public RangedField(string name) : base(name) { }

		public T? MinValue { get; set; }
		public T? MaxValue { get; set; }
		public T? LowBound { get; set; }
		public T? HighBound { get; set; }

		public override Exception Validate()
		{
			var error = base.Validate();
			if (error == null) {
				if (!CheckRange()) {
					error = new FieldValueOutOfRangeError(MsgValueOutOfRange);
				}
			}
			return error;
		}

		protected virtual bool CheckRange()
		{
			if (Value != null) {
				var value = (T)Value;

				if (MinValue != null && value.CompareTo(MinValue) < 0)
					return false;

				if (MaxValue != null && value.CompareTo(MaxValue) > 0)
					return false;

				if (LowBound != null && value.CompareTo(LowBound) <= 0)
					return false;

				if (HighBound != null && value.CompareTo(HighBound) >= 0)
					return false;
			}

			return true;
		}

		string msgValueOutOfRange;
		public string MsgValueOutOfRange {
			get => GetMessage(msgValueOutOfRange, ValidationMessages.cValueOutOfRange);
			set => msgValueOutOfRange = value;
		}

	}
}
