using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class FramedPage : DecoratedPage
	{
		//static ClassRef @class = new ClassRef(typeof(FramedPage));

		public static Color DefaultBorderColor { get; set; } = Color.Black;
		public static float DefaultCornerRadius { get; set; } = -1;
		public static Thickness DefaultFrameMargin { get; set; } = new Thickness(1);

		public FramedPage()
		{
			//Debug.EnableTracing(@class);

			Frame = new FrameEx() {
				BorderColor = DefaultBorderColor,
				CornerRadius = DefaultCornerRadius,
				Margin = DefaultFrameMargin
			};
			LayoutDecorection();

			Content = Frame;
		}

		public FrameEx Frame { get; protected set; }

		public override IContentHolder Decoration { 
			get => Frame;
			set {
				var frame = value as FrameEx;
				if (frame != Frame) {
					OnPropertyChanging();
					Frame = frame;
					OnPropertyChanged();
				}
			}
		}

		protected override void LayoutDecorection()
		{
			LayoutOptions layoutOptions;
			
			switch (DecorationLayout)
			{
			case DecorationLayout.Center:
				layoutOptions = LayoutOptions.Center;
				break;
		
			case DecorationLayout.FullScreen:
				layoutOptions = LayoutOptions.FillAndExpand;
				break;

			default:
				return;
			}

			Frame.HorizontalOptions = Frame.VerticalOptions = layoutOptions;
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

		public static readonly BindableProperty BorderIsVisibleProperty =
			BindableProperty.Create(
				nameof(BorderIsVisible),
				typeof(bool),
				typeof(FramedPage),
				defaultValue: true,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FramedPage page && newValue is bool isVisible) {
						page.Frame.BorderColor = isVisible ? page.BorderColor : Color.Transparent;
					}
				});

		public bool BorderIsVisible {
			set { SetValue(BorderIsVisibleProperty, value); }
			get { return (bool)GetValue(BorderIsVisibleProperty); }
		}

		//public static readonly BindableProperty FrameIsCenteredProperty =
		//	BindableProperty.Create(
		//		nameof(FrameIsCentered),
		//		typeof(bool),
		//		typeof(FramedPage),
		//		defaultValue: DefaultFrameIsCentered,
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is FramedPage page && newValue is bool isCentered) {
		//				if (isCentered) {
		//					page.Frame.WidthRequest = page.FrameSize.Width;
		//					page.Frame.HeightRequest = page.FrameSize.Height;
		//					page.Frame.HorizontalOptions = page.Frame.VerticalOptions = LayoutOptions.Center;
		//				} else {
		//					page.Frame.HorizontalOptions = page.Frame.VerticalOptions = LayoutOptions.FillAndExpand;
		//				}
		//			}
		//		});

		//public bool FrameIsCentered {
		//	set { SetValue(FrameIsCenteredProperty, value); }
		//	get { return (bool)GetValue(FrameIsCenteredProperty); }
		//}

		public static readonly BindableProperty FrameSizeProperty =
			BindableProperty.Create(
				nameof(FrameSize),
				typeof(Size),
				typeof(FramedPage),
				//defaultValue: DefaultFrameMargin,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FramedPage page && newValue is Size size) {
						if (page.DecorationLayout != DecorationLayout.FullScreen) {
							page.Frame.WidthRequest = size.Width;
							page.Frame.HeightRequest = size.Height;
						}
					}
				});

		public Size FrameSize {
			set { SetValue(FrameSizeProperty, value); }
			get { return (Size)GetValue(FrameSizeProperty); }
		}

	}
}
