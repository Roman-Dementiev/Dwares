using System;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ProgressOverlay : BusyOverlay
	{
		const double DefaultProgressBarWidth = 80;
		const double DefaultProgressBarHeight = 2;

		public ProgressOverlay() : this(true) {}
		public ProgressOverlay(bool progressOutideFrame) :
			base(false)
		{
			ProgressBar = new ProgressBar {
				Margin = new Thickness(0, 0),
				HorizontalOptions = DefaultHorizontalOptions,
				VerticalOptions = DefaultVerticalOptions,
				WidthRequest = DefaultProgressBarWidth,
				HeightRequest = DefaultProgressBarHeight
			};

			if (progressOutideFrame) {
				MessageFrame.VerticalOptions = LayoutOptions.Start;

				Content = new StackLayout {
					HorizontalOptions = DefaultHorizontalOptions,
					VerticalOptions = DefaultVerticalOptions,
					Margin = DefaultContentMargin,
					Children = {
						ProgressBar,
						MessageFrame
					}
				};
			}
			else {
				Content = MessageFrame;
				Content.Margin = DefaultContentMargin;
			}
		}

		public ProgressBar ProgressBar { get; }


		public static readonly BindableProperty ProgressProperty =
			BindableProperty.Create(
				nameof(Progress),
				typeof(double),
				typeof(ProgressOverlay),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ProgressOverlay overlay && newValue is double progress) {
						overlay.ProgressBar.Progress = progress;
					}
				});

		public double Progress {
			set { SetValue(ProgressProperty, value); }
			get { return (double)GetValue(ProgressProperty); }
		}

		public static readonly BindableProperty ProgressBarWidthProperty =
			BindableProperty.Create(
				nameof(ProgressBarWidth),
				typeof(double),
				typeof(ProgressOverlay),
				defaultValue: DefaultProgressBarWidth,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ProgressOverlay overlay && newValue is double width) {
						overlay.ProgressBar.WidthRequest = width;
					}
				});

		public double ProgressBarWidth {
			set { SetValue(ProgressBarWidthProperty, value); }
			get { return (double)GetValue(ProgressBarWidthProperty); }
		}

		public static readonly BindableProperty ProgressBarHeightProperty =
			BindableProperty.Create(
				nameof(ProgressBarHeight),
				typeof(double),
				typeof(ProgressOverlay),
				defaultValue: DefaultProgressBarHeight,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ProgressOverlay overlay && newValue is double height) {
						overlay.ProgressBar.HeightRequest = height;
					}
				});

		public double ProgressBarHeight {
			set { SetValue(ProgressBarHeightProperty, value); }
			get { return (double)GetValue(ProgressBarHeightProperty); }
		}

		public static readonly BindableProperty ProgressColorProperty =
			BindableProperty.Create(
				nameof(ProgressColor),
				typeof(Color),
				typeof(ProgressOverlay),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ProgressOverlay overlay && newValue is Color color) {
						overlay.ProgressBar.ProgressColor = color;
					}
				});

		public Color ProgressColor {
			set { SetValue(ProgressColorProperty, value); }
			get { return (Color)GetValue(ProgressColorProperty); }
		}

	}
}
