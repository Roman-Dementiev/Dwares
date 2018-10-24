using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Druid.Services;

[assembly: Dependency(typeof(Dwares.Druid.UWP.BingMapSvc))]

namespace Dwares.Druid.UWP
{
	class BingMapSvc : IMapSvc
	{
		public Task OpenMapUri(string uri)
		{
			var options = new Dictionary<string, object> {
				{ "TargetApplicationPackageFamilyName", "Microsoft.WindowsMaps_8wekyb3d8bbwe" }
			};

			return Dwares.Druid.Services.Launcher.OpenUri(new Uri(uri), options);
		}

		public static string Escape(string str)
		{
			if (String.IsNullOrEmpty(str))
				return "";
			return Uri.EscapeDataString(str);
		}

		public static string Adderss(string address)
		{
			if (String.IsNullOrEmpty(address))
				return "";
			return "adr." +  Uri.EscapeDataString(address);
		}


		public Task OpenAddress(string address)
		{
			var uri = String.Format("bingmaps:?rtp={0}", Adderss(address));
			return OpenMapUri(uri);
		}

		public Task OpenDirections(string from, string dest)
		{
			var uri = String.Format("bingmaps:?rtp={0}~{1}", Adderss(from), Adderss(dest));
			return OpenMapUri(uri);
		}
	}
}
