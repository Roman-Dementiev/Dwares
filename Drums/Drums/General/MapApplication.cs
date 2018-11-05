using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dwares.Druid.Services;

namespace Dwares.Drums
{
	public abstract class MapApplication : IMapApplication
	{
		public static string Escape(string str)
		{
			if (String.IsNullOrEmpty(str))
				return "";
			return Uri.EscapeDataString(str);
		}

		public abstract Uri GetAddressUri(string address);
		public abstract Uri GetDirectionsUri(string from, string dest);

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
	}
}
