using System;
using System.Threading.Tasks;


namespace Dwares.Drums.Bing
{
	public class BingMaps : MapApplication
	{
		public override Uri GetAddressUri(string address)
		{
			var uri = String.Format("bingmaps:?rtp={0}", Adderss(address));
			return new Uri(uri);
		}

		public override Uri GetDirectionsUri(string from, string dest)
		{
			var uri = String.Format("bingmaps:?rtp={0}~{1}", Adderss(from), Adderss(dest));
			return new Uri(uri);

		}

		public static string Adderss(string address)
		{
			if (String.IsNullOrEmpty(address))
				return "";
			return "adr." + Uri.EscapeDataString(address);
		}
	}
}
