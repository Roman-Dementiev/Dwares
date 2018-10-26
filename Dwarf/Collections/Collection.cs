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

		public static T First<T>(IList<T> collection)
		{
			if (!IsNullOrEmpty(collection)) {
				return collection[0];
			} else {
				return default(T);
			}
		}

		public static T First<T>(IReadOnlyList<T> collection)
		{
			if (!IsNullOrEmpty(collection)) {
				return collection[0];
			} else {
				return default(T);
			}
		}

		public static T Last<T>(IList<T> collection)
		{
			if (!IsNullOrEmpty(collection)) {
				return collection[collection.Count - 1];
			} else {
				return default(T);
			}
		}

		public static T Last<T>(IReadOnlyList<T> collection)
		{
			if (!IsNullOrEmpty(collection)) {
				return collection[collection.Count - 1];
			} else {
				return default(T);
			}
		}

		//public static T[] ToArray<T>(IList<T> collection)
		//{
		//	if (collection == null)
		//		return null;

		//	int count = collection.Count;
		//	T[] array = new T[count];
		//	for (int i = 0; i < count; i++) {
		//		array[i] = collection[i];
		//	}
		//	return array;
		//}

		public static T[] ToArray<T>(ICollection<T> collection)
		{
			if (collection == null)
				return null;

			T[] array = new T[collection.Count];
			int i = 0;
			foreach (var item in collection) {
				array[i++] = item;
			}
			return array;
		}

		//public static T[] ToArray<T>(IReadOnlyCollection<T> collection)
		//{
		//	if (collection == null)
		//		return null;

		//	T[] array = new T[collection.Count];
		//	int i = 0;
		//	foreach (var item in collection) {
		//		array[i++] = item;
		//	}
		//	return array;
		//}

		// TODO
		public static int BinarySearch<T>(ICollection<T> collection, T item, IComparer<T> comparer)
		{
			T[] items = ToArray(collection);
			return Array.BinarySearch(items, item, comparer);
		}
	}
}
