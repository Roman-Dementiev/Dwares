using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class PickerEx : Picker
	{
		//static ClassRef @class = new ClassRef(typeof(PickerEx));

		public PickerEx()
		{
			//Debug.EnableTracing(@class);
			Debug.Print("PixkeEx.PickerEx()");

			this.ApplyTheme();
			UITheme.CurrentThemeChanged += OnCurrentUIhemeChanged;
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

		public static readonly BindableProperty ThemeStyleProperty =
			BindableProperty.Create(
				nameof(ThemeStyle),
				typeof(string),
				typeof(PickerEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PickerEx picker) {
						picker.ApplyTheme();
					}
				});

		public string ThemeStyle {
			set { SetValue(ThemeStyleProperty, value); }
			get { return (string)GetValue(ThemeStyleProperty); }
		}

		private void OnCurrentUIhemeChanged(object sender, EventArgs e)
		{
			this.ApplyTheme();
		}
	}
}
