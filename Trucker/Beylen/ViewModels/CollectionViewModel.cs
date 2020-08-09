using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid;
using System.Threading.Tasks;

namespace Beylen.ViewModels
{
	public enum CollectionViewReloadMode
	{
		Fast,
		Full
	}

	public class CollectionViewModel<Item> : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(CollectionViewModel));

		public event Action<SelectedItemChangedEventArgs> SelectedItemChangedEvent;

		public CollectionViewModel(BindingScope parentScope, ObservableCollection<Item> items = null) :
			base(parentScope)
		{
			//Debug.EnableTracing(@class);
			Items = items ?? new ObservableCollection<Item>();
			FastLoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(CollectionViewReloadMode.Fast));
			FullLoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(CollectionViewReloadMode.Full));
		}

		public ObservableCollection<Item> Items { get; }
		public Command FastLoadItemsCommand { get; set; }
		public Command FullLoadItemsCommand { get; set; }

		public bool HasSelected() => SelectedItem != null;

		Item selectedItem;
		public Item SelectedItem {
			get => selectedItem;
			set {
				if (!Equals(selectedItem, value)) {
					ChangeSelectedItem(value, Items.IndexOf(value));
				}
			}
		}

		int selectedIndex = -1;
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

		void ChangeSelectedItem(Item item, int index)
		{
			if (selectedItem is ISelectable oldSelectable) {
				oldSelectable.IsSelected = false;
			}

			selectedItem = item;
			selectedIndex = index;

			if (selectedItem is ISelectable newSelectable) {
				newSelectable.IsSelected = true;
			}

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

		async Task ExecuteLoadItemsCommand(CollectionViewReloadMode mode)
		{
			if (reloading)
				return;

			reloading = true;
			StartBusy();

			try {
				await ReloadItems(mode);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			finally {
				reloading = false;
				ClearBusy();
			}
		}
		bool reloading = false;

		protected virtual async Task ReloadItems(CollectionViewReloadMode mode)
		{
			await Task.Delay(5000);
			await AppScope.Instance.ReloadData();
		}
	}
}
