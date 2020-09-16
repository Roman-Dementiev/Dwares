using Dwares.Dwarf.Toolkit;
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
		public ShadowCollection() { }

		public ShadowCollection(ObservableCollection<SourceItem> source, Func<SourceItem, ShadowItem> itemFactory)
		{
			SetSource(
				source ?? throw new ArgumentNullException(nameof(source)),
				itemFactory /*?? throw new ArgumentNullException(nameof(itemFactory))*/,
				false
				);
		}

		public Func<SourceItem, ShadowItem> ItemFactory { get; protected set; }

		public ObservableCollection<SourceItem> Source {
			get => source;
			set => SetSource(value, ItemFactory);
		}
		ObservableCollection<SourceItem> source = null;

		public virtual void SetSource(
			ObservableCollection<SourceItem> newSource, 
			Func<SourceItem, ShadowItem> itemFactory,
			bool fire = true)
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
				AddShadows();
				newSource.CollectionChanged += SourceCollectionChanged;
			}

			if (fire) {
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(Source)));
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


		public bool IsSuspended {
			get => isSuspended;
			set {
				if (value != isSuspended) {
					if (isSuspended && needRecollect) {
						RecollectShadows();
					}
					needRecollect = false;
					isSuspended = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSuspended)));
				}
			}
		}
		bool isSuspended = false;
		bool needRecollect = false;

		protected void AddShadows()
		{
			if (Source == null)
				return;

			foreach (var item in Source) {
				var shadowItem = ShadowFromObject(item);
				if (shadowItem != null) {
					Add(shadowItem);
				}
			}

			if (HasPlaceholder) {
				Add(Placeholder);
			}
		}

		protected void RecollectShadows(bool fullRecollect = true)
		{
			if (Source == null) {
				Clear();
				return;
			}

			if (fullRecollect) {
				Clear();
				AddShadows();
			}
			else
			{
				int i, first;
				for (first = 0; first < Source.Count; first++) {
					if (first < this.Count) {
						var shadow = this[first];
						if (shadow is ISourced<SourceItem> sourced) {
							if (Equals(sourced.Source, Source[first]))
								continue;
						}
					}
					break;
				}


				for (i = this.Count - 1; i >= first; i--) {
					RemoveAt(i);
				}

				for (i = first; i < Source.Count; i++) {
					var item = Source[i];
					var shadowItem = ShadowFromObject(item);
					if (shadowItem != null) {
						Add(shadowItem);
					}
				}

				if (HasPlaceholder) {
					Add(Placeholder);
				}
			}
		}

		private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (IsSuspended) {
				needRecollect = true;
			} else {
				RecollectShadows();
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
