using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace Dwares.Drums
{
	public abstract class MapApplication : IMapApplication
	{
		public MapApplication(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public static string Escape(string str)
		{
			if (String.IsNullOrEmpty(str))
				return "";
			return Uri.EscapeDataString(str);
		}

		public abstract Uri GetAddressUri(string address);
		public abstract Uri GetDirectionsUri(string from, string dest);

		public abstract Uri GetDirectionsUri(ILocation from, ILocation dest, IRouteOptions options);
		public abstract Uri GetDirectionsUri(IList<ILocation> stops, IRouteOptions options);

		public virtual Task OpenMapUri(Uri uri)
		{
			return Launcher.OpenAsync(uri);
		}

		public Task OpenAddress(string address)
		{
			var uri = GetAddressUri(address);
			return OpenMapUri(uri);

		}

		public Task OpenDirections(string from, string dest)
		{
			var uri = GetDirectionsUri(from, dest);
			return OpenMapUri(uri);
		}

		public Task OpenDirections(IList<ILocation> stops, IRouteOptions options)
		{
			var uri = GetDirectionsUri(stops, options);
			return OpenMapUri(uri);
		}

		public Task OpenDirections(ILocation from, ILocation dest, IRouteOptions options)
		{
			var uri = GetDirectionsUri(from, dest, options);
			return OpenMapUri(uri);
		}

	}
}
