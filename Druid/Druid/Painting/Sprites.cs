using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Dwares.Dwarf;


namespace Dwares.Druid.Painting
{
	public interface ISprite
	{
		void Draw(SKCanvas canvas, SKRect dest, SKPaint paint);
	}


	public class BitmapSprite : ISprite
	{
		public BitmapSprite(ResourceId resourceId)
		{
			Bitmap = Bitmaps.LoadBitmap(resourceId);
		}

		public BitmapSprite(SKBitmap bitmap)
		{
			Bitmap = bitmap;
		}

		public SKBitmap Bitmap { get; set; }


		public void Draw(SKCanvas canvas, SKRect dest, SKPaint paint = null)
		{
			canvas.DrawBitmap(Bitmap, dest, paint);
		}
	}


	public class ImageSprite : ISprite
	{
		public ImageSprite(ResourceId resourceId)
		{
			var bitmap = Bitmaps.LoadBitmap(resourceId);
			Image = SKImage.FromBitmap(bitmap);
		}

		public ImageSprite(SKBitmap bitmap)
		{
			Image = SKImage.FromBitmap(bitmap);
		}

		public ImageSprite(SKImage image)
		{
			Image = image;
		}

		public SKImage Image { get; set; }

		public void Draw(SKCanvas canvas, SKRect dest, SKPaint paint = null)
		{
			canvas.DrawImage(Image, dest, paint);
		}
	}

	public static class Sprites
	{
		public static void DrawSprite(this SKCanvas canvas, ISprite sprite, SKRect dest, SKPaint paint = null)
		{
			Debug.AssertNotNull(canvas);
			Debug.AssertNotNull(sprite);

			sprite.Draw(canvas, dest, paint);
		}
	}
}
