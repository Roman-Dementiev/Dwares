//using System;
//using System.Collections;
//using System.Collections.Generic;


//namespace Dwares.Rookie.Dwarf
//{
//	public static class Extras
//	{
//		public static void ForEachPair<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values, Action<TKey, TValue> action)
//		{
//			if (keys == null)
//				throw new ArgumentNullException(nameof(keys));
//			if (values == null)
//				throw new ArgumentNullException(nameof(values));
//			if (action == null)
//				throw new ArgumentNullException(nameof(action));

//			var val = values.GetEnumerator();
//			foreach (var key in keys) {
//				if (!val.MoveNext())
//					break;
//				action(key, val.Current);
//			}
//		}

//		public static void ForEachPair<TKey>(IEnumerable<TKey> keys, IEnumerable values, Action<TKey, object> action)
//		{
//			if (keys == null)
//				throw new ArgumentNullException(nameof(keys));
//			if (values == null)
//				throw new ArgumentNullException(nameof(values));
//			if (action == null)
//				throw new ArgumentNullException(nameof(action));

//			var val = values.GetEnumerator();
//			foreach (var key in keys) {
//				if (!val.MoveNext())
//					break;
//				action(key, val.Current);
//			}
//		}
//	}
//}
