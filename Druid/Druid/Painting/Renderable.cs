using System;
using SkiaSharp;

namespace Dwares.Druid.Painting
{
	public interface IRenderable
	{
		void Prepare(SKCanvas canvas, SKSize canvasSize, object args);
		void Release();
		void Render();
	}

	public interface IRenderable<TArgs> : IRenderable
	{
		void Prepare(SKCanvas canvas, SKSize canvasSize, TArgs args);
	}

	public abstract class Renderable<TArgs> : IRenderable<TArgs> where TArgs: class
	{
		//public static Render(SKCanvas canvas, IRenderabl, TArgs args = null)
		//{
		//	try { 
		//		Prepare(canvas, args as TArgs);
		//		Render();
		//	} finally {
		//		Release(canvas);
		//	}
		//}

		public virtual void Prepare(SKCanvas canvas, SKSize canvasSize, object args)
		{
			Prepare(canvas, canvasSize, args as TArgs);
		}

		public virtual void Prepare(SKCanvas canvas, SKSize canvasSize, TArgs args) { }
		public virtual void Release() { }
		public abstract void Render();
	}
}
