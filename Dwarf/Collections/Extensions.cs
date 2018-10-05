using System;
using System.Collections.Generic;


namespace Dwares.Dwarf.Collections
{
	public static class Extensions
	{
		public static void PutValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			if (dict.ContainsKey(key)) {
				dict[key] = value;
			} else {
				dict.Add(key, value);
			}
		}

		public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
		{
			if (dict.TryGetValue(key, out var value)) {
				return value;
			} else {
				return defaultValue;
			}
		}

		public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue: new()
		{
			TValue value;
			if (!dict.TryGetValue(key, out value)) {
				value = new TValue();
				dict.Add(key, value);
			}
			return value;
		}
	}
}
