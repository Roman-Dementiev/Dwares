using System;
using System.Reflection;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Passket.Models
{
	public interface IEntry
	{
		string Name { get; set; }
		object Value { get; set; }
	}


	public class Entry<T> : IEntry, IValueHolder<T>
	{
		//static ClassRef @class = new ClassRef(typeof(Entry));

		public Entry()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name { get; set; }

		public T Value { get; set; }
		object IEntry.Value {
			get => Value;
			set => TrySetValue(value);
		}

		public bool TrySetValue(object value, bool tryConvert = true)
		{
			if (value == null) {
				Value = default(T);
				return true;
			}

			if (value is T val) {
				Value = val;
				return true;
			}

			if (tryConvert) {
				try {
					var converted = Convert.ChangeType(value, typeof(T));
					if (converted != null) {
						Value = (T)converted;
					}
				}
				catch (Exception ex) {
					Debug.Print("Can not convert {0} to {1}", value, typeof(T).GetTypeInfo(), Name);
				}
			}

			return true;
		}
	}
}
