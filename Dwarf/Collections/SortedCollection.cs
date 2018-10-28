using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Dwares.Dwarf.Collections
{
	public class SortedCollection<T> : SourcedCollection<T>
	{
		public SortedCollection(ObservableCollection<T> source, Func<T, bool> criterion = null) :
			this(source, (IComparer<T>)null, criterion)
		{
		}

		public SortedCollection(ObservableCollection<T> source, Comparison<T> comparison, Func<T, bool> criterion = null) :
			this(source, Comparer<T>.Create(comparison), criterion)
		{
		}

		public SortedCollection(ObservableCollection<T> source, IComparer<T> comparer, Func<T, bool> criterion = null) :
			base(source, criterion, false)
		{
			this.comparer = comparer;
			Recollect(false);
		}

		IComparer<T> comparer;
		public IComparer<T> Comparer {
			get => comparer;
			set {
				if (value != comparer) {
					comparer = value;
					Recollect(true);
				}
			}
		}

		public bool Descending {
			get {
				if (Comparer is SortOrder<T> sortOrder) {
					return sortOrder.Descending;
				} else {
					return false;
				}
			}
			set {
				if (Comparer is SortOrder<T> sortOrder) {
					if (value != sortOrder.Descending) {
						sortOrder.Descending = value;
						Recollect(true);
					}
				} else if (value && Comparer != null) {
					Comparer = new SortOrder<T>(Comparer, value);
				}
			}
		}

		protected override void Recollect(bool clear)
		{
			if (Comparer == null || Source == null) {
				base.Recollect(clear);
				return;
			}

			if (clear) {
				ClearThis();
			}

			var items = new List<T>();
			foreach (var obj in Source) {
				if (obj is T item && Match(item)) {
					items.Add(item);
				}
			}

			items.Sort(Comparer);

			foreach (var item in items) {
				base.AddToThis(item);
			}
		}

		protected override void AddToThis(T item)
		{
			if (Comparer != null) {
				if (!Match(item))
					return;

				T[] items = Collection.ToArray(this);
				int index = Array.BinarySearch(items, item, Comparer);
				if (index < 0) {
					index = ~index;
				}

				base.InsertItem(index, item);
			} else {
				base.AddToThis(item);
			}
		}
	}
}
