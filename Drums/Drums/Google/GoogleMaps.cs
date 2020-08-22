using Dwares.Dwarf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dwares.Drums.Google
{
	public class GoogleMaps : MapApplication
	{
		const string SearchBaseUri = "http://www.google.com/maps/search/?api=1";
		const string DirectionsBaseUri = "http://www.google.com/maps/dir/?api=1";

		public GoogleMaps() : base(nameof(GoogleMaps)) { }

		public override Uri GetAddressUri(string address)
		{
			return GetSearchUri(address);
		}

		public Uri GetSearchUri(string query)
		{
			query = Escape(query);
			var uri = $"{SearchBaseUri}&query={query}";
			return new Uri(uri);
		}

		public override Uri GetDirectionsUri(string from, string dest)
		{
			Guard.ArgumentNotEmpty(dest, nameof(dest));

			var destination = Escape(dest);
			string uri;
			if (string.IsNullOrEmpty(from)) {
				uri = $"{DirectionsBaseUri}&destination={destination}&travelmode=driving";
			} else {
				var origin = Escape(from);
				uri = $"{DirectionsBaseUri}&origin={origin}&destination={destination}&travelmode=driving";
			}
			return new Uri(uri);
		}

		public override Uri GetDirectionsUri(ILocation from, ILocation dest, IRouteOptions options)
		{
			Guard.ArgumentNotNull(dest, nameof(dest));

			var destination = Escape(Location(dest));
			var travelmode = TravelMode(options);
			string uri;
			if (from == null) {
				uri = $"{DirectionsBaseUri}&destination={destination}&travelmode={travelmode}";
			} else {
				var origin = Escape(Location(from));
				uri = $"{DirectionsBaseUri}&origin={origin}&destination={destination}&travelmode={travelmode}";
			}
			return new Uri(uri);
		}

		public override Uri GetDirectionsUri(IList<ILocation> stops, IRouteOptions options)
		{
			if (stops == null || stops.Count < 2)
				throw new ArgumentException("Insufficient number of stops", nameof(stops));
			foreach (var stop in stops) {
				if (stop == null || (!stop.HasAddress && !stop.HasCoordinate))
					throw new ArgumentException("Invalid stop in the list", nameof(stops));
			}

			var origin = Escape(Location(stops[0]));
			var destination = Escape(Location(stops[stops.Count-1]));
			var travelmode = TravelMode(options);
			string uri;
			if (stops.Count == 2) {
				uri = $"{DirectionsBaseUri}&origin={origin}&destination={destination}&travelmode={travelmode}";
			} else {	
				var waypoints = string.Empty;
				for (int i = 1; i < stops.Count-1; i++) {
					var stop = stops[i];
					if (waypoints.Length > 0)
						waypoints += '|';
					waypoints += Location(stop);
				}
				waypoints = Escape(waypoints);

				uri = $"{DirectionsBaseUri}&origin={origin}&destination={destination}&waypoints={waypoints}&travelmode={travelmode}";
			}
			return new Uri(uri);
		}

		string Location(ILocation location)
		{
			if (location.HasCoordinate) {
				var coord = location.GetCoordinate();
				return $"{coord.Latitude},{coord.Longitude}";
			} else {
				return location.GetAddress();
			}
		}

		string TravelMode(IRouteOptions options)
		{
			switch (options?.TravelMode)
			{
			case Drums.TravelMode.Walking:
				return "walking";
			case Drums.TravelMode.Bicycle:
				return "bicycling";
			case Drums.TravelMode.Transit:
				return "transit";
			default:
				return "driving";
			}
		}

	}
}
