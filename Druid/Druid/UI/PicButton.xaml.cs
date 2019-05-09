using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf;

namespace Dwares.Druid.UI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PicButton : ContentView, ICommandHolder //, ISelectable
	{
		WritMixin wmix;
		//public event EventHandler Tapped;

		public PicButton()
		{
			wmix = new WritMixin(this);

			InitializeComponent();
		}

		public static readonly BindableProperty OrientationProperty =
			BindableProperty.Create(
				nameof(Orientation),
				typeof(StackOrientation),
				typeof(PicButton),
				//defaultValue: StackOrientation.Vertical,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is StackOrientation value) {
						button.layout.Orientation = value;
					}
				});

		public StackOrientation Orientation {
			set { SetValue(OrientationProperty, value); }
			get { return (StackOrientation)GetValue(OrientationProperty); }
		}

		public static readonly BindableProperty LabelTextProperty =
			BindableProperty.Create(
				nameof(LabelText),
				typeof(string),
				typeof(PicButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is string value) {
						button.label.Text = value;
					}
				});

		public string LabelText {
			set { SetValue(LabelTextProperty, value); }
			get { return (string)GetValue(LabelTextProperty); }
		}

		public static readonly BindableProperty LabelColorProperty =
			BindableProperty.Create(
				nameof(LabelColor),
				typeof(Color),
				typeof(PicButton),
				defaultValue: Color.Black,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is Color value) {
						button.label.TextColor = value;
					}
				});

		[TypeConverter(typeof(ColorTypeConverter))]
		public Color LabelColor {
			set { SetValue(LabelColorProperty, value); }
			get { return (Color)GetValue(LabelColorProperty); }
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(
				nameof(FontSize),
				typeof(double),
				typeof(PicButton),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is double value) {
						button.label.FontSize = value;
					}
				});

		public double FontSize {
			set { SetValue(FontSizeProperty, value); }
			get { return (double)GetValue(FontSizeProperty); }
		}

		public static readonly BindableProperty FontAttributesProperty =
			BindableProperty.Create(
				nameof(FontAttributes),
				typeof(FontAttributes),
				typeof(PicButton),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is FontAttributes value) {
						button.label.FontAttributes = value;
					}
				});

		public FontAttributes FontAttributes {
			set { SetValue(FontAttributesProperty, value); }
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
		}



		public static readonly BindableProperty ImageWidthProperty =
			BindableProperty.Create(
				nameof(ImageWidth),
				typeof(double),
				typeof(PicButton),
				//defaultValue: 80,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is double value) {
						button.image.WidthRequest = value;
					}
				});

		public double ImageWidth {
			set { SetValue(ImageWidthProperty, value); }
			get { return (double)GetValue(ImageWidthProperty); }
		}

		public static readonly BindableProperty ImageHeightProperty =
			BindableProperty.Create(
				nameof(ImageHeight),
				typeof(double),
				typeof(PicButton),
				//defaultValue: 80,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is double value) {
						button.image.HeightRequest = value;

					}
				});

		public double ImageHeight {
			set { SetValue(ImageHeightProperty, value); }
			get { return (double)GetValue(ImageHeightProperty); }
		}

		public static readonly BindableProperty ImageNameProperty =
			BindableProperty.Create(
				nameof(ImageName),
				typeof(string),
				typeof(PicButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is string value) {
						button.ImageSource = ImageProvider.GetImageSource(value);
					}
				});

		public string ImageName {
			set { SetValue(ImageNameProperty, value); }
			get { return (string)GetValue(ImageNameProperty); }
		}

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource ImageSource {
			get => imageSource;
			set {
				if (value != imageSource) {
					imageSource = value;
					UodateImageSource();
				}
			}
		}
		ImageSource imageSource;

/*
		public static readonly BindableProperty SelectedImageNameProperty =
			BindableProperty.Create(
				nameof(SelectedImageName),
				typeof(string),
				typeof(PicButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is string value) {
						button.SelectedImageSource = ImageProvider.GetImageSource(value);
					}
				});

		public string SelectedImageName {
			set { SetValue(SelectedImageNameProperty, value); }
			get { return (string)GetValue(SelectedImageNameProperty); }
		}


		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource SelectedImageSource {
			get => selectedImageSource;
			set {
				if (value != selectedImageSource) {
					selectedImageSource = value;
					UodateImageSource();
				}
			}
		}
		ImageSource selectedImageSource;

		public static readonly BindableProperty IsSelectedProperty =
			BindableProperty.Create(
				nameof(IsSelected),
				typeof(bool),
				typeof(PicButton),
				//defaultValue: false,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PicButton button && newValue is StackOrientation value) {
						button.layout.Orientation = value;
					}
				});

		public bool IsSelected {
			set { SetValue(IsSelectedProperty, value); }
			get { return (bool)GetValue(IsSelectedProperty); }
		}


		void UodateImageSource()
		{
			if (IsSelected && selectedImageSource != null) {
				image.Source = selectedImageSource;
			} else {
				image.Source = imageSource;
			}
		}
*/

		void UodateImageSource()
		{
			image.Source = imageSource;
		}

		public ICommand Command { get; set; }

		public WritCommand WritCommand {
			get => wmix.WritCommand;
			set => wmix.WritCommand = value;
		}

		public string Writ {
			get => wmix.Writ;
			set => wmix.Writ = value;
		}

		// TapGestureRecognizer handler.
		void OnTapped(object sender, EventArgs args)
		{
			//if (IsEnabled) {
			//	Tapped?.Invoke(sender, args);
			//}
			//Debug.Print("Dwares.Druid.UI.PicButton.OnTapped");

			if (IsEnabled && Command != null && Command.CanExecute(null)) {
				Command.Execute(null);
			}
		}
	}
}
