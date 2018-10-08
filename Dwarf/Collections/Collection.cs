using System;
using System.Collections.Generic;


namespace Dwares.Dwarf.Collections
{
	public static class Collection
	{
		public static bool IsNullOrEmpty<T>(ICollection<T> collection)
		{
			if (collection == null)
				return true;

			return collection.Count == 0;
		}

		public static bool IsNullOrEmpty<T>(IReadOnlyCollection<T> collection)
		{
			if (collection == null)
				return true;

			return collection.Count == 0;
		}

		public static T LastElement<T>(IList<T> collection)
		{
			if (!IsNullOrEmpty(collection)) {
				return collection[collection.Count - 1];
			} else {
				return default(T);
			}
		}

		public static T LastElement<T>(IReadOnlyList<T> collection)
		{
			if (!IsNullOrEmpty(collection)) {
				return collection[collection.Count - 1];
			} else {
				return default(T);
			}
		}
	}
}
