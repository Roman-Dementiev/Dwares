using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.Satchel
{
	public static class Colors
	{
		public static Color FromHsva(double h, double s, double v, double a = 1)
		{
			var hh = h;
			var ll = (2 - s) * v;
			var ss = s * v;
			ss /= (ll <= 1) ? ll : 2 - ll;
			ll /= 2;

			return Color.FromHsla(hh, ss, ll, a);
		}

		public static void ToHsva(this Color color, out double h, out double s, out double v, out double a)
		{
			h = color.Hue;
			a = color.A;

			//var ss = color.Saturation;
			//var ll = color.Luminosity * 2;
			//ss *= (ll <= 1) ? ll : 2 - ll;
			//v = (ll + ss) / 2;
			//s = (2 * ss) / (ll + ss);

			var ss = color.Saturation;
			var ll = color.Luminosity;
			ss *= (ll <= 0.5) ? ll : 1 - ll;
			v = ll + ss;
			s = (2 * ss) / v;
		}

		public static Color GetShade(this Color color, double shade)
		{
			Debug.Assert(shade >= 0 && shade <= 1);

			double h, s, v, a;
			color.ToHsva(out h, out s, out v, out a);

			return FromHsva(h, s, v * shade, a);
		}
	}
}
