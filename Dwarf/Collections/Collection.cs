using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

		public static void AddOrReplace<T>(this IList<T> collection, T newItem, T oldItem)
		{
			int index = collection.IndexOf(oldItem);
			if (index >= 0) {
				collection.RemoveAt(index);
				collection.Insert(index, newItem);
			}
			else {
				collection.Add(newItem);
			}
		}

		// TODO
		public static int BinarySearch<T>(this ICollection<T> collection, T item, IComparer<T> comparer)
		{
			T[] items = ToArray(collection);
			return Array.BinarySearch(items, item, comparer);
		}

		// TODO
		public static ICollection<T> Sort<T>(ICollection<T> collection, ICollection<T> sorted, IComparer<T> comparer)
		{
			Guard.ArgumentNotNull(collection, nameof(collection));

			var items = new List<T>();
			foreach (var item in collection) {
				items.Add(item);
			}

			items.Sort(comparer);

			;
			foreach (var item in collection) {
				items.Add(item);
			}

			items.Sort(comparer);

			if (sorted == null) {
				sorted = Activator.CreateInstance(collection.GetType()) as ICollection<T>;
				Debug.AssertNotNull(sorted);
			} else {
				sorted.Clear();
			}

			foreach (var item in items) {
				sorted.Add(item);
			}

			return sorted;
		}

		public static void Sort<T>(this ICollection<T> collection, IComparer<T> comparer)
			=> Sort(collection, collection, comparer);

	}
}
