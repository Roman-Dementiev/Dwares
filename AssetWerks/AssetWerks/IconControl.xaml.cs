using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SkiaSharp.Views.UWP;
using SkiaSharp;
using System.Threading.Tasks;
using Windows.UI;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AssetWerks
{
	public sealed partial class IconControl : UserControl
	{
		public IconControl()
		{
			FrameColor = Colors.Black;
			this.InitializeComponent();

			RegisterPropertyChangedCallback(ImageProperty, (sender, dp) => OnIconChanged());
			RegisterPropertyChangedCallback(FrameColorProperty, (sender, dp) => OnFrameChanged());
			OnIconChanged();
			OnFrameChanged();
		}

		public static readonly DependencyProperty ImageProperty =
			DependencyProperty.Register(
			nameof(Image),
			typeof(SKImage),
			typeof(IconControl),
			typeMetadata: null
			);

		public SKImage Image {
			get { return (SKImage)GetValue(ImageProperty); }
			set { SetValue(ImageProperty, value); }
		}

		public static readonly DependencyProperty FrameColorProperty =
			DependencyProperty.Register(
			nameof(FrameColor),
			typeof(Color),
			typeof(IconControl),
			typeMetadata: PropertyMetadata.Create(Colors.Black)
			);

		public Color FrameColor {
			get { return (Color)GetValue(FrameColorProperty); }
			set { SetValue(FrameColorProperty, value); }
		}

		void OnIconChanged()
		{
			canvas.Image = Image;
			canvas.Invalidate();
		}

		void OnFrameChanged()
		{
			frame.BorderBrush = new SolidColorBrush(FrameColor);
			canvas.CrossColor = new SKColor(FrameColor.R, FrameColor.G, FrameColor.B, FrameColor.A);
			if (canvas.Image == null) {
				canvas.Invalidate();
			}
		}
	}

	internal class IconCanvas : SKXamlCanvas
	{
		public SKImage Image { get; set; }
		public SKColor CrossColor { get; set; } = SKColors.Black;

		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			//base.OnPaintSurface(e);

			var canvas = e.Surface.Canvas;
			canvas.Clear();
			
			if (Image != null) {
				canvas.DrawImage(Image, new SKRect(0, 0, CanvasSize.Width, CanvasSize.Height));
			}
			else {
				using (var paint = new SKPaint() { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = CrossColor }) {
					canvas.DrawLine(0, 0, CanvasSize.Width, CanvasSize.Height, paint);
					canvas.DrawLine(0, CanvasSize.Height, CanvasSize.Width, 0, paint);
				}
			}
		}
	}
}
