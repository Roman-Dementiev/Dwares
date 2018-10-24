using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace Dwares.Dwarf.Collections
{
	public class SourcedCollection<T> : ObservableCollection<T>
	{
		//public SourcedCollection() { }

		public SourcedCollection(ObservableCollection<T> source)
		{
			Source = source;
		}

		ObservableCollection<T> source = null;
		public ObservableCollection<T> Source {
			get => source;
			set => SetSource(value);
		}

		public virtual void SetSource(ObservableCollection<T> source)
		{
			if (this.source != null) {
				this.source.CollectionChanged -= OnSourceCollectionChanged;
			}

			ClearThis();

			this.source = source;

			if (source != null) {
				source.CollectionChanged += OnSourceCollectionChanged;

				AddItems(source, false);
			}
		}

		private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				AddItems(e.NewItems, false);
				break;

			case NotifyCollectionChangedAction.Remove:
				RemoveItems(e.OldItems, false);
				break;

			case NotifyCollectionChangedAction.Replace:
				RemoveItems(e.OldItems, false);
				AddItems(e.NewItems, false);
				break;

			case NotifyCollectionChangedAction.Reset:
				Clear(false);
				AddItems(Source, false);
				break;
			}
		}

		public new void Add(T item) => Add(item, true);

		public void Add(T item, bool addToSource)
		{
			if (addToSource && Source != null) {
				Source.Add(item);
				return;
			}

			AddToThis(item);
		}

		protected virtual void AddToThis(T item)
		{
			base.Add(item);
		}

		public void AddItems(IEnumerable items, bool addToSource)
		{
			foreach (var obj in items) {
				if (obj is T item) {
					Add(item, addToSource);
				}
			}
		}

		public new void Remove(T item) => Remove(item, true);

		public void Remove(T item, bool removeFromSource)
		{
			if (removeFromSource && Source != null) {
				Source.Remove(item);
				return;
			}

			RemoveFromThis(item);
		}

		protected virtual void RemoveFromThis(T item)
		{
			base.Remove(item);
		}

		public void RemoveItems(IEnumerable items, bool removeFromSource)
		{
			foreach (var obj in items) {
				if (obj is T item) {
					Remove(item, removeFromSource);
				}
			}
		}

		public new void Clear() => Clear(true);

		public void Clear(bool removeFromSource)
		{
			if (removeFromSource && Source != null) {
				Source.Clear();
				return;
			}

			ClearThis();
		}

		protected virtual void ClearThis()
		{
			base.Clear();
		}
	}
}
