//using System;
//using Xamarin.Forms;
//using Dwares.Druid.Satchel;
//using Dwares.Druid.Services;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using System.Threading.Tasks;

//[assembly: Dependency(typeof(Dwares.Druid.Android.GeoLocator))]

//namespace Dwares.Druid.Android
//{
//	public class GeoLocator : IGeoLocator
//	{
//		public async Task<GeoPosition> GetPosition()
//		{
//			return await GetPosition(GeolocationAccuracy.Medium, TimeSpan.Zero);
//		}

//		public async Task<GeoPosition> GetPosition(GeolocationAccuracy accuracy, TimeSpan timeout)
//		{
//		}
//	}
//}