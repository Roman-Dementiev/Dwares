using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Support;
using Dwares.Dwarf;
using Dwares.Drums.Bing;
using Dwares.Drums.Google;


namespace Dwares.Drums
{
	public static class Maps
	{
		static NamedItemsCollection<IMapApplication> applications;
		public static NamedItemsCollection<IMapApplication> Applications {
			get => LazyInitializer.EnsureInitialized(ref applications, () =>
			{
				var factory = new CreateByName<IMapApplication>();
				factory.Add<BingMaps>(nameof(BingMaps));
				factory.Add<GoogleMaps>(nameof(GoogleMaps));

				return new NamedItemsCollection<IMapApplication>(factory);
			});
		}

		static NamedItemsCollection<IMapService> services;
		public static NamedItemsCollection<IMapService> Services {
			get => LazyInitializer.EnsureInitialized(ref services, () =>
			{
				var factory = new CreateByName<IMapService>();
				factory.Add<BingMapsREST>(nameof(BingMapsREST));

				return new NamedItemsCollection<IMapService>(factory);
			});
		}

		public static IRouteOptions DefaultOptions { get; set; } = new RouteOptions {
			TravelMode = TravelMode.Driving,
			Optimization = Optimization.TimeWithTraffic
		};

		public static IMapApplication DefaultApplication {
			get => (Applications.Count > 0) ? Applications[0] : new MapApplicationNotImplemented();
		}

		static IMapApplication mapApplication;
		public static IMapApplication MapApplication {
			get => mapApplication ?? DefaultApplication;
			set => mapApplication = value;
		}

		public static IMapService DefaultService {
			get => (Services.Count > 0) ? Services[0] : new MapServiceNotImplemented();
		}

		static IMapService mapService;
		public static IMapService MapService {
			get => mapService ?? DefaultService;
			set => mapService = value;
		}

		public static IMapApplication GetApplication(string name, bool defaultIfNotFound = true)
		{
			var application = Applications.Get(name);
			if (application == null && defaultIfNotFound) {
				application = DefaultApplication;
			}
			return application;
		}

		public static IMapService GetService(string name, bool defaultIfNotFound = true)
		{
			var service = Services.Get(name);
			if (service == null && defaultIfNotFound) {
				service = DefaultService;
			}
			return service;
		}

		public static IMapApplication SetApplication(String name, bool defaultIfNotFound = true)
		{
			mapApplication = GetApplication(name, defaultIfNotFound);
			return mapApplication;
		}

		public static IMapService SetService(String name, bool defaultIfNotFound = true)
		{
			mapService = GetService(name, defaultIfNotFound);
			return mapService;
		}


		public static void InitDefault(bool addDefaultApplication = true, bool addDefaultService = true)
		{
			if (DeviceEx.Is_UWP) { 
				if (addDefaultApplication) {
					Applications.Add(nameof(BingMaps));
				}
				if (addDefaultService) {
					Services.Add(nameof(BingMapsREST));
				}
			} else {
				// TODO
			}
		}

		public static void InitAll(bool addAllApplications = true, bool addAllService = true, bool defaultFirst = true)
		{
			if (defaultFirst) {
				InitDefault(addAllApplications, addAllService);
			}

			if (addAllApplications) {
				Applications.Add(nameof(BingMaps));
				Applications.Add(nameof(GoogleMaps));
			}
			if (addAllService) {
				Services.Add(nameof(BingMapsREST));
				//TODO
			}
		}

		public static Task OpenAddress(string address)
		{
			return MapApplication.OpenAddress(address);
		}

		public static Task OpenDirections(string from, string dest)
		{
			return MapApplication.OpenDirections(from, dest);
		}

		public static Task OpenDirections(ILocation from, ILocation dest, IRouteOptions options = null)
		{
			return MapApplication.OpenDirections(from, dest, options);
		}

		public static Task<IRouteInfo> GetRouteInfo(IEnumerable<IWaypoint> waypoints, IRouteOptions options = null)
		{
			return MapService.GetRouteInfo(waypoints, options ?? DefaultOptions);
		}

		public static Task<IRouteInfo> GetRouteInfo(ILocation from, ILocation dest, IRouteOptions options = null)
		{
			var waypoints = new Waypoint[] {
				Waypoint.FromLocation(WaypointType.SatrtPoint, from),
				Waypoint.FromLocation(WaypointType.EndPoint, dest)
			};

			return GetRouteInfo(waypoints, options);
		}

		public static Task<IRouteInfo> GetRouteInfo(string from, string dest, IRouteOptions options = null)
		{
			var waypoints = new Waypoint[] {
				Waypoint.FromAddress(WaypointType.SatrtPoint, from),
				Waypoint.FromAddress(WaypointType.EndPoint, dest)
			};

			return GetRouteInfo(waypoints, options);
		}
	}

	public sealed class MapApplicationNotImplemented : IMapApplication
	{
		public string Name => "Not Implemented";
		public Task OpenAddress(string address) => throw new NotImplementedException(nameof(OpenAddress));
		public Task OpenDirections(string from, string dest) => throw new NotImplementedException(nameof(OpenDirections));
		public Task OpenDirections(ILocation from, ILocation dest, IRouteOptions options) => throw new NotImplementedException(nameof(OpenDirections));
	}

	public sealed class MapServiceNotImplemented : IMapService
	{
		public string Name => "Not Implemented";
		//public Task Something() => throw new NotImplementedException(nameof(Something));
		public Task<IRouteInfo> GetRouteInfo(IEnumerable<IWaypoint> waypoints, IRouteOptions options) => throw new NotImplementedException(nameof(GetRouteInfo));
	}
}
