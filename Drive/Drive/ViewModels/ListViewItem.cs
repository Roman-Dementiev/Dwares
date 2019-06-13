using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.UI;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Xamarin.Forms;

namespace Drive.ViewModels
{
	public class ListViewItem<TSource> : PropertyNotifier, ISelectable where TSource : class, IModel
	{
		protected StyleSet Styles { get; } = new StyleSet();

		protected ListViewItem(TSource source, bool initStyles = true)
		{
			Source = source ?? throw new ArgumentNullException(nameof(source));
			Source.ModelChanged += (sender, e) => OnSourceChanged(e.ChangedProperties);


			if (initStyles) {
				Styles.Add(nameof(ItemFrameDefault), "ListView-item-frame-default");
				Styles.Add(nameof(ItemFrameSelected), "ListView-item-frame-selected");
				Styles.Add(nameof(ItemFontDefault), "ListView-item-font-default");
				Styles.Add(nameof(ItemFontSmall), "ListView-item-font-small");
				Styles.Add(nameof(ItemFontBold), "ListView-item-font-bold");
				
				UpdateStyles();
			}
		}

		public TSource Source { get; }

		protected virtual void OnSourceChanged(IEnumerable<string> changedProperties)
		{
			UpdateFromSource();
		}

		protected virtual void UpdateFromSource() { }

		protected virtual void UpdateStyles()
		{
			//var theme = UITheme.Current;
			//ItemFrameDefault = theme.GetStyleByName("ListView-item-frame-default");
			//ItemFrameSelected = theme.GetStyleByName("ListView-item-frame-selected");
			//ItemFontDefault = theme.GetStyleByName("ListView-item-font-default");
			//ItemFontSmall = theme.GetStyleByName("ListView-item-font-small");
			//ItemFontBold = theme.GetStyleByName("ListView-item-font-bold");

			Styles.Update();
			ItemFrameStyle = IsSelected ? ItemFrameSelected : ItemFrameDefault;

			PropertiesChanged(nameof(ItemFrameDefault), nameof(ItemFrameSelected), nameof(ItemFrameStyle),
				nameof(ItemFontDefault), nameof(ItemFontSmall), nameof(ItemFontBold));
		}

		bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				if (SetProperty(ref isSelected, value)) {
					ItemFrameStyle = isSelected ? ItemFrameSelected : ItemFrameDefault;
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

		//public Style ItemFrameDefault { get; protected set; }
		//public Style ItemFrameSelected { get; protected set; }
		//public Style ItemFontDefault { get; protected set; }
		//public Style ItemFontSmall { get; protected set; }
		//public Style ItemFontBold { get; protected set; }

		public Style ItemFrameDefault => Styles.Get(nameof(ItemFrameDefault));
		public Style ItemFrameSelected => Styles.Get(nameof(ItemFrameSelected));
		public Style ItemFontDefault => Styles.Get(nameof(ItemFontDefault));
		public Style ItemFontSmall => Styles.Get(nameof(ItemFontSmall));
		public Style ItemFontBold => Styles.Get(nameof(ItemFontBold));

		Style itemFrameStyle;
		public Style ItemFrameStyle {
			get => itemFrameStyle;
			set => SetProperty(ref itemFrameStyle, value);
		}
	}
}
