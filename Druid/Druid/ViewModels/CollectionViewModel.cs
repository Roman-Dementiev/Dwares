using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Xamarin.Forms;


namespace Dwares.Druid.ViewModels
{
	public class CollectionViewModel<TItem> : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(CollectionViewModel));

		public event Action<SelectedItemChangedEventArgs> SelectedItemChangedEvent;

		protected CollectionViewModel() { }

		public CollectionViewModel(ObservableCollection<TItem> items)
		{
			//Debug.EnableTracing(@class);
			Items = items ?? throw new ArgumentNullException(nameof(items));
		}

		public ObservableCollection<TItem> Items { get; protected set; }

		public TItem SelectedItem {
			get => selectedItem;
			set {
				if (!Equals(selectedItem, value)) {
					ChangeSelectedItem(value, Items.IndexOf(value));
				}
			}
		}
		TItem selectedItem;

		public int SelectedIndex {
			get => selectedIndex;
			set {
				if (value != selectedIndex) {
					if (value < 0 || value >= Items.Count)
						throw new ArgumentOutOfRangeException(nameof(SelectedIndex));

					ChangeSelectedItem(Items[value], value);
				}
			}
		}
		int selectedIndex = -1;

		void ChangeSelectedItem(TItem item, int index)
		{
			if (selectedItem is ISelectable oldSelectable) {
				oldSelectable.IsSelected = false;
			}

			selectedItem = item;
			selectedIndex = index;

			if (selectedItem is ISelectable newSelectable) {
				newSelectable.IsSelected = true;
			}

			FirePropertiesChanged(nameof(SelectedItem), nameof(SelectedIndex));
			OnSelectedItemChanged();
		}

		protected virtual void OnSelectedItemChanged()
		{
			if (SelectedItemChangedEvent != null) {
				var args = new SelectedItemChangedEventArgs(SelectedItem, SelectedIndex);
				SelectedItemChangedEvent(args);
			}
			UpdateCommands();
		}

		public virtual void UpdateCommands() { }
	}
}
