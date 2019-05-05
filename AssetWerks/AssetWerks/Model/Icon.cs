using System;
using SkiaSharp;


namespace AssetWerks.Model
{
	public class Icon : TitleHolder
	{
		SKImage image;
		public SKImage Image {
			get => image;
			set => SetProperty(ref image, value);
		}

		public int ImageWidth { get; set; }
		public int ImageHeight { get; set; }

		public int IconWidth { get; set; }
		public int IconHeight { get; set; }

		public int CellWidth { get; set; }
		public int CellHeight { get; set; }

		public SKRect BadgeRect { get; set; }
	}
}
