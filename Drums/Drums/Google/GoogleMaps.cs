using System;


namespace Dwares.Drums.Google
{
	public class GoogleMaps : MapApplication
	{
		public GoogleMaps() : base(nameof(GoogleMaps)) { }

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
			return GetSearchUri(dest);
		}

		public override Uri GetDirectionsUri(ILocation from, ILocation dest, IRouteOptions options)
		{
			// TODO
			return GetSearchUri(dest.Address);
		}
	}
}
