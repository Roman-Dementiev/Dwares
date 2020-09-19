using System;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class SpinningOverlay : BusyOverlay
	{
		public const double DefaultIndicatorSize = 32;
		public static Color DefaultIndicatorColor => Color.Black;

		public SpinningOverlay() : 
			base(false)
		{
			MessageFrame.VerticalOptions = LayoutOptions.Start;

			ActivityIndicator = new ActivityIndicator {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
				Color = DefaultIndicatorColor,
				WidthRequest = DefaultIndicatorSize,
				HeightRequest = DefaultIndicatorSize
			};

			Content = new StackLayout {
				HorizontalOptions = DefaultHorizontalOptions,
				VerticalOptions = DefaultVerticalOptions,
				Margin = DefaultContentMargin,
				Children = {
					ActivityIndicator,	
					MessageFrame
				}
			};

			this.PropertyChanged += (s, e) => {
				if (e.PropertyName == nameof(IsVisible)) {
					ActivityIndicator.IsRunning = IsVisible;
				}
			};
		}

		public ActivityIndicator ActivityIndicator { get; }

		public static readonly BindableProperty IndicatorSizeProperty =
			BindableProperty.Create(
				nameof(IndicatorSize),
				typeof(double),
				typeof(SpinningOverlay),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SpinningOverlay overlay && newValue is double size) {
						overlay.ActivityIndicator.WidthRequest = size;
						overlay.ActivityIndicator.HeightRequest = size;
					}
				});

		public double IndicatorSize {
			set { SetValue(IndicatorSizeProperty, value); }
			get { return (double)GetValue(IndicatorSizeProperty); }
		}

		public static readonly BindableProperty IndicatorColorProperty =
			BindableProperty.Create(
				nameof(IndicatorColor),
				typeof(Color),
				typeof(SpinningOverlay),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SpinningOverlay overlay && newValue is Color color) {
						overlay.ActivityIndicator.Color = color;
					}
				});

		public Color IndicatorColor {
			set { SetValue(IndicatorColorProperty, value); }
			get { return (Color)GetValue(IndicatorColorProperty); }
		}
	}
}
