﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid;
using ACE.Models;


namespace ACE.ViewModels
{
	public delegate void SelectedItemChangedHandler(SelectedItemChangedEventArgs args);

	public class CollectionViewModel<Item> : BindingScope
	{
		public event SelectedItemChangedHandler SelectedItemChangedEvent;

		protected CollectionViewModel() : base(null) { }

		public CollectionViewModel(BindingScope parentScope, ObservableCollection<Item> items = null) :
			base(parentScope)
		{
			Items = items ?? new ObservableCollection<Item>();
		}

		public ObservableCollection<Item> Items { get; protected set; }

		// TODO
		public Thickness PanelMargin { get; set; } = new Thickness(0, 2, 0, 0);
		public Color PanelBackgroundColor { get; set; } = Color.LightGray;
		public Color PanelOutlineColor { get; set; } = Color.Black;

		object selectedObject;
		public object SelectedObject {
			get => selectedObject;
			set {
				if (!EqualityComparer<object>.Default.Equals(selectedObject, value)) {
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
			UpdateCommands();
		}
	}
}
