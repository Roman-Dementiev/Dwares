//using System;
//using Dwares.Dwarf;
//using Xamarin.Forms;
//using SkiaSharp;


//namespace Dwares.Druid.Painting
//{
//	public class Icon
//	{
//		//static ClassRef @class = new ClassRef(typeof(Icon));
//		SKBitmap bitmap;
//		WeakBitmap weakBitmap;

//		public Icon(ResourceId resourceId, int width = 0, int height = 0, Color? color = null)
//		{
//			//Debug.EnableTracing(@class);
//			Guard.ArgumentNotNull(resourceId, nameof(resourceId));

//			weakBitmap = new WeakBitmap(resourceId);
//			Width = width;
//			Height = height;
//			Color = color ?? DefaultCoilor;
//		}

//		public Icon(SKBitmap bitmap, Color? color = null)
//		{
//			//Debug.EnableTracing(@class);
//			Guard.ArgumentNotNull(bitmap, nameof(bitmap));

//			this.bitmap = bitmap;
//			Width = bitmap.Width;
//			Height = bitmap.Height;
//			Color = color ?? DefaultCoilor;
//		}

//		int width;
//		public int Width {
//			get {
//				if (width <= 0 && weakBitmap != null) {
//					LoadWeakBitmap();
//				}
//				return width;
//			}
//			protected set {
//				width = value;
//			}
//		}

//		int height;
//		public int Height {
//			get {
//				if (height <= 0 && weakBitmap != null) {
//					LoadWeakBitmap();
//				}
//				return height;
//			}
//			protected set {
//				height = value;
//			}
//		}
//		public Color Color { get; protected set; }

//		public SKBitmap Bitmap { 
//			get {
//				if (bitmap != null) {
//					return bitmap;
//				} else if (weakBitmap != null) {
//					return LoadWeakBitmap();
//				} else {
//					return null;
//				}
//			}
//		}

//		SKBitmap LoadWeakBitmap()
//		{
//			var bitmap = weakBitmap.Get();
//			Width = bitmap.Width;
//			Height = bitmap.Height;
//			return bitmap;
//		}

//		public static Color DefaultCoilor { get; } = Color.Black;
//	}
//}
