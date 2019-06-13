using Dwares.Druid.Satchel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dwares.Druid.UI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ArtButton : ContentView
	{
		const float kDefaultImageSize = 40;
		const float kDefaultFontSize = 16;
		const float kDefaultCornerRadius = 8;

		public ArtButton()
		{
			InitializeComponent();

			frame.CornerRadius = kDefaultCornerRadius;
			frame.BorderColor = Color.Transparent;
			frame.BackgroundColor = Color.Transparent;

			image.WidthRequest = image.HeightRequest = kDefaultImageSize;

			label.FontSize = kDefaultFontSize;
		}

		public static readonly BindableProperty OrientationProperty =
			BindableProperty.Create(
				nameof(Orientation),
				typeof(StackOrientation),
				typeof(ArtButton),
				//defaultValue: StackOrientation.Vertical,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is StackOrientation value) {
						button.layout.Orientation = value;
					}
				});

		public StackOrientation Orientation {
			set { SetValue(OrientationProperty, value); }
			get { return (StackOrientation)GetValue(OrientationProperty); }
		}

		public static readonly BindableProperty ImageWidthProperty =
			BindableProperty.Create(
				nameof(ImageWidth),
				typeof(double),
				typeof(ArtButton),
				defaultValue: kDefaultImageSize,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is double value) {
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
				typeof(ArtButton),
				defaultValue: kDefaultImageSize,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is double value) {
						button.image.HeightRequest = value;
					}
				});

		public double ImageHeight {
			set { SetValue(ImageHeightProperty, value); }
			get { return (double)GetValue(ImageHeightProperty); }
		}

		public static readonly BindableProperty LabelTextProperty =
			BindableProperty.Create(
				nameof(LabelText),
				typeof(string),
				typeof(ArtButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is string value) {
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
				typeof(ArtButton),
				defaultValue: Color.Black,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is Color value) {
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
				typeof(ArtButton),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is double value) {
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
				typeof(ArtButton),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is FontAttributes value) {
						button.label.FontAttributes = value;
					}
				});

		public FontAttributes FontAttributes {
			set { SetValue(FontAttributesProperty, value); }
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
		}


		public static new readonly BindableProperty BackgroundColorProperty =
			BindableProperty.Create(
				nameof(BackgroundColor),
				typeof(Color),
				typeof(ArtButton),
				defaultValue: Color.Transparent,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is Color value) {
						button.frame.BackgroundColor = value;
					}
				});

		[TypeConverter(typeof(ColorTypeConverter))]
		public new Color BackgroundColor {
			set { SetValue(BackgroundColorProperty, value); }
			get { return (Color)GetValue(BackgroundColorProperty); }
		}

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(
				nameof(BorderColor),
				typeof(Color),
				typeof(ArtButton),
				defaultValue: Color.Transparent,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is Color value) {
						button.frame.BorderColor = value;
					}
				});

		[TypeConverter(typeof(ColorTypeConverter))]
		public Color BorderColor {
			set { SetValue(BorderColorProperty, value); }
			get { return (Color)GetValue(BorderColorProperty); }
		}

		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create(
				nameof(CornerRadius),
				typeof(float),
				typeof(ArtButton),
				defaultValue: kDefaultCornerRadius,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is Color value) {
						button.frame.BorderColor = value;
					}
				});

		public float CornerRadius {
			set { SetValue(CornerRadiusProperty, value); }
			get { return (float)GetValue(CornerRadiusProperty); }
		}

		//[TypeConverter(typeof(ImageSourceConverter))]
		//public ImageSource ImageSource {
		//	get => imageSource;
		//	set {
		//		if (value != imageSource) {
		//			imageSource = value;
		//			image.Source = imageSource;
		//		}
		//	}
		//}
		//ImageSource imageSource;

		public static readonly BindableProperty ArtProperty =
			BindableProperty.Create(
				nameof(Art),
				typeof(string),
				typeof(ArtButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is string value) {
						button.image.Source = ImageProvider.GetImageSource(value);
					}
				});

		public string Art {
			set { SetValue(ArtProperty, value); }
			get { return (string)GetValue(ArtProperty); }
		}

	}
}
