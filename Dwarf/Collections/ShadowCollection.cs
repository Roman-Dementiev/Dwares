using Dwares.Dwarf.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Dwares.Dwarf.Collections
{
	public class ShadowCollection<ShadowItem, SourceItem> : ObservableCollectionEx<ShadowItem>
		//where SourceItem : class
		where ShadowItem : class
	{
		public ShadowCollection() { }

		public ShadowCollection(
			ObservableCollection<SourceItem> source, 
			Func<SourceItem, ShadowItem> itemFactory, 
			Predicate<SourceItem> itemFilter = null)
		{
			SetItemFactory(itemFactory);
			SetItemFilter(itemFilter);
			Source = source ?? throw new ArgumentNullException(nameof(source));
		}


		public ObservableCollection<SourceItem> Source {
			get => source;
			set {
				if (value != source) {
					if (source != null) {
						source.CollectionChanged -= SourceCollectionChanged;
						Clear();
					}
					if (value != null) {
						value.CollectionChanged += SourceCollectionChanged;
					}

					source = value;
					Recollect();
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Source)));
				}
			}
		}
		ObservableCollection<SourceItem> source = null;

		public Func<SourceItem, ShadowItem> ItemFactory {
			get => itemFactory;
			set {
				if (SetItemFactory(value)) {
					Recollect();
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(ItemFactory)));
				}
			}
		}
		Func<SourceItem, ShadowItem> itemFactory;

		protected bool SetItemFactory(Func<SourceItem, ShadowItem> factory)
		{
			if (factory != itemFactory) {
				itemFactory = factory ?? throw new ArgumentNullException(nameof(factory));
				return true;
			} else {
				return false;
			}
		}

		public Predicate<SourceItem> ItemFilter {
			get => itemFilter;
			set {
				if (SetItemFilter(value)) {
					Recollect();
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(ItemFilter)));
				}
			}
		}
		Predicate<SourceItem> itemFilter;

		protected bool SetItemFilter(Predicate<SourceItem> filter)
		{
			if (filter == null) {
				filter = Unfiltered;
			}
			if (filter != itemFilter) {
				itemFilter = filter;
				return true;
			} else {
				return false;
			}
		}

		public bool HasPlaceholder {
			get => Placeholder != null;
		}
		public ShadowItem Placeholder {
			get => placeholder;
			set {
				if (value == placeholder)
					return;
				//if (Equals(value, placeholder))
				//	return;

				if (placeholder != null) {
					Remove(placeholder);
				}
				if (value != null) {
					Add(value);
				}
				placeholder = value;
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(Placeholder)));
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasPlaceholder)));
			}
		}
		ShadowItem placeholder;


		protected virtual void Recollect()
		{
			using (var batch = new BatchCollectionChange(this))
			{
				Clear();
				if (Source == null)
					return;

				foreach (var item in Source) {
					if (ItemFilter(item)) {
						var shadowItem = ItemFactory(item);
						if (shadowItem != null) {
							Add(shadowItem);
						}
					}
				}

				if (HasPlaceholder) {
					Add(Placeholder);
				}
			}
		}

		private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//if (!AlwaysRecollect) {
			//	if (e.Action == NotifyCollectionChangedAction.Add) {
			//		if (Placeholder != null) {
			//			Remove(Placeholder);
			//		}

			//		AddShadows(e.NewItems);

			//		if (Placeholder != null) {
			//			Add(Placeholder);
			//		}
			//		return;
			//	}
			//	if (e.Action == NotifyCollectionChangedAction.Remove) {
			//		if (Placeholder != null) {
			//			Remove(Placeholder);
			//		}

			//		RemoveShadows(e.OldItems);

			//		if (Placeholder != null) {
			//			Add(Placeholder);
			//		}
			//		return;
			//	}
			//}

			Recollect();
		}

		//protected void AddShadows(IList sourceItems)
		//{
		//	foreach (var item in sourceItems) {
		//		if (item is SourceItem sourceItem) {
		//			if (ItemFilter(sourceItem)) {
		//				var shadowItem = ItemFactory(sourceItem);
		//				if (shadowItem != null) {
		//					Add(shadowItem);
		//				}
		//			}
		//		}
		//	}
		//}

		//protected void RemoveShadows(IList sourceItems)
		//{
		//	foreach (var item in sourceItems) {
		//		for (int i = 0; i <= Count; i++) {
		//			var shadow = this[i];
		//			if (shadow.Source == item) {
		//				RemoveAt(i);
		//				break;
		//			}
		//		}
		//	}
		//}


		//Turning AlwaysRecollect off makes too much troubles
		//protected bool AlwaysRecollect { get; set; } = true;

		static bool Unfiltered(SourceItem item) => true;
	}
}
