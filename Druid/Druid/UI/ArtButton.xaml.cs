﻿using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Painting;
using Dwares.Druid.Satchel;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf;
using Xamarin.Forms.Markup;

namespace Dwares.Druid.UI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ArtButton : ContentView, IThemeAware
	{
		public event EventHandler Tapped;
		public event EventHandler DoubleTapped;

		public ArtButton()
		{
			InitializeComponent();

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
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
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is float value) {
						button.frame.CornerRadius = value;
					}
				});

		public float CornerRadius {
			set { SetValue(CornerRadiusProperty, value); }
			get { return (float)GetValue(CornerRadiusProperty); }
		}

		const int innerMarginAdjustment = -12;
		public static readonly BindableProperty InnerMarginProperty =
			BindableProperty.Create(
				nameof(InnerMargin),
				typeof(Thickness),
				typeof(ArtButton),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is Thickness value) {
						button.layout.Margin = value.Add(innerMarginAdjustment);
					}
				});

		[TypeConverter(typeof(ThicknessTypeConverter))]
		public Thickness InnerMargin {
			set { SetValue(InnerMarginProperty, value); }
			get { return (Thickness)GetValue(InnerMarginProperty); }
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

		public static readonly BindableProperty LabelIsVisibleProperty =
			BindableProperty.Create(
				nameof(LabelIsVisible),
				typeof(bool),
				typeof(ArtButton),
				defaultValue: true,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is bool value) {
						button.label.IsVisible = value;
					}
				});

		public bool LabelIsVisible {
			set { SetValue(LabelIsVisibleProperty, value); }
			get { return (bool)GetValue(LabelIsVisibleProperty); }
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

		//[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize {
			set { SetValue(FontSizeProperty, value); }
			get { return (double)GetValue(FontSizeProperty); }
		}

		[TypeConverter(typeof(FontAttributesConverter))]
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



		public static readonly BindableProperty IconIsVisibleProperty =
			BindableProperty.Create(
				nameof(IconIsVisible),
				typeof(bool),
				typeof(ArtButton),
				defaultValue: true,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is bool value) {
						button.image.IsVisible = value;
					}
				});

		public bool IconIsVisible {
			set { SetValue(IconIsVisibleProperty, value); }
			get { return (bool)GetValue(IconIsVisibleProperty); }
		}

		public static readonly BindableProperty IconWidthProperty =
			BindableProperty.Create(
				nameof(IconWidth),
				typeof(double),
				typeof(ArtButton),
				//defaultValue: 80,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is double value) {
						button.image.WidthRequest = value;
					}
				});

		public double IconWidth {
			set { SetValue(IconWidthProperty, value); }
			get { return (double)GetValue(IconWidthProperty); }
		}

		public static readonly BindableProperty IconHeightProperty =
			BindableProperty.Create(
				nameof(IconHeight),
				typeof(double),
				typeof(ArtButton),
				//defaultValue: 80,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is double value) {
						button.image.HeightRequest = value;

					}
				});

		public double IconHeight {
			set { SetValue(IconHeightProperty, value); }
			get { return (double)GetValue(IconHeightProperty); }
		}

		public static readonly BindableProperty IconArtProperty =
			BindableProperty.Create(
				nameof(IconArt),
				typeof(string),
				typeof(ArtButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button && newValue is string value) {
						button.SelectImageSource(value, button.ArtColor);
					}
				});

		public string IconArt {
			set { SetValue(IconArtProperty, value); }
			get { return (string)GetValue(IconArtProperty); }
		}

		public static readonly BindableProperty ArtColorProperty =
			BindableProperty.Create(
				nameof(ArtColor),
				typeof(Color?),
				typeof(ArtButton),
				defaultValue: null,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button) {
						if (newValue == null) {
							button.SelectImageSource(button.IconArt, null);
						}
						else if (newValue is Color color) {
							button.SelectImageSource(button.IconArt, color);
						}
					}
				});

		[TypeConverter(typeof(ColorTypeConverter))]
		public Color? ArtColor {
			set { SetValue(ArtColorProperty, value); }
			get { return (Color?)GetValue(ArtColorProperty); }
		}

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(
				nameof(Command),
				typeof(ICommand),
				typeof(ArtButton)
				//propertyChanged: (bindable, oldValue, newValue) => {
				//	if (bindable is ArtButtonEx button && newValue is string value) {
				//		Debug.Print($"CommandProperty changed");
				//	}
				//}
				);

		public ICommand Command {
			set { SetValue(CommandProperty, value); }
			get { return (ICommand)GetValue(CommandProperty); }
		}

		public static readonly BindableProperty CommandParameterProperty =
			BindableProperty.Create(
				nameof(CommandParameter),
				typeof(object),
				typeof(ArtButton)
				//propertyChanged: (bindable, oldValue, newValue) => {
				//	if (bindable is ArtButtonEx button && newValue is string value) {
				//		Debug.Print($"CommandParameterProperty changed");
				//	}
				//}
				);

		public object CommandParameter {
			set { SetValue(CommandParameterProperty, value); }
			get { return GetValue(CommandParameterProperty); }
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ArtButton),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButton button) {
						button.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}


		//[TypeConverter(typeof(ImageSourceConverter))]
		//public ImageSource ImageSource {
		//	get => image.Source;
		//	set => image.Source = value;
		//}

		protected virtual void SelectImageSource(string name, Color? color)
		{
			image.Source = ArtBroker.Instance.GetImageSource(name, null, color);
		}

		public string Writ {
			get => writ;
			set {
				if (value != writ) {
					OnPropertyChanging();
					writ = value;
					Command = new WritCommand(writ);
					OnPropertyChanged();
				}
			}
		}
		string writ;

		// TapGestureRecognizer handler.
		protected virtual void OnTapped(object sender, EventArgs args)
		{
			if (IsEnabled && Command != null && Command.CanExecute(CommandParameter)) {
				//Debug.Print("ArtButton.OnTapped(): Wtit={0}", Writ);
				Command.Execute(CommandParameter);
			}

			Tapped?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnDoubleTapped(object sender, EventArgs args)
		{
			//Debug.Print("ArtButton.OnDoubleTapped()");
			DoubleTapped?.Invoke(this, EventArgs.Empty);
		}

		protected Image Image => image;
	}
}
