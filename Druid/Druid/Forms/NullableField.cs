using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Forms
{
	public class NullableField<T> : Field<T?> where T : struct
	{
		public NullableField(string name) : base(name) { }

		protected override bool IsUnset()
		{
			return Value == null;
		}

		protected override void ConvertFromText(string text)
		{
			if (string.IsNullOrEmpty(text))
 {				Value = null;
			} else {
				var value = Convert.ChangeType(text, typeof(T));
				Value = (T)value;
			}
		}

		public static implicit operator T(NullableField<T> field)
		{
			return field.Value ?? default(T);
		}

	}
}
