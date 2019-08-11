using System;
using System.Drawing;
using Dwares.Dwarf;
using SkiaSharp;


namespace Dwares.Druid.Painting
{

	public class Bitmap : BitmapBase
	{
		//static ClassRef @class = new ClassRef(typeof(Bitmap));

		SKBitmap skBitmap;

		protected Bitmap() { }

		public Bitmap(SKBitmap bitmap)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(bitmap, nameof(bitmap));

			skBitmap = bitmap;
			Info = AcquireInfo(bitmap);
		}

		public Bitmap(ResourceId resourceId)
		{
			skBitmap = LoadResource(resourceId);
			Info = AcquireInfo(skBitmap);
		}

		public override SKBitmap SKBitmap {
			get => skBitmap;
			//set {
			//	skBitmap = value;
			//	Info = AcquireInfo(value);
			//}
		}

	}


}
