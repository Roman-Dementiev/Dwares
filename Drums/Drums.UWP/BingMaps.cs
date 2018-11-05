using System;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(Dwares.Drums.UWP.BingMaps))]
[assembly: Dependency(typeof(Dwares.Drums.UWP.BingMapsRest))]

namespace Dwares.Drums.UWP
{
	public class BingMaps : Dwares.Drums.Bing.BingMaps
	{
		public override Task OpenMapUri(Uri uri)
		{
			//var options = new Dictionary<string, object> {
			//	{ "TargetApplicationPackageFamilyName", "Microsoft.WindowsMaps_8wekyb3d8bbwe" }
			//};

			//return Launcher.OpenUri(uri, options);
			return base.OpenMapUri(uri);
		}
	}

	public class BingMapsRest : Dwares.Drums.Bing.BingMapsRest
	{

	}
}
