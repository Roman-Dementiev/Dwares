using System;
using System.Threading.Tasks;
using Dwares.Druid.Services;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Drums
{
	public abstract class MapApplication : NameHolder, IMapApplication
	{
		public MapApplication(string name) : base(name) { }

		public static string Escape(string str)
		{
			if (String.IsNullOrEmpty(str))
				return "";
			return Uri.EscapeDataString(str);
		}

		public abstract Uri GetAddressUri(string address);
		public abstract Uri GetDirectionsUri(string from, string dest);

		public abstract Uri GetDirectionsUri(ILocation from, ILocation dest, IRouteOptions options);

		public virtual Task OpenMapUri(Uri uri)
		{
			return Launcher.OpenUri(uri);
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

		public Task OpenDirections(ILocation from, ILocation dest, IRouteOptions options)
		{
			var uri = GetDirectionsUri(from, dest, options);
			return OpenMapUri(uri);
		}
	}
}
