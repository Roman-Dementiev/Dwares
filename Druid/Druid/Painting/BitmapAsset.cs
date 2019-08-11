using System;
using Dwares.Dwarf;
using SkiaSharp;
using Xamarin.Forms;

namespace Dwares.Druid.Painting
{
	public class BitmapAsset : BitmapBase
	{
		//static ClassRef @class = new ClassRef(typeof(BitmapAsset));

		WeakReference<SKBitmap> weakBitmap = new WeakReference<SKBitmap>(null);

		double resolution;
		Color? color;

		public BitmapAsset(ResourceId resourceId) :
			this(resourceId, 0, 0, 1, null)
		{
		}

		public BitmapAsset(ResourceId resourceId, int bitmapWidth, int bitmapHeight, double resolution, Color? color = null)
		{
			//Debug.EnableTracing(@class);

			ResourceId = resourceId;

			this.resolution = resolution;
			this.color = (color == default(Color)) ? null : color;

			if (bitmapWidth > 0 && bitmapHeight > 0) {
				Info = new BitmapInfo(bitmapWidth, bitmapHeight, resolution, color);
			}
		}

		public ResourceId ResourceId { get; }

		public override SKBitmap SKBitmap {
			get {
				SKBitmap bitmap;
				if (!weakBitmap.TryGetTarget(out bitmap)) {
					bitmap = LoadResource(ResourceId);
				}
				return bitmap;
			}
		}	

		protected override PictureInfo AcquireInfo()
		{
			return AcquireInfo(SKBitmap, resolution, color);
		}

	}
}
