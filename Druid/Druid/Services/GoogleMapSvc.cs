using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dwares.Druid.Services
{
	public class GoogleMapSvc : IMapSvc
	{
		public static string Escape(string str)
		{
			if (String.IsNullOrEmpty(str))
				return "";
			return Uri.EscapeDataString(str);
		}

		public Task OpenSearchUri(string query)
		{
			var uri = String.Format("http://www.google.com/maps/search/?api=1&query={0}", Escape(query));
			return Launcher.OpenUri(new Uri(uri));
		}

		public Task OpenAddress(string address)
		{
			return OpenSearchUri(address);
		}

		public Task OpenDirections(string from, string dest)
		{
			// TODO
			var uri = "http://maps.google.com/";
			return Launcher.OpenUri(new Uri(uri));
		}
	}
}
