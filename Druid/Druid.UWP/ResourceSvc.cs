//using System;
//using Xamarin.Forms;
//using Dwares.Dwarf;
//using Dwares.Druid.Services;
//using Windows.ApplicationModel.Resources;


//[assembly: Dependency(typeof(Druid.UWP.ResourceSvc))]
//namespace Druid.UWP
//{
//	public class ResourceSvc : IResourceSvc
//	{
//		public static ClassRef @class => new ClassRef(typeof(ResourceSvc));

//		public string GetString(string key, bool useKeyAsDefault = true)
//		{
//			string value = null;
//			try {
//				var loader = ResourceLoader.GetForCurrentView();
//				//var loader = ResourceLoader.GetForViewIndependentUse();
//				value = loader.GetString(key);
//			}
//			catch (Exception ex) {
//				Debug.Print("Can not get string: key={0}");
//				Debug.Print("{0}", ex);
//			}

//			if (String.IsNullOrEmpty(value) && useKeyAsDefault)
//				value = key;
//			return value;
//		}
//	}
//}
