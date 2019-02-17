using System;
using System.Collections.Generic;
using Dwares.Dwarf;


namespace Dwares.Dwarf.Collections
{
	public interface IProps
	{
		bool GetString(string key, out string value);
		void SetString(string key, string value);
		bool GetBoolean(string key, out bool value);
		void SetBoolean(string key, bool value);
		bool GetInteger(string key, out int value);
		void Setinteger(string key, int value);
	}

	public interface IPropContainer<TStored>
	{
		bool ContainsKey(string key);
		bool GetStored(string key, out TStored stored);
		void SetStored(string key, TStored stored);
	}

	public class PropsBase<TStored>
	{

		public PropsBase(IPropContainer<TStored> container)
		{
			if (container != null) {
				Container = container;
			} else {
				Container = new PropContainer<TStored>();
			}
		}

		public IPropContainer<TStored> Container { get; }

		public virtual bool Get<T>(string key, out T value)
		{
			TStored stored;
			if (Container.GetStored(key, out stored)) {
				try {
					value = (T)Convert.ChangeType(stored, typeof(T));
					return true;
				}
				catch (Exception exc) {
					Debug.ExceptionCaught(exc);
				}
			}
			value = default(T);
			return false;
		}

		public virtual void Set<T>(string key, T value)
		{
			var stored = (TStored)Convert.ChangeType(value, typeof(TStored));
			Container.SetStored(key, stored);
		}

		public bool GetString(string key, out string value) => Get(key, out value);
		public void SetString(string key, string value) => Set(key, value);
		public bool GetBoolean(string key, out bool value) => Get(key, out value);
		public void SetBoolean(string key, bool value) => Set(key, value);
		public bool GetInteger(string key, out int value) => Get(key, out value);
		public void Setinteger(string key, int value) => Set(key, value);
	}

	public class Props : PropsBase<object>, IProps
	{
		//static ClassRef @class = new ClassRef(typeof(Props));

		public Props() : this(null) { }

		public Props(IPropContainer<object> container) :
			base(container)
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class StringProps : PropsBase<string>, IProps
	{
		//static ClassRef @class = new ClassRef(typeof(StringProps));

		public StringProps() : this(null) { }

		public StringProps(IPropContainer<string> container) :
			base(container)
		{
			//Debug.EnableTracing(@class);
		}

		public override void Set<T>(string key, T value)
		{
			Container.SetStored(key, value?.ToString());
		}

		public new bool GetString(string key, out string value)
		{
			return Container.GetStored(key, out value);
		}

		public new void SetString(string key, string value)
		{
			Container.SetStored(key, value);
		}
	}

	public class PropContainer<TStored> : Dictionary<string, TStored>, IPropContainer<TStored>
	{
		public bool GetStored(string key, out TStored value)
		{
			if (ContainsKey(key)) {
				value = this[key];
				return true;
			} else {
				value = default(TStored);
				return false;
			}
		}

		public void SetStored(string key, TStored value)
		{
			this[key] = value;
		}
	}
}
