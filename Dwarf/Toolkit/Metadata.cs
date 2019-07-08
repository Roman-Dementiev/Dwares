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

		public virtual void Set(string key, object value, object target = null)
		{
			Guard.ArgumentNotEmpty(key, nameof(key));
			if (value == null) {
				dict.Remove(key);
				return;
			}


			dict[key] = value;

			var property = target?.GetPropertyInfo(key);
			if (property?.SetMethod != null) {
				if (!property.PropertyType.IsAssignableFrom(value.GetType())) {
					try {
						value = Convert.ChangeType(value, property.PropertyType);
					}
					catch (Exception exc) {
						Debug.Print($"Metadata.Set(): can not convert {value} to {property.PropertyType} => {exc}");
						return;
					}
				}

				try {
					Reflection.SetPropertyValue(target, property, value);
				}
				catch (Exception exc) {
					Debug.Print($"Metadata.Set(): can not set property {key} to {value} => {exc}");
				}
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
