using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DwMatter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
		SKBitmap source;
		SKBitmap result;
		SKColor sourceColor;
		SKColor resultColor;

		public TestPage()
		{
			InitializeComponent();

			//var stream = new FileStream("test.png", FileMode.Open);


			sourceColor = SKColors.Black;
			resultColor = SKColors.Red;

			source = LoadBitmapResource("test.png");
			result = Recolor(source, sourceColor, resultColor, true);

			sourceImage.Source = new SKBitmapImageSource { Bitmap = source };
			resultImage.Source = new SKBitmapImageSource { Bitmap = result };
		}

		private void SourceView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			PaintImage(e.Surface.Canvas, source, sourceColor);
		}

		private void ResultView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			PaintImage(e.Surface.Canvas, result, resultColor);
		}

		void PaintImage(SKCanvas canvas, SKBitmap bitmap, SKColor color)
		{
			var bounds = canvas.LocalClipBounds;
		
			canvas.DrawRect(bounds, new SKPaint { 
				Style = SKPaintStyle.Stroke, 
				StrokeWidth = 8, 
				Color = color });

			if (bitmap != null) {
				canvas.DrawBitmap(bitmap, bounds);
			}
		}

		public static SKBitmap LoadBitmapResource(Type type, string resourceID)
		{
			var assembly = type.GetTypeInfo().Assembly;

			using (var stream = assembly.GetManifestResourceStream(resourceID)) {
				return SKBitmap.Decode(stream);
			}
		}

		public static SKBitmap LoadBitmapResource(string resourceID)
		{
			return LoadBitmapResource(typeof(TestPage), "DwMatter."+resourceID);
		}


		public static SKBitmap Recolor(SKBitmap source, Func<SKColor, SKColor> func)
		{
			int width = source.Width;
			int height = source.Height;
			SKBitmap result = new SKBitmap(width, height);

			for (int row = 0; row < height; row++) {
				for (int col = 0; col < width; col++) {
					var color = func(source.GetPixel(col, row));

					result.SetPixel(col, row, color);
				}
			}

			return result;
		}

		public static SKBitmap Recolor(SKBitmap source, SKColor srcColor, SKColor newColor, bool keepAlpha)
		{
			if (keepAlpha) {
				return Recolor(source, (color) => {
					if (color.Red == srcColor.Red && color.Green == srcColor.Green && color.Blue == srcColor.Blue) {
						return new SKColor(newColor.Red, newColor.Green, newColor.Blue, color.Alpha);
					} else {
						return color;
					}
				});
			} else {
				return Recolor(source, (color) => (color == srcColor) ? newColor : color);
			}
		}

		//public static SKBitmap Recolor(SKBitmap source, SKColor srcColor, SKColor newColor)
		//{
		//	int width = source.Width;
		//	int height = source.Height;
		//	SKBitmap result = new SKBitmap(width, height);

		//	for (int row = 0; row < height; row++) {
		//		for (int col = 0; col < width; col++) {
		//			var color = source.GetPixel(col, row);
		//			if (color.Red == srcColor.Red && color.Green == srcColor.Green && color.Blue == srcColor.Blue)
		//				color = new SKColor(newColor.Red, newColor.Green, newColor.Blue, color.Alpha);

		//			result.SetPixel(col, row, color);
		//		}
		//	}

		//	return result;
		//}

	}
}
