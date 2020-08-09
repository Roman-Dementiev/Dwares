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
	public class CardViewModel<TSource> : PropertyNotifier, ISelectable where TSource : class, IModel
	{
		//static ClassRef @class = new ClassRef(typeof(CardViewModel));

		public CardViewModel(TSource source)
		{
			//Debug.EnableTracing(@class);

			Source = source ?? throw new ArgumentNullException(nameof(source));
			Source.ModelChanged += (sender, e) => UpdateFromSource(); //OnSourceChanged(e.ChangedProperties);
			CardFrameFlavor = "Card-frame-default";
		}

		public TSource Source { get; }

		public string CardFrameFlavor {
			get => cardFrameFlavor;
			set => SetProperty(ref cardFrameFlavor, value);
		}
		string cardFrameFlavor;

		public bool IsSelected {
			get => isSelected;
			set {
				if (SetProperty(ref isSelected, value)) {
					CardFrameFlavor = isSelected ? "Card-frame-selected" : "Card-frame-default";
				}
			}
		}
		bool isSelected;

		public bool IsExpanded => IsSelected;

		//public bool IsEditing {
		//	get => isEditing;
		//	set => SetProperty(ref isEditing, value);
		//}
		//bool isEditing;

		//protected virtual void OnSourceChanged(IEnumerable<string> changedProperties)
		//{
		//	UpdateFromSource();
		//}

		protected virtual void UpdateFromSource() { }
	}
}
