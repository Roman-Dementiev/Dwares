using System;
using System.Reflection;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Passket.Models
{
	//public enum EntryType
	//{
	//	// Base types
	//	String = 0x00000,
	//	Number = 0x10000,
	//	Integer = 0x2000,
	//	Boolean = 0x4000,
	//	Composite = 0x8000,

	//	Password = String+1,
	//	Email,
	//	Uri,
	//	Text,

	//	PINCode = Integer+1,

	//	Hint = Composite+1
	//};


	public interface IEntry
	{
		EntryKind Kind { get; }
		string Name { get; set; }
		object Value { get; set; }
		bool IsEmpty { get; }
	}


	public class Entry<T> : IEntry, IValueHolder<T>
	{
		//static ClassRef @class = new ClassRef(typeof(Entry));

		public Entry(EntryKind kind)
		{
			//Debug.EnableTracing(@class);
			Kind = kind;
		}

		public EntryKind Kind { get; }
		public string Name { get; set; }
		public T Value { get; set; }

		object IEntry.Value {
			get => Value;
			set => TrySetValue(value);
		}

		public virtual bool IsEmpty {
			get => Strings.IsNullOrEmpty(Value);
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
