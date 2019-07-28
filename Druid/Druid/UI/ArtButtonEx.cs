using System;
using System.Windows.Input;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;
using Xamarin.Forms;

namespace Dwares.Druid.UI
{
	public class ArtButtonEx : ArtButton, ISelectable
	{
		//static ClassRef @class = new ClassRef(typeof(ArtButtonEx));

		public ArtButtonEx()
		{
			//Debug.EnableTracing(@class);

			UITheme.CurrentThemeChanged += UITheme_CurrentThemeChanged;
		}

		private void UITheme_CurrentThemeChanged(object sender, EventArgs e)
		{
			Update();
		}

		//public static readonly BindableProperty IsSelectedProperty =
		//	BindableProperty.Create(
		//		nameof(IsSelected),
		//		typeof(bool),
		//		typeof(ArtButtonEx),
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is ArtButtonEx button && newValue is bool value) {
		//				button.Update();
		//			}
		//		});

		//public bool IsSelected {
		//	set { SetValue(IsSelectedProperty, value); }
		//	get { return (bool)GetValue(IsSelectedProperty); }
		//}

		bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				if (value != isSelected) {
					isSelected = value;
					Update();
				}
			}
		}

		public static readonly BindableProperty DefaultFlavorProperty =
			BindableProperty.Create(
				nameof(DefaultFlavor),
				typeof(string),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button) {
						button.Update();
					}
				});

		public string DefaultFlavor {
			set { SetValue(DefaultFlavorProperty, value); }
			get { return (string)GetValue(DefaultFlavorProperty); }
		}

		public static readonly BindableProperty SelectedFlavorProperty =
			BindableProperty.Create(
				nameof(SelectedFlavor),
				typeof(string),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button) {
						button.Update();
					}
				});

		public string SelectedFlavor {
			set { SetValue(SelectedFlavorProperty, value); }
			get { return (string)GetValue(SelectedFlavorProperty); }
		}

		public static readonly BindableProperty DisabledFlavorProperty =
			BindableProperty.Create(
				nameof(DisabledFlavor),
				typeof(string),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button) {
						button.Update();
					}
				});

		public string DisabledFlavor {
			set { SetValue(DisabledFlavorProperty, value); }
			get { return (string)GetValue(DisabledFlavorProperty); }
		}

		//public static readonly BindableProperty DefaultArtProperty =
		//	BindableProperty.Create(
		//		nameof(DefaultArt),
		//		typeof(string),
		//		typeof(ArtButton),
		//		defaultValue: string.Empty,
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is ArtButtonEx button && newValue is string value) {
		//				button.Update();
		//			}
		//		});

		//public string DefaultArt {
		//	set { SetValue(DefaultArtProperty, value); }
		//	get { return (string)GetValue(DefaultArtProperty); }
		//}

		//public static readonly BindableProperty SelectedArtProperty =
		//	BindableProperty.Create(
		//		nameof(SelectedArt),
		//		typeof(string),
		//		typeof(ArtButton),
		//		defaultValue: string.Empty,
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is ArtButtonEx button && newValue is string value) {
		//				button.Update();
		//			}
		//		});

		//public string SelectedArt {
		//	set { SetValue(SelectedArtProperty, value); }
		//	get { return (string)GetValue(SelectedArtProperty); }
		//}

		//public static readonly BindableProperty DisabledArtProperty =
		//	BindableProperty.Create(
		//		nameof(DisabledArt),
		//		typeof(string),
		//		typeof(ArtButton),
		//		defaultValue: string.Empty,
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is ArtButtonEx button && newValue is string value) {
		//				button.Update();
		//			}
		//		});

		//public string DisabledArt {
		//	set { SetValue(DisabledArtProperty, value); }
		//	get { return (string)GetValue(DisabledArtProperty); }
		//}

		protected virtual void Update()
		{
			string /*art = null,*/ flavor = null;
			if (!IsEnabled) {
				//if (!string.IsNullOrEmpty(DisabledArt))
				//	art = DisabledArt;
				if (!string.IsNullOrEmpty(DisabledFlavor))
					flavor = DisabledFlavor;
			} else if (IsSelected) {
				//if (!string.IsNullOrEmpty(SelectedArt))
				//	art = SelectedArt;
				if (!string.IsNullOrEmpty(SelectedFlavor))
					flavor = SelectedFlavor;
			}

			//if (string.IsNullOrEmpty(art))
			//	art = DefaultArt;
			if (string.IsNullOrEmpty(flavor))
				flavor = DefaultFlavor;

			//if (!string.IsNullOrEmpty(art))
			//	IconArt = art;
			if (!string.IsNullOrEmpty(flavor))
				Flavor = flavor;
		}

		protected override void SelectImageSource(string name, Color? color)
		{
			base.SelectImageSource(name, color);
			//ImageSource = UITheme.Current.GetImage(name);
		}
	}
}
