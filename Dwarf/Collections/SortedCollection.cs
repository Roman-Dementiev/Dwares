using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Dwares.Dwarf.Collections
{
	public class SortedCollection<T> : SourcedCollection<T>
	{
		public SortedCollection(ObservableCollection<T> source) :
			base(source)
		{
		}

		IComparer<T> comparer;
		public IComparer<T> Comparer {
			get => comparer;
			set {
				if (value != comparer) {
					comparer = value;
					SortItems();
				}
			}
		}

		protected void SortItems()
		{
			if (Comparer != null) {
				T[] items = Collection.ToArray(this);
				Array.Sort(items, Comparer);

				ClearThis();
				AddItems(items, false);
			}
		}

		protected override void AddToThis(T item)
		{
			if (Comparer != null) {
				T[] items = Collection.ToArray(this);
				int index = Array.BinarySearch(items, item, Comparer);
				if (index < 0) {
					index = ~index;
				}

				base.InsertItem(index, item);
			}
		}
	}
}
