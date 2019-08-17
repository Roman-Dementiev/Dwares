using System;
using Dwares.Dwarf;
using Dwares.Druid.Painting;
using SkiaSharp;
using Xamarin.Forms;

namespace Dwares.Druid.Xaml
{
	[ContentProperty(nameof(ResourceId))]
	public class BitmapAsset : BitmapBase, IAsset<SKBitmap>
	{
		//static ClassRef @class = new ClassRef(typeof(BitmapAsset));

		public WeakReference<SKBitmap> @ref;

		public BitmapAsset()
		{
			//Debug.EnableTracing(@class);

			@ref = new WeakReference<SKBitmap>(null);
		}

		//public BitmapAsset(string resourceId, int bitmapWidth, int bitmapHeight, double resolution, Color color = default) :
		//	this()
		//{

		//	ResourceId = resourceId;
		//	BitmapWidth = bitmapWidth;
		//	BitmapHeight = bitmapHeight;
		//	Resolution = resolution;
		//	Color = color;

		//	//if (bitmapWidth > 0 && bitmapHeight > 0) {
		//	//	Info = new BitmapInfo(bitmapWidth, bitmapHeight, resolution, color);
		//	//}
		//}

		public string ResourceId { get; set; }
		public int BitmapWidth { get; set; }
		public int BitmapHeight { get; set; }
		public double Resolution { get; set; }
		public Color Color { get; set; }


		[TypeConverter(typeof(SizeTypeConverter))]
		public Size BitmapSize {
			get => new Size(BitmapWidth, BitmapHeight);
			set {
				BitmapWidth = (int)value.Width;
				BitmapHeight = (int)value.Height;
			}
		}

		SKBitmap IAsset<SKBitmap>.AssetValue => SKBitmap;

		public override SKBitmap SKBitmap {
			get {
				SKBitmap bitmap;
				if (!@ref.TryGetTarget(out bitmap)) {
					var resourceId = AssetLocator.GetResourceId(ResourceId);
					bitmap = LoadResource(resourceId);
					@ref.SetTarget(bitmap);
				}
				return bitmap;
			}
		}

		protected override PictureInfo AcquireInfo()
		{
			if (BitmapWidth > 0 && BitmapHeight > 0 && !forceAcquireInfo) {
				return new BitmapInfo(BitmapWidth, BitmapHeight, Resolution, Color);
			} else {
				return AcquireInfo(SKBitmap, Resolution, Color);
			}
		}
	}
}
