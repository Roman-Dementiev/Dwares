using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ToggleImage : ContentView, IToggleControl
	{
		//public event EventHandler Tapped;
		public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

		public ToggleImage()
		{
			InitializeComponent();
		}

		protected void FireCheckedChanged()
		{
			if (CheckedChanged != null) {
				var args = new CheckedChangedEventArgs { IsChecked = this.IsChecked };
				CheckedChanged(this, args);
			}
		}

		public static readonly BindableProperty IsCheckedProperty =
			BindableProperty.Create(
				nameof(IsChecked),
				typeof(bool),
				typeof(ToggleImage),
				false,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ToggleImage control && newValue is bool isChecked) {
						control.image.Source = control.GetImageSource(isChecked);
						control.FireCheckedChanged();
					}
				});

		public bool IsChecked {
			set { SetValue(IsCheckedProperty, value); }
			get { return (bool)GetValue(IsCheckedProperty); }
		}

		public static readonly BindableProperty CheckedImageProperty =
			BindableProperty.Create(
				nameof(CheckedImage),
				typeof(ImageSource),
				typeof(ToggleImage),
				null,
				//propertyChanged: (bindable, oldValue, newValue) => {
				//	if (bindable is ToggleImage control) {
				//		control.image.Source = control.GetImageSource(control.IsChecked);
				//	}
				//}
				propertyChanged: OnImageSourceSchanged
				);

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource CheckedImage {
			set { SetValue(CheckedImageProperty, value); }
			get { return (ImageSource)GetValue(CheckedImageProperty); }
		}

		public static readonly BindableProperty UncheckedImageProperty =
			BindableProperty.Create(
				nameof(UncheckedImage),
				typeof(ImageSource),
				typeof(ToggleImage),
				null,
				//propertyChanged: (bindable, oldValue, newValue) => {
				//	if (bindable is ToggleImage control) {
				//		control.image.Source = control.GetImageSource(control.IsChecked);
				//	}
				//}
				propertyChanged: OnImageSourceSchanged
				);

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource UncheckedImage {
			set { SetValue(UncheckedImageProperty, value); }
			get { return (ImageSource)GetValue(UncheckedImageProperty); }
		}

		static void OnImageSourceSchanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ToggleImage control) {
				control.image.Source = control.GetImageSource(control.IsChecked);
			}
		}

		// TapGestureRecognizer handler.
		void OnTapped(object sender, EventArgs args)
		{
			if (IsEnabled) {
				OnTapped();

				//Tapped?.Invoke(sender, args);
			}
		}

		protected virtual void OnTapped()
		{
			IsChecked = !IsChecked;
		}

		protected ImageSource GetImageSource(bool isChecked)
		{
			if (isChecked && CheckedImage != null) {
				return CheckedImage;
			}
			return UncheckedImage;
		}

		public string CheckedActionImage {
			set {
				CheckedImage = new ActionImageSource(value);
			}
		}

		public string UncheckedActionImage {
			set {
				UncheckedImage = new ActionImageSource(value);
			}
		}

	}
}
