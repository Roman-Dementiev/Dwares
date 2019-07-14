using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Satchel
{
	public static class ResourcesLoader
	{
		public static void Load<Target>(IDictionary<string, object> resources, Target target, Metadata metadata, Func<Target, string, object, bool> loadValue = null)
		{
			foreach (var pair in resources)
			{
				if (loadValue?.Invoke(target, pair.Key, pair.Value) == true)
					continue;

				if (target != null && Reflection.TrySetPropertyValue<Target>(target, pair.Key, pair.Value))
					continue;

				if (metadata != null) 
					metadata.Set(pair.Key, pair.Value);
			}
		}

		public static void Load<Target>(IDictionary<string, object> resources, Target target, Func<Target, string, object, bool> loadValue = null)
			=> Load(resources, target, null, loadValue);

		public static void Load(IDictionary<string, object> resources, Metadata metadata, Func<string, object, bool> loadValue = null)
		{
			if (loadValue != null) {
				Load<object>(resources, null, metadata, (@null, key, value) => loadValue(key, value));
			} else {
				Load<object>(resources, null, metadata, null);
			}
		}


	}
}
