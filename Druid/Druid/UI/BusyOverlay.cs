using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class BusyOverlay : ContentView, IThemeAware
	{
		public static Color DefaultBackgroundColor => Color.FromHex("#C0808080");
		public static Color DefaultMessageTextColor => Color.Black;
		public static Color DefaultMessageBackColor => Color.White;
		public static Thickness DefaultContentMargin => new Thickness(8);
		public static LayoutOptions DefaultHorizontalOptions => LayoutOptions.Center;
		public static LayoutOptions DefaultVerticalOptions => LayoutOptions.Center;

		public static Thickness DefaultMessageFramePadding => new Thickness(10, 8);
		public static float DefaultMessageFrameCornerRadius => 10;


		protected BusyOverlay(bool setContent)
		{
			IsVisible = false;
			BackgroundColor = DefaultBackgroundColor;

			MessageLabel = new Label {
				HorizontalOptions = LayoutOptions.Center,
				TextColor = DefaultMessageTextColor
			};
			MessageFrame = new Frame {
				BackgroundColor = DefaultMessageBackColor,
				HorizontalOptions = DefaultHorizontalOptions,
				VerticalOptions = DefaultVerticalOptions,
				Padding = DefaultMessageFramePadding,
				CornerRadius = DefaultMessageFrameCornerRadius,
				Content = MessageLabel
			};

			if (setContent) {
				//Content = new StackLayout {
				//	HorizontalOptions = DefaultHorizontalOptions,
				//	VerticalOptions = DefaultVerticalOptions,
				//	Margin = DefaultContentMargin,
				//	Children = {
				//		MessageFrame
				//	}
				//};
				Content = MessageFrame;
				Content.Margin = DefaultContentMargin;
			}
		}

		public BusyOverlay() : this(true)
		{
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public Label MessageLabel { get; protected set; }
		public Frame MessageFrame { get; protected set; }

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(BusyOverlay),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is BusyOverlay overlay) {
						overlay.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}


		public static readonly BindableProperty MessageProperty =
			BindableProperty.Create(
				nameof(Message),
				typeof(string),
				typeof(BusyOverlay),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is BusyOverlay overlay && newValue is string text) {
						if (string.IsNullOrWhiteSpace(text)) {
							overlay.MessageLabel.Text = string.Empty;
							overlay.MessageFrame.IsVisible = false;
						} else {
							overlay.MessageLabel.Text = text;
							overlay.MessageFrame.IsVisible = true;
						}
					}
				});

		public string Message {
			set { SetValue(MessageProperty, value); }
			get { return (string)GetValue(MessageProperty); }
		}

		public static readonly BindableProperty MessageTextColorProperty =
			BindableProperty.Create(
				nameof(MessageTextColor),
				typeof(Color),
				typeof(BusyOverlay),
				defaultValue: DefaultMessageBackColor,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is BusyOverlay overlay && newValue is Color color) {
						overlay.MessageLabel.TextColor = color;
					}
				});

		public Color MessageTextColor {
			set { SetValue(MessageTextColorProperty, value); }
			get { return (Color)GetValue(MessageTextColorProperty); }
		}

		public static readonly BindableProperty MessageBackColorProperty =
			BindableProperty.Create(
				nameof(MessageBackColor),
				typeof(Color),
				typeof(BusyOverlay),
				defaultValue: DefaultMessageTextColor,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is BusyOverlay overlay && newValue is Color color) {
						overlay.MessageFrame.BackgroundColor = color;
					}
				});

		public Color MessageBackColor {
			set { SetValue(MessageBackColorProperty, value); }
			get { return (Color)GetValue(MessageBackColorProperty); }
		}

		public static new readonly BindableProperty HorizontalOptionsProperty =
			BindableProperty.Create(
				nameof(HorizontalOptions),
				typeof(LayoutOptions),
				typeof(BusyOverlay),
				defaultValue: DefaultHorizontalOptions,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is BusyOverlay overlay && newValue is LayoutOptions options) {
						overlay.Content.HorizontalOptions = options;
					}
				});

		public new LayoutOptions HorizontalOptions {
			set { SetValue(HorizontalOptionsProperty, value); }
			get { return (LayoutOptions)GetValue(HorizontalOptionsProperty); }
		}

		public static new readonly BindableProperty VerticalOptionsProperty =
			BindableProperty.Create(
				nameof(VerticalOptions),
				typeof(LayoutOptions),
				typeof(BusyOverlay),
				defaultValue: DefaultVerticalOptions,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is BusyOverlay overlay && newValue is LayoutOptions options) {
						overlay.Content.VerticalOptions = options;
					}
				});

		public new LayoutOptions VerticalOptions {
			set { SetValue(VerticalOptionsProperty, value); }
			get { return (LayoutOptions)GetValue(VerticalOptionsProperty); }
		}
	}
}
