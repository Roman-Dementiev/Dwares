using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace Dwares.Dwarf.Collections
{
	public class ShadowCollection<ShadowItem, SourceItem> : ObservableCollection<ShadowItem>
		where ShadowItem: class
	{
		protected ShadowCollection() { }

		public ShadowCollection(ObservableCollection<SourceItem> source, Func<SourceItem, ShadowItem> itemFactory)
		{
			SetSource(
				source ?? throw new ArgumentNullException(nameof(source)),
				itemFactory /*?? throw new ArgumentNullException(nameof(itemFactory))*/,
				false
				);
		}

		ObservableCollection<SourceItem> source = null;
		public ObservableCollection<SourceItem> Source {
			get => source;
			set => SetSource(value, ItemFactory);
		}

		public Func<SourceItem, ShadowItem> ItemFactory { get; protected set; }

		protected virtual void SetSource(ObservableCollection<SourceItem> newSource, Func<SourceItem, ShadowItem> itemFactory, bool fire = true)
		{
			if (newSource == source && itemFactory == ItemFactory)
				return;
			
			if (source != null) {
				source.CollectionChanged -= SourceCollectionChanged;
				Clear();
			}

			source = newSource;
			ItemFactory = itemFactory ?? ((sourceItem) => sourceItem as ShadowItem);

			if (newSource != null) {
				AddShadows(newSource);
				newSource.CollectionChanged += SourceCollectionChanged;
			}

			if (fire) {
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(Source)));
			}
		}

		protected void AddShadows(IList<SourceItem> items)
		{
			foreach (var sourceItem in items) {
				var shadowItem = ItemFactory(sourceItem);
				Add(shadowItem);
			}
		}

		protected void InsertShadows(int startinIndex, IList items)
		{
			int index = startinIndex;
			foreach (var item in items) {
				var shadowItem = ShadowFromObject(item);
				Add(shadowItem);
			}
		}

		protected void RemoveShadows(int startingIndex, int count)
		{
			for (int i = startingIndex + count - 1; i >= startingIndex; i--) {
				RemoveItem(i);
			}
		}

		protected void MoveShadows(int oldStartingIndex, int newStartingIndex, int count)
		{
			if (newStartingIndex == oldStartingIndex)
				return;

			if (newStartingIndex < oldStartingIndex) {
				int oldIndex = oldStartingIndex;
				int newIndex = newStartingIndex;
				for (int i = 0; i < count; i++) {
					MoveItem(oldIndex++, newIndex++);
				}
			} else {
				int oldIndex = oldStartingIndex + count - 1;
				int newIndex = newStartingIndex + count - 1;
				for (int i = 0; i < count; i++) {
					MoveItem(oldIndex--, newIndex--);
				}

			}
		}

		private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				InsertShadows(e.NewStartingIndex, e.NewItems);
				break;

			case NotifyCollectionChangedAction.Remove:
				RemoveShadows(e.OldStartingIndex, e.OldItems.Count);
				break;

			case NotifyCollectionChangedAction.Replace:
				RemoveShadows(e.OldStartingIndex, e.OldItems.Count);
				InsertShadows(e.NewStartingIndex, e.NewItems);
				break;

			case NotifyCollectionChangedAction.Move:
				MoveShadows(e.OldStartingIndex, e.NewStartingIndex, e.OldItems.Count);
				break;
			}
		}

		protected ShadowItem ShadowFromObject(object item)
		{
			if (item is SourceItem sourceItem) {
				return ItemFactory(sourceItem);
			} else {
				return null;
			}
		}
	}
}
