//using System;
//using SkiaSharp;
//using Xamarin.Forms;


//namespace Dwares.Druid.Painting
//{
//	public interface IRenderable
//	{
//		void Prepare(SKCanvas canvas, SKSize canvasSize, object args);
//		void Release();
//		void Render();

//		SKBitmap ToBitmap(int width, int height);
//	}

//	public interface IRenderable<TArgs> : IRenderable
//	{
//		void Prepare(SKCanvas canvas, SKSize canvasSize, TArgs args);
//	}

//	public abstract class Renderable<TArgs> : IRenderable<TArgs> where TArgs: class
//	{
//		//public static Render(SKCanvas canvas, IRenderabl, TArgs args = null)
//		//{
//		//	try { 
//		//		Prepare(canvas, args as TArgs);
//		//		Render();
//		//	} finally {
//		//		Release(canvas);
//		//	}
//		//}

//		public virtual void Prepare(SKCanvas canvas, SKSize canvasSize, object args)
//		{
//			Prepare(canvas, canvasSize, args as TArgs);
//		}

//		public virtual void Prepare(SKCanvas canvas, SKSize canvasSize, TArgs args) { }
//		public virtual void Release() { }
//		public abstract void Render();

//		public virtual SKBitmap ToBitmap(int width, int height)
//		{
//			var bitmap = new SKBitmap(width, height);
//			using (var canvas = new SKCanvas(bitmap)) {
//				try {
//					Prepare(canvas, new SKSize(width, height), null);
//					Render();
//				} finally {
//					Release();
//				}

//			}
//			return bitmap;
//		}
//	}
//}
