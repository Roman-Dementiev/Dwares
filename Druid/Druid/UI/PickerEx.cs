using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class PickerEx : Picker, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(PickerEx));

		public PickerEx()
		{
			//Debug.EnableTracing(@class);

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static bool HasNativeLabel {
			get => Device.RuntimePlatform == Device.UWP;
		}

		bool useNativeLabel = false;
		public bool UseNativeLabel {
			get => useNativeLabel;
			set {
				if (HasNativeLabel && value != useNativeLabel) {
					useNativeLabel = value;
					OnPropertyChanged();
				}
			}
		}

		void SetTitle(string title)
		{
			if (UseNativeLabel) {
				base.Title = title;
			}
		}

		public static new readonly BindableProperty TitleProperty =
			BindableProperty.Create(
				nameof(Title),
				typeof(string),
				typeof(PickerEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PickerEx picker && newValue is string title) {
						picker.SetTitle(title);
					}
				});

		public new string Title {
			set { SetValue(TitleProperty, value); }
			get { return (string)GetValue(TitleProperty); }
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(PickerEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PickerEx picker) {
						picker.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
