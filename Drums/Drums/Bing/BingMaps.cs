using System;
using System.Collections.Generic;
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
			var uri = string.Format("bingmaps:?rtp={0}~{1}", LocationStr(from), LocationStr(dest));
			return new Uri(uri);
		}

		public override Uri GetDirectionsUri(IList<ILocation> stops, IRouteOptions options)
			=> throw new NotImplementedException();


		public static string LocationStr(ILocation loc)
		{
			if (loc != null) {
				if (loc.HasCoordinate) {
					var coord = loc.GetCoordinate();
					if (coord?.IsValidCoordinate()==true) {
						return string.Format("pos.{0}_{1}", coord.Latitude, coord.Longitude);
					}
				}
				if (loc.HasAddress) {
					return AdderssStr(loc.GetAddress());
				}
			}
			return string.Empty;
		}

		public static string AdderssStr(string address)
		{
			if (string.IsNullOrEmpty(address))
				return string.Empty;
			return "adr." + Uri.EscapeDataString(address);
		}
	}
}
