using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using BingMapsRESTToolkit;


namespace Dwares.Drums.Bing
{
	public class BingMapsRest : IMapService
	{
		const string BingMapsKey = "At3kj4rBGQ5lVXSMcxAoYc7AQ2tLFhbyfikyPfaEbXuw03XiRTGCWAdYeiUzqFNa";

	//	public async Task Something()
	//	{
	//		Debug.Print("BingMapsRest.Something()");

	//		var r = new RouteRequest() {
	//			RouteOptions = new RouteOptions() {
	//				Avoid = new List<AvoidType>()
	//	{
	//					AvoidType.MinimizeTolls
	//				},
	//				TravelMode = TravelModeType.Driving,
	//				DistanceUnits = DistanceUnitType.Miles,
	//				Heading = 45,
	//				RouteAttributes = new List<RouteAttributeType>()
	//	{
	//					RouteAttributeType.RoutePath
	//				},
	//				Optimize = RouteOptimizationType.TimeWithTraffic
	//			},
	//			Waypoints = new List<SimpleWaypoint>()
	//{
	//				new SimpleWaypoint(){
	//					Address = "Seattle, WA"
	//				},
	//				new SimpleWaypoint(){
	//					Address = "Bellevue, WA",
	//					IsViaPoint = true
	//				},
	//				new SimpleWaypoint(){
	//					Address = "Redmond, WA"
	//				}
	//			},
	//			BingMapsKey = BingMapsKey
	//		};

	//		await ProcessSomething(r);
	//	}

	//	private async Task ProcessSomething(BaseRestRequest request)
	//	{
	//		try {
	//			//Execute the request.
	//			var response = await request.Execute();
				
	//			var resourses = response?.ResourceSets;
	//			if (resourses != null && resourses.Length > 0) {
	//				var resourceSet = resourses[0];
	//				var resource = resourceSet.Resources[0];
	//				Debug.Print("BingMapsRest.ProcessRequest() - resource={0}", resource);
	//				if (resource is Route route) {
	//					Debug.Print("TravelDistance = {0}{1} TravelDuration {2}{3} TrafficCongestion={4}",
	//						route.TravelDistance, route.DistanceUnit,
	//						route.TravelDuration, route.DurationUnit,
	//						route.TrafficCongestion
	//						);
	//				}
	//			} else {
	//				Debug.Print("BingMapsRest.ProcessRequest() - Response is empty");
	//			}
	//		}
	//		catch (Exception ex) {
	//			Debug.ExceptionCaught(ex);
	//			//MessageBox.Show(ex.Message);
	//		}
	//	}

		public async Task<IRouteInfo> GetRouteInfo(IRouteOptions options, IEnumerable<IWaypoint> waypoints)
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

			return new BingRouteInfo(route);
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
