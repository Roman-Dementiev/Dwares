using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;


namespace AssetWerks.Model
{
	public class ImageBadge : Badge
	{
		public ImageBadge() : base("Image") { }

		public SKImage Image { get; set; }

		// TODO
		public override void Draw(SKCanvas canvas, SKRect rect, SKColor? color)
		{
			if (Image != null) {
				canvas.DrawImage(Image, rect);
			}
		}
	}
}
