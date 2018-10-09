using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Druid.Support;
using ACE.Models;

namespace ACE.ViewModels
{
	public delegate void SelectedItemChangedHandler(SelectedItemChangedEventArgs args);

	public class CollectionViewModel<Item> : BindingScope
	{
		public event SelectedItemChangedHandler SelectedItemChangedEvent;
	
		public CollectionViewModel(ObservableCollection<Item> items = null) 
		{
			Items = items ?? new ObservableCollection<Item>();
		}

		public ObservableCollection<Item> Items { get; }

		object selectedItem;
		public object SelectedItem {
			get => selectedItem;
			set {
				if (!EqualityComparer<object>.Default.Equals(selectedItem, value)) {
					if (selectedItem is ISelectable selectable) {
						selectable.IsSelected = false;
					}
					selectedItem = value;
					if (selectedItem is ISelectable newSelectable) {
						newSelectable.IsSelected = true;
					}
					OnSelectedItemChanged();
				}
			}
		}

		public Item Selected {
			get {
				if (SelectedItem is Item item) {
					return item;
				} else {
					return default(Item);
				}
			}
			set => SelectedItem = value;
		}

		public bool HasSelected() => SelectedItem != null;

		protected virtual void OnSelectedItemChanged()
		{
			if (SelectedItemChangedEvent != null) {
				var args = new SelectedItemChangedEventArgs(SelectedItem);
				SelectedItemChangedEvent(args);
			}
		}
	}

}
