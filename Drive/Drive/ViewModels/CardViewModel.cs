using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.UI;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Xamarin.Forms;

namespace Drive.ViewModels
{
	public class CardViewModel<TSource> : PropertyNotifier, ISelectable where TSource : class, IModel
	{
		//protected StyleSet Styles { get; } = new StyleSet();

		protected CardViewModel(TSource source, bool initStyles = true)
		{
			Source = source ?? throw new ArgumentNullException(nameof(source));
			Source.ModelChanged += (sender, e) => OnSourceChanged(e.ChangedProperties);

			CardFrameFlavor = "Card-frame-default";

			//if (initStyles) {
			//	Styles.Add(nameof(ItemFrameDefault), "Card-frame-default");
			//	Styles.Add(nameof(ItemFrameSelected), "Card-frame-selected");
			//	Styles.Add(nameof(ItemFontDefault), "Card-text-default");
			//	Styles.Add(nameof(ItemFontSmall), "Card-text-small");
			//	Styles.Add(nameof(ItemFontBold), "Card-text-bold");
				
			//	UpdateStyles();
			//}
		}

		public TSource Source { get; }

		protected virtual void OnSourceChanged(IEnumerable<string> changedProperties)
		{
			UpdateFromSource();
		}

		protected virtual void UpdateFromSource() { }

		//protected virtual void UpdateStyles()
		//{
		//	//var theme = UITheme.Current;
		//	//ItemFrameDefault = theme.GetStyleByName("ListView-item-frame-default");
		//	//ItemFrameSelected = theme.GetStyleByName("ListView-item-frame-selected");
		//	//ItemFontDefault = theme.GetStyleByName("ListView-item-font-default");
		//	//ItemFontSmall = theme.GetStyleByName("ListView-item-font-small");
		//	//ItemFontBold = theme.GetStyleByName("ListView-item-font-bold");

		//	//Styles.Update();
		//	ItemFrameStyle = IsSelected ? ItemFrameSelected : ItemFrameDefault;

		//	PropertiesChanged(nameof(ItemFrameDefault), nameof(ItemFrameSelected), nameof(ItemFrameStyle),
		//		nameof(ItemFontDefault), nameof(ItemFontSmall), nameof(ItemFontBold));
		//}

		bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				if (SetProperty(ref isSelected, value)) {
					CardFrameFlavor = isSelected ? "Card-frame-selected" : "Card-frame-default";
					//ItemFrameStyle = isSelected ? ItemFrameSelected : ItemFrameDefault;
					// TODO: why UneventRows doesn't work on UWP??
					ShowDetails = isSelected || Device.RuntimePlatform == Device.UWP;
				}
			}
		}

		bool showDetails;
		public bool ShowDetails {
			get => showDetails;
			set {
				if (SetProperty(ref showDetails, value)) {
					OnShowDetailsChanged();
				}
			}
		}

		protected virtual void OnShowDetailsChanged() { }

		//public Style ItemFrameDefault => Styles.Get(nameof(ItemFrameDefault));
		//public Style ItemFrameSelected => Styles.Get(nameof(ItemFrameSelected));
		//public Style ItemFontDefault => Styles.Get(nameof(ItemFontDefault));
		//public Style ItemFontSmall => Styles.Get(nameof(ItemFontSmall));
		//public Style ItemFontBold => Styles.Get(nameof(ItemFontBold));

		//Style itemFrameStyle;
		//public Style ItemFrameStyle {
		//	get => itemFrameStyle;
		//	set => SetProperty(ref itemFrameStyle, value);
		//}

		string cardFrameFlavor;
		public string CardFrameFlavor {
			get => cardFrameFlavor;
			set => SetProperty(ref cardFrameFlavor, value);
		}
	}
}
