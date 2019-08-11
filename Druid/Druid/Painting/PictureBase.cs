using System;
using Dwares.Dwarf;
using SkiaSharp;
using Xamarin.Forms;


namespace Dwares.Druid.Painting
{
	public abstract class Picture : IPicture
	{
		//static ClassRef @class = new ClassRef(typeof(APainting));

		protected bool forceAcquireInfo = false;

		protected Picture()
		{
			//Debug.EnableTracing(@class);
		}

		//protected Picture(PictureInfo info)
		//{
		//	//Debug.EnableTracing(@class);
		//	Info = info;
		//}

		protected PictureInfo info;
		public PictureInfo Info {
			get {
				if (info == null || forceAcquireInfo) {
					info = AcquireInfo();
					Debug.Assert(info != null);
					forceAcquireInfo = false;
				}
				return info;
			}
			protected set {
				info = value;
			}
		}

		protected abstract PictureInfo AcquireInfo();

		public double DefaultWidth {
			get => Info.Width;
		}

		public double DefaultHeight {
			get => Info.Height;
		}

		public Size? DefaultSize {
			get {
				var info = Info;
				if (info.Width > 0 && info.Height > 0) {
					return new Size(info.Width, info.Height);
				} else {
					return null;
				}
			}
		}

		public Color? DefaultColor {
			get => Info.Color;
		}

		public abstract void Render(SKCanvas canvas, SKRect rect);

		public virtual SKBitmap ToBitmap()
		{
			return ToBitmap((int)DefaultWidth, (int)DefaultHeight);
		}

		public virtual SKBitmap ToBitmap(int width, int height)
		{
			var bitmap = new SKBitmap(width, height);
			using (var canvas = new SKCanvas(bitmap)) {
				Render(canvas, new SKRect(0, 0, width, height));
			}
			return bitmap;
		}
	}

}
