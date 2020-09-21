using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Dwares.Dwarf;


namespace Dwares.Dwarf.Collections
{
	public class SortableShadowCollection<ShadowItem, SourceItem> : ShadowCollection<ShadowItem, SourceItem>
		//where SourceItem : class
		where ShadowItem : class
	{
		//static ClassRef @class = new ClassRef(typeof(SortableShadowCollection));

		public SortableShadowCollection()
		{
			//Debug.EnableTracing(@class);

			//AlwaysRecollect = true;
		}


		public SortableShadowCollection(
			ObservableCollection<SourceItem> source,
			Func<SourceItem, ShadowItem> itemFactory,
			Predicate<SourceItem> itemFilter = null,
			Comparison<SourceItem> sortOrder = null
			)
		{
			SetItemFactory(itemFactory);
			SetItemFilter(itemFilter);
			SetSortOrder(sortOrder);
			Source = source ?? throw new ArgumentNullException(nameof(source));
		}

		public Comparison<SourceItem> SortOrder {
			get => sortOrder;
			set {
				if (SetSortOrder(value)) {
					Recollect();
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(SortOrder)));
				}
			}
		}
		Comparison<SourceItem> sortOrder;

		protected bool SetSortOrder(Comparison<SourceItem> order)
		{
			if (order != sortOrder) {
				sortOrder = order;
				return true;
			} else {
				return false;
			}
		}

		protected override void Recollect()
		{
			if (Source == null) {
				Clear();
			}
			else if (SortOrder == null) {
				base.Recollect();
			}
			else {
				var list = new List<SourceItem>();
				foreach (var item in Source) {
					if (ItemFilter(item)) {
						list.Add(item);
					}
				}
				list.Sort(SortOrder);

				using (var batch = new BatchCollectionChange(this))
				{
					Clear();

					foreach (var item in list) {
						var shadowItem = ItemFactory(item);
						if (shadowItem != null) {
							Add(shadowItem);
						}
					}

					if (HasPlaceholder) {
						Add(Placeholder);
					}
				}
			}
		}
	}
}
