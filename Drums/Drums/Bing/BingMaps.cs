using System;
using System.Threading.Tasks;


namespace Dwares.Drums.Bing
{
	public class BingMaps : MapApplication
	{
		public BingMaps() : base(nameof(BingMaps)) { }

		public override Uri GetAddressUri(string address)
		{
			var uri = String.Format("bingmaps:?rtp={0}", AdderssStr(address));
			return new Uri(uri);
		}

		public override Uri GetDirectionsUri(string from, string dest)
		{
			var uri = String.Format("bingmaps:?rtp={0}~{1}", AdderssStr(from), AdderssStr(dest));
			return new Uri(uri);

		}

		public override Uri GetDirectionsUri(ILocation from, ILocation dest, IRouteOptions options)
		{
			var uri = String.Format("bingmaps:?rtp={0}~{1}", LocationStr(from), LocationStr(dest));
			return new Uri(uri);
		}

		public static string LocationStr(ILocation loc)
		{
			if (loc != null) {
				if (loc.Coordinate?.IsValidCoordinate()==true) {
					return String.Format("pos.{0}_{1}", loc.Coordinate.Latitude, loc.Coordinate.Longitude);
				}
				if (!String.IsNullOrEmpty(loc.Address)) {
					return "adr." + Uri.EscapeDataString(loc.Address);
				}
			}
			return String.Empty;
		}

		public static string AdderssStr(string address)
		{
			if (String.IsNullOrEmpty(address))
				return "";
			return "adr." + Uri.EscapeDataString(address);
		}
	}
}
