using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace Dwares.Dwarf.Collections
{
	public class OrderedCollection<T> : ObservableCollection<T>
	{
		public OrderedCollection() { }

		public OrderedCollection(Comparer<T> order)
		{
			this.order = order;
		}

		public OrderedCollection(Comparison<T> comparison, bool descending=false)
		{
			if (descending) {
				order = Comparer<T>.Create((x, y) => comparison(y, x));
			} else {
				order = Comparer<T>.Create(comparison);
			}
		}

		public Comparer<T> order;
		public Comparer<T> Order {
			get => order;
			set { 
				if (value != order) {
					order = value;

					if (order != null) {
						this.Sort(order);
					}
				}
			}
		}

		public new void Add(T item)
		{
			if (Order != null) {
				int index = this.BinarySearch(item, Order);
				if (index < 0) {
					index = ~index;
				}

				base.InsertItem(index, item);
			} else {
				base.Add(item);
			}
		}
	}
}
