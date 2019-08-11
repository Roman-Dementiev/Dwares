using System;
using Dwares.Dwarf;
using Xamarin.Forms;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public abstract class BitmapBase : Picture, IBitmap
	{
		//static ClassRef @class = new ClassRef(typeof(BitmapBase));

		protected BitmapBase()
		{
			//Debug.EnableTracing(@class);
		}

		public abstract SKBitmap SKBitmap { get; }

		public static implicit operator SKBitmap(BitmapBase bitmap) => bitmap.SKBitmap;

		public new BitmapInfo Info {
			get => base.Info as BitmapInfo;
			set => base.Info = value;
		}

		protected override PictureInfo AcquireInfo()
		{
			return AcquireInfo(SKBitmap);
		}

		protected BitmapInfo AcquireInfo(SKBitmap bitmap, double resolution = 0, Color? color = null)
		{
			if (bitmap != null) {
				return new BitmapInfo(bitmap.Width, bitmap.Height, resolution, color);
			} else {
				return new BitmapInfo(0, 0, resolution, color);
			}
		}

		public override void Render(SKCanvas canvas, SKRect rect)
		{
			var bitmap = SKBitmap;
			if (bitmap != null) {
				canvas.DrawBitmap(bitmap, rect);
			}
		}

		public override SKBitmap ToBitmap(int width, int height)
		{
			var bitmap = SKBitmap;
			if (bitmap == null || (width == bitmap.Width && height == bitmap.Height)) {
				return bitmap;
			} else {
				return base.ToBitmap(width, height);
			}
		}


		//IBitmap Recolor(Func<Color, Color> func)
		//{
		//	var source = SKBitmap;
		//	if (source == null)
		//		return this;

		//	int width = source.Width;
		//	int height = source.Height;
		//	var result = new SKBitmap(width, height);

		//	for (int row = 0; row < height; row++) {
		//		for (int col = 0; col < width; col++) {
		//			var pixel = source.GetPixel(col, row);
		//			var color = func(pixel.ToXamColor());

		//			result.SetPixel(col, row, color.ToSKColor());
		//		}
		//	}

		//	return new Bitmap(result);
		//}

		public IBitmap Recolor(Func<SKColor, SKColor> func)
		{
			var source = SKBitmap;
			if (source == null)
				return this;

			int width = source.Width;
			int height = source.Height;
			var result = new SKBitmap(width, height);

			for (int row = 0; row < height; row++) {
				for (int col = 0; col < width; col++) {
					var pixel = source.GetPixel(col, row);
					result.SetPixel(col, row, func(pixel));
				}
			}

			return new Bitmap(result);
		}

		public IBitmap Recolor(Color srcColor, Color newColor, bool keepAlpha = true)
		{
			var source = SKBitmap;
			if (source == null || newColor == srcColor)
				return this;

			var _srcColor = srcColor.ToSKColor();
			var _newColor = newColor.ToSKColor();

			if (keepAlpha) {
				return Recolor((color) => {
					if (color.Red == _srcColor.Red && color.Green == _srcColor.Green && color.Blue == _srcColor.Blue) {
						return new SKColor(_newColor.Red, _newColor.Green, _newColor.Blue, color.Alpha);
					} else {
						return color;
					}
				});
			} else {
				return Recolor((color) => (color == _srcColor) ? _newColor : color);
			}
		}

		public IBitmap Resize(int newWidth, int newHeight)
		{
			var source = SKBitmap;
			if (source == null || (newWidth == source.Width && newHeight == source.Height))
				return this;

			var result = new SKBitmap(newWidth, newHeight);
			using (var canvas = new SKCanvas(result)) {
				canvas.DrawBitmap(source, new SKRect(0, 0, newWidth, newHeight));
			}
			return new Bitmap(result);
		}

		//public IBitmap Resize(SKSize newSize)
		//{
		//	int newWidth = (int)Math.Round(newSize.Width);
		//	int newHeight = (int)Math.Round(newSize.Height);
		//	return Resize(newWidth, newHeight);
		//}


		public static SKBitmap LoadResource(ResourceId resourceId, bool throwIfError = true)
		{
			try {
				using (var stream = resourceId.GetStream()) {
					return SKBitmap.Decode(stream);
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				if (throwIfError)
					throw;
			}
			return null;
		}
	}
}
