using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace AssetWerks.Model
{
	public abstract class ShapeBadge : Badge
	{
		public ShapeBadge(string name) : base(name) { }

		public SKColor Color { get; set; } = SKColors.Black;
	}

	public class CircleBadge : ShapeBadge
	{
		public CircleBadge() : base("Circle") { }

		public override void Draw(SKCanvas canvas, SKRect rect)
		{
			var x = (rect.Left+rect.Right) / 2;
			var y = (rect.Top+rect.Bottom) / 2;
			var r = Math.Min(rect.Width, rect.Height) / 2;
	
			var paint = new SKPaint {
				Style = SKPaintStyle.Fill,
				Color = this.Color
			};
			canvas.DrawCircle(x, y, r, paint);
		}
	}
}
