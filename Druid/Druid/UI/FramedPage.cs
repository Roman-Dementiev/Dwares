using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public enum FramedPageMode
	{
		Center,
		Extend,
		FullScreen
	}

	[Xamarin.Forms.ContentProperty("ContentView")]
	public class FramedPage : ContentPageEx
	{
		//static ClassRef @class = new ClassRef(typeof(FramedPage));

		public static readonly Color DefaultBorderColor = Color.Black;
		public static readonly float DefaultCornerRadius = -1;
		public static readonly Thickness DefaultFrameMargin = new Thickness(1);
		//public static readonly Thickness NoMargin = new Thickness(0);

		public FramedPage()
		{
			//Debug.EnableTracing(@class);

			Frame = new Frame() {
				BorderColor = DefaultBorderColor,
				CornerRadius = DefaultCornerRadius,
				Margin = DefaultFrameMargin
			};
			Content = Frame;
		}

		public Frame Frame { get; }

		public View ContentView {
			get => Frame.Content; 
			set => Frame.Content = value;
		}

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(
				nameof(BorderColor),
				typeof(Color),
				typeof(FramedPage),
				defaultValue: DefaultBorderColor,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FramedPage page && newValue is Color color) {
						page.Frame.BorderColor = color;
					}
				});

		public Color BorderColor {
			set { SetValue(BorderColorProperty, value); }
			get { return (Color)GetValue(BorderColorProperty); }
		}

		public static readonly BindableProperty CornerRadiusrProperty =
			BindableProperty.Create(
				nameof(CornerRadius),
				typeof(float),
				typeof(FramedPage),
				defaultValue: DefaultCornerRadius,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FramedPage page && newValue is float raduis) {
						page.Frame.CornerRadius = raduis;
					}
				});

		public float CornerRadius {
			set { SetValue(CornerRadiusrProperty, value); }
			get { return (float)GetValue(CornerRadiusrProperty); }
		}

		public static readonly BindableProperty FrameMarginProperty =
			BindableProperty.Create(
				nameof(FrameMargin),
				typeof(Thickness),
				typeof(FramedPage),
				defaultValue: DefaultFrameMargin,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FramedPage page && newValue is Thickness margin) {
						page.Frame.Margin = margin;
					}
				});

		public Thickness FrameMargin {
			set { SetValue(FrameMarginProperty, value); }
			get { return (Thickness)GetValue(FrameMarginProperty); }
		}

		public static readonly BindableProperty FrameSizeProperty =
			BindableProperty.Create(
				nameof(FrameSize),
				typeof(Size),
				typeof(FramedPage),
				//defaultValue: DefaultFrameMargin,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FramedPage page && newValue is Size newSize && oldValue is Size oldSize) {
						page.SetupFrame(page.Mode, newSize, page.Mode, oldSize);
					}
				});

		public Size FrameSize {
			set { SetValue(FrameSizeProperty, value); }
			get { return (Size)GetValue(FrameSizeProperty); }
		}

		public static readonly BindableProperty ModeProperty =
			BindableProperty.Create(
				nameof(Mode),
				typeof(FramedPageMode),
				typeof(FramedPage),
				defaultValue: DefaultMode,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FramedPage page && newValue is FramedPageMode newMode && oldValue is FramedPageMode oldMode) {
						page.SetupFrame(newMode, page.FrameSize, oldMode, page.FrameSize);
					}
				});

		public FramedPageMode Mode {
			set { SetValue(ModeProperty, value); }
			get { return (FramedPageMode)GetValue(ModeProperty); }
		}

		public static FramedPageMode DefaultMode {
			get => FramedPageMode.Center;
		}

		void SetupFrame(FramedPageMode newMode, Size newSize, FramedPageMode prevMode, Size prevSize)
		{
			if (newMode != prevMode || (newMode == FramedPageMode.Center && newSize != prevSize))
			{
				if (newMode == FramedPageMode.Center) {
					Frame.WidthRequest = newSize.Width;
					Frame.HeightRequest = newSize.Height;
					Frame.Margin = FrameMargin;
					Frame.HorizontalOptions = LayoutOptions.Center;
					Frame.VerticalOptions = LayoutOptions.Center;
					Frame.BorderColor = BorderColor;
				}
				else if (newMode == FramedPageMode.Extend)
				{
					Frame.HorizontalOptions = LayoutOptions.Fill;
					Frame.VerticalOptions = LayoutOptions.Fill;
					Frame.BorderColor = BorderColor;
				}
				else {
					//Frame.Margin = NoMargin;
					Frame.HorizontalOptions = LayoutOptions.Fill;
					Frame.VerticalOptions = LayoutOptions.Fill;
					Frame.BorderColor = Color.Transparent;
				}
			}
		}
	}
}
