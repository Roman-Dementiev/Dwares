using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.UI;
using Dwares.Druid.Satchel;
using Xamarin.Forms;
using Dwares.Dwarf.Collections;
using System.Collections.ObjectModel;

namespace Beylen.ViewModels
{
	public class CardViewModel<TSource> : PropertyNotifier, ISelectable where TSource : IModel
	{
		//static ClassRef @class = new ClassRef(typeof(CardViewModel));

		public CardViewModel(TSource source)
		{
			//Debug.EnableTracing(@class);

			if (source != null) {
				Source = source;
				Source.ModelChanged += (sender, e) => OnSourceChanged(e.ChangedProperties);
			}
			CardFrameFlavor = "Card-frame-default";
		}

		public TSource Source { get; }

		public string CardFrameFlavor {
			get => cardFrameFlavor;
			set => SetProperty(ref cardFrameFlavor, value, setModified: false);
		}
		string cardFrameFlavor;

		public bool IsSelected {
			get => isSelected;
			set {
				if (SetProperty(ref isSelected, value, setModified: false)) {
					OnSelectedChanged();
				}
			}
		}
		bool isSelected;

		public bool IsExpanded {
			get => isExpanded;
			set => SetProperty(ref isExpanded, value, setModified: false);
		}
		bool isExpanded;

		public bool ExpandSelected { get; set; } = true;

		protected virtual void OnSelectedChanged()
		{
			if (ExpandSelected)
				IsExpanded = IsSelected;

			CardFrameFlavor = IsSelected ? "Card-frame-selected" : "Card-frame-default";
		}

		protected virtual void OnSourceChanged(IEnumerable<string> changedProperties)
		{
			UpdateFromSource();
		}

		protected virtual void UpdateFromSource() { }
	}
}
