using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Services;
using BingMapsRESTToolkit;


namespace Dwares.Drums.Bing
{
	public class BingMapsREST : MapService
	{
		//const string BingMapsKey = "At3kj4rBGQ5lVXSMcxAoYc7AQ2tLFhbyfikyPfaEbXuw03XiRTGCWAdYeiUzqFNa";
		static string bingMapsKey;
		static string BingMapsKey {
			get => LazyInitializer.EnsureInitialized(ref bingMapsKey, () => {
				return Preferences.Get("BingMaps.Key", string.Empty);
			});
		}

		public BingMapsREST() : base(nameof(BingMapsREST)) { }

		public override async Task<IRouteInfo> GetRouteInfo(IEnumerable<IWaypoint> waypoints, IRouteOptions options)
		{
			var request = new RouteRequest()
			{
				RouteOptions = new BingMapsRESTToolkit.RouteOptions() {
					TravelMode = BingConvert.TravelMode(options),
					Avoid = BingConvert.Avoid(options),
					DistanceUnits = DistanceUnitType.Miles,
					Heading = 45,
					RouteAttributes = new List<RouteAttributeType>() {
						RouteAttributeType.RouteSummariesOnly
					},
					Optimize = RouteOptimizationType.TimeWithTraffic
				},
				Waypoints = BingWaypoint.List(waypoints),
				BingMapsKey = BingMapsKey
			};

			var route = await ProcessRequest<Route>(request);

			if (route != null) {
				return new BingRouteInfo(route);
			} else {
				return null;
			}
		}


		private async Task<ResultType> ProcessRequest<ResultType>(BaseRestRequest request) where ResultType : Resource
		{
			try {
				//Execute the request.
				var response = await request.Execute();

				var resourses = response?.ResourceSets;
				if (resourses != null && resourses.Length > 0 && resourses[0].Resources.Length > 0) {
					var resourceSet = resourses[0];
					var resource = resourceSet.Resources[0];
					if (resource is ResultType result) {
						return result;
					}
				} else {
					Debug.Print("BingMapsRest.ProcessRequest() - Response is empty");
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				//MessageBox.Show(ex.Message);
			}
			return null;
		}
	}
}
