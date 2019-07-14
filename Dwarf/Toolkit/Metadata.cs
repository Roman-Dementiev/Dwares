using System;
using System.Collections;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf.Toolkit
{
	public class Metadata
	{
		//static ClassRef @class = new ClassRef(typeof(Metadata));

		Dictionary<string, object> dict = new Dictionary<string, object>();

		public Metadata()
		{
			//Debug.EnableTracing(@class);
		}

		public void Load(IDictionary source, object target = null)
		{
			var enumerator = source.GetEnumerator();
			while (enumerator.MoveNext()) {
				Set(enumerator.Key.ToString(), enumerator.Value);
			}
		}

		public virtual void Set(string key, object value)
		{
			Guard.ArgumentNotEmpty(key, nameof(key));
			if (value != null) {
				dict[key] = value;
			} else {
				dict.Remove(key);
				return;
			}
		}

		public object Get(string key, object defaultValue = null)
		{
			object value;
			if (dict.TryGetValue(key, out value)) {
				return value;
			}
			return defaultValue;
		}

		public string GetAsString(string key, string defaultValue = null)
		{
			object value;
			if (dict.TryGetValue(key, out value)) {
				return value.ToString();
			}
			return defaultValue;
		}

	}
}
