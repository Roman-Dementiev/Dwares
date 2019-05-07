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
		protected ShapeBadge(string name) : base(name) { }
		protected ShapeBadge(string name, float iconInset) : base(name, iconInset) { }
		protected ShapeBadge(string name, SKRect iconInset) : base(name, iconInset) { }

		public SKColor Color { get; set; } = SKColors.Black;
	}

	public class CircleBadge : ShapeBadge
	{
		public CircleBadge() : base("Circle",0.1f) { }

		public override void Draw(SKCanvas canvas, SKRect rect, SKColor? color)
		{
			var x = (rect.Left+rect.Right) / 2;
			var y = (rect.Top+rect.Bottom) / 2;
			var r = Math.Min(rect.Width, rect.Height) / 2;
	
			using (var paint = Skia.FillPaint(color ?? Color)) {
				canvas.DrawCircle(x, y, r, paint);
			}
		}
	}
}
