using System;


namespace Dwares.Drums.Google
{
	public class GoogleMaps : MapApplication
	{
		public Uri GetSearchUri(string query)
		{
			var uri = String.Format("http://www.google.com/maps/search/?api=1&query={0}", Escape(query));
			return new Uri(uri);
		}

		public override Uri GetAddressUri(string address)
		{
			return GetSearchUri(address);
		}

		public override Uri GetDirectionsUri(string from, string dest)
		{
			// TODO
			// return new Uri("http://maps.google.com/");
			return GetSearchUri(dest);
		}
	}
}
