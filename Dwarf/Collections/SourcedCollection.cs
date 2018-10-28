using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace Dwares.Dwarf.Collections
{
	public class SourcedCollection<T> : ObservableCollection<T>
	{
		//public SourcedCollection() { }

		protected SourcedCollection(ObservableCollection<T> source, Func<T, bool> criterion, bool collect)
		{
			SetSource(source, criterion, collect);
		}

		public SourcedCollection(ObservableCollection<T> source, Func<T, bool> criterion = null) :
			this(source, criterion, true)
		{
		}

		ObservableCollection<T> source = null;
		public ObservableCollection<T> Source {
			get => source;
			set => SetSource(value, Criterion, true);
		}

		Func<T, bool> criterion = null;
		public Func<T, bool> Criterion {
			get => criterion;
			set => SetSource(Source, value, true);
		}

		protected virtual void SetSource(ObservableCollection<T> source, Func<T, bool> criterion, bool recollect)
		{
			if (source == this.source && criterion == this.criterion)
				return;

			if (this.source != null) {
				this.source.CollectionChanged -= OnSourceCollectionChanged;
			}

			ClearThis();

			this.source = source;
			this.criterion = criterion;

			if (source != null) {
				source.CollectionChanged += OnSourceCollectionChanged;

				if (recollect) {
					Recollect(false);
				}
			}
		}

		protected virtual void Recollect(bool clear)
		{
			if (clear) {
				ClearThis();
			}
			if (Source != null) {
				AddToThis(Source);
			}
		}

		private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				AddToThis(e.NewItems);
				break;

			case NotifyCollectionChangedAction.Remove:
				RemoveFromThis(e.OldItems);
				break;

			case NotifyCollectionChangedAction.Replace:
				RemoveFromThis(e.OldItems);
				AddToThis(e.NewItems);
				break;

			case NotifyCollectionChangedAction.Reset:
				Recollect(true);
				break;
			}
		}

		public virtual bool Match(T item)
		{
			if (Criterion != null) {
				return Criterion(item);
			} else {
				return true;
			}
		}

		public new virtual void Add(T item)
		{
			if (Source != null) {
				Source.Add(item);
			} else {
				AddToThis(item);
			}
		}

		protected virtual void AddToThis(T item)
		{
			if (Match(item)) {
				base.Add(item);
			}
		}

		protected void AddToThis(IEnumerable items)
		{
			if (items == null)
				return;

			foreach (var obj in items) {
				if (obj is T item) {
					AddToThis(item);
				}
			}
		}

		public new void Remove(T item)
		{
			if (Source != null) {
				Source.Remove(item);
			} else {
				RemoveFromThis(item);
			}
		}

		protected virtual void RemoveFromThis(T item)
		{
			base.Remove(item);
		}

		protected void RemoveFromThis(IEnumerable items)
		{
			if (items == null)
				return;
			
			foreach (var obj in items) {
				if (obj is T item) {
					RemoveFromThis(item);
				}
			}
		}

		public new void Clear()
		{
			if (Source != null) {
				foreach (var item in Items) {
					Source.Remove(item);
				}
			} else {
				ClearThis();
			}
		}

		protected virtual void ClearThis()
		{
			base.Clear();
		}
	}
}
