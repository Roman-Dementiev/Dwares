using System;


namespace RouteOptimizer.Models
{
	public static class Ids
	{
		public static string PlaceId(Place place)
		{
			if (place != null) {
				return PlaceId(place.Name, place.Address);
			} else {
				return string.Empty;
			}
		}

		public static string PlaceId(string name, string address)
		{
			if (!string.IsNullOrEmpty(name)) {
				return NameToId(name);
			} else if (!string.IsNullOrEmpty(address)) {
				return AddrToId(address);
			} else {
				return string.Empty;
			}
		}

		public static string NameToId(string name) => $"[{name}]";
		public static string AddrToId(string addr) => $"({addr})";
	}
}
