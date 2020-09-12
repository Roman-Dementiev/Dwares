using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Collections
{
	public class OrderableCollection<T> : ObservableCollection<T>, IOrderableCollection
	{
		//static ClassRef @class = new ClassRef(typeof(OrdarableCollection));

		public event EventHandler OrderChanged;

		public OrderableCollection()
		{
			//Debug.EnableTracing(@class);
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

			if (AutoOrdinals) {
				ResetOrdinals();
			}

			OrderChanged?.Invoke(this, EventArgs.Empty);

			OnCollectionChanged(
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Move,
					changedItem,
					newIndex,
					oldIndex));
		}

		public virtual void ResetOrdinals(OrdinalType type = OrdinalType.Default)
		{
			int ordinal = StartingOrdinal;
			foreach (var item in Items) {
				if (item is IOrdinal ordinalItem) {
					ordinalItem.SetOrdinal(ordinal, type);
				}
				ordinal++;
			}

		}
	}
}
