using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Collections
{
	public class OrderableCollection<T> : ObservableCollectionEx<T>, IOrderableCollection
	{
		//static ClassRef @class = new ClassRef(typeof(OrdarableCollection));

		public event EventHandler OrderChanged;

		public OrderableCollection()
		{
			//Debug.EnableTracing(@class);

			this.CollectionChanged += HandleCollectionChanged;
		}

		public bool AutoOrdinals {
			get => autoOrdinals;
			set {
				if (value != autoOrdinals) {
					autoOrdinals = value;
					if (value) {
						ResetOrdinals();
					}
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(AutoOrdinals)));
				}
			}
		}
		bool autoOrdinals;

		public int StartingOrdinal {
			get => startingOrdinal;
			set {
				if (value != startingOrdinal) {
					startingOrdinal = value;
					if (AutoOrdinals) {
						ResetOrdinals();
					}
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(StartingOrdinal)));
				}
			}
		}
		int startingOrdinal = 1;

		public virtual void ChangeOrder(int oldIndex, int newIndex)
		{
			var priorIndex = oldIndex;
			var latterIndex = newIndex;

			var changedItem = Items[oldIndex];
			if (newIndex < oldIndex) {
				// add one to where we delete, because we're increasing the index by inserting
				priorIndex += 1;
			} else {
				// add one to where we insert, because we haven't deleted the original yet
				latterIndex += 1;
			}

			Items.Insert(latterIndex, changedItem);
			Items.RemoveAt(priorIndex);

			FireOrderChanged();

			OnCollectionChanged(
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Move,
					changedItem,
					newIndex,
					oldIndex));
		}

		protected void FireOrderChanged()
		{
			OrderChanged?.Invoke(this, EventArgs.Empty);
		}

		public virtual void ResetOrdinals(OrdinalType ordinalType = OrdinalType.Default)
		{
			ResetOrdinals(Items, ordinalType, StartingOrdinal);
		}


		// For internal use by other implementations of IOrderableCollection
		public static void ResetOrdinals(IList<T> items, OrdinalType ordinalType, int startingOrdinal)
		{
			int ordinal = startingOrdinal;

			if (ordinalType == OrdinalType.Nested) {
				foreach (var item in items) {
					if (item is INestedOrdinal ordinalItem) {
						ordinalItem.NestedOrdinal = ordinal;
					}
					ordinal++;
				}
			} else {
				foreach (var item in items) {
					if (item is IOrdinal ordinalItem) {
						ordinalItem.Ordinal = ordinal;
					}
					ordinal++;
				}
			}
		}

		protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (AutoOrdinals) {
				ResetOrdinals();
			}
		}
	}
}
