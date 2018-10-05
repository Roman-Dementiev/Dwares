using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Dwares.Dwarf;


namespace Dwares.Druid.Painting
{	
	public class SpriteSheet: IDisposable
	{
		//public SpriteSheet(PackageUnit package, string resourceName, int numCols, int numRows = 1) :
		//	this(Bitmaps.LoadBitmap(package, resourceName), numCols, numRows)
		//{
		//}

		public SpriteSheet(ResourceId id, int numCols, int numRows = 1) :
			this(Bitmaps.LoadBitmap(id), numCols, numRows)
		{
		}

		public SpriteSheet(SKBitmap bitmap, int numCols, int numRows=1)
		{
			Debug.Assert(bitmap != null && numRows > 0 && numCols > 0);
			Bitmap = bitmap;
			NumCols = numCols;
			NumRows = numRows;
			SpriteWidth = bitmap.Width / numCols;
			SpriteHeight = bitmap.Height / numRows;
		}

		public void Dispose()
		{
			Bitmap?.Dispose();
		}

		public SKBitmap Bitmap { get; }
		public int NumCols { get; }
		public int NumRows { get; }
		public int SpriteWidth { get; }
		public int SpriteHeight { get; }

		public void DrawSprite(SKCanvas canvas, SKRect dest, int col, int row=0, SKPaint paint=null)
		{
			Debug.Assert(col >= 0 && col < NumCols && row >= 0 && row < NumRows);
			int top = row * SpriteHeight;
			int left = col * SpriteWidth;
			int bottom = top + SpriteHeight;
			int right = left + SpriteWidth;
			var src = new SKRect(left, top, right, bottom);
			canvas.DrawBitmap(Bitmap, src, dest);
		}

		public SpriteSheetSprite Sprite(int col, int row = 0)
		{
			return new SpriteSheetSprite(this, col, row);
		}

	}

	public struct SpriteSheetSprite : ISprite
	{
		public SpriteSheetSprite(SpriteSheet spriteSheet, int col = 0, int row = 0)
		{
			SpriteSheet = spriteSheet;
			Column = col;
			Row = row;
		}

		public SpriteSheet SpriteSheet { get; set; }
		public int Column { get; set; }
		public int Row { get; set; }

		public void Draw(SKCanvas canvas, SKRect dest, SKPaint paint = null)
		{
			SpriteSheet.DrawSprite(canvas, dest, Column, Row, paint);
		}
	}

}
