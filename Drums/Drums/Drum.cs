using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Druid.Services;
using Dwares.Dwarf;


namespace Dwares.Drums
{
	public class Drum
	{
		static DependencyService<Drum> instance;
		public static Drum Instance {
			get {
				var drum = DependencyService<Drum>.GetInstance(ref instance, false);
				if (drum == null) {
					drum = new Drum(null, null);
					DependencyService<Drum>.SetInstance(ref instance, drum);
				}
				return drum;
			}
			set {
				DependencyService<Drum>.SetInstance(ref instance, value);
			}
		}

		static DependencyService<IMapApplication> mapApplication;
		public static IMapApplication DefaultMapAppication {
			get => DependencyService<IMapApplication>.GetInstance(ref mapApplication, false);
			set => DependencyService<IMapApplication>.SetInstance(ref mapApplication, value);
		}

		static DependencyService<IMapService> mapService;
		public static IMapService DefaultMapService {
			get => DependencyService<IMapService>.GetInstance(ref mapService, false);
			set => DependencyService<IMapService>.SetInstance(ref mapService, value);
		}

		protected Drum() { }

		public Drum(IMapApplication app, IMapService svc, IRouteOptions defaultOptions = null)
		{
			Init(app, svc, defaultOptions);
		}

		protected void Init(IMapApplication app, IMapService svc, IRouteOptions defaultOptions = null)
		{
			if (app == null) {
				app = DefaultMapAppication;
			}
			if (svc == null) {
				svc = DefaultMapService;
			}

			Application = app ?? new MapApplicationNotImplemented();
			Service = svc ?? new MapServiceNotImplemented();
			DefaultOptions = defaultOptions ?? new RouteOptions {
				TravelMode = TravelMode.Driving,
				Optimization = Optimization.TimeWithTraffic
			};
		}

		public IMapApplication Application { get; private set; }
		public IMapService Service { get; private set; }
		public IRouteOptions DefaultOptions { get; set; }

		public static Task OpenAddress(string address)
		{
			return Instance.Application.OpenAddress(address);
		}

		public static Task OpenDirections(string from, string dest)
		{
			return Instance.Application.OpenDirections(from, dest);
		}

		public static Task<IRouteInfo> GetRouteInfo(IRouteOptions options, IEnumerable<IWaypoint> waypoints)
		{
			return Instance.Service.GetRouteInfo(options ?? Instance.DefaultOptions, waypoints);
		}

		public static Task<IRouteInfo> GetRouteInfo(ILocation from, ILocation dest, IRouteOptions options = null)
		{
			var waypoints = new Waypoint[] {
				new Waypoint(WaypointType.SatrtPoint, from),
				new Waypoint(WaypointType.EndPoint, dest)
			};

			return GetRouteInfo(options, waypoints);
		}

		public static Task<IRouteInfo> GetRouteInfo(string from, string dest, IRouteOptions options = null)
		{
			var waypoints = new Waypoint[] {
				new Waypoint(WaypointType.SatrtPoint) { Address = from  },
				new Waypoint(WaypointType.EndPoint) { Address = dest }
			};

			return GetRouteInfo(options, waypoints);
		}
	}

	public sealed class MapApplicationNotImplemented : IMapApplication
	{
		public Task OpenAddress(string address) => throw new NotImplementedException(nameof(OpenAddress));
		public Task OpenDirections(string from, string dest) => throw new NotImplementedException(nameof(OpenDirections));
	}

	public sealed class MapServiceNotImplemented : IMapService
	{
		//public Task Something() => throw new NotImplementedException(nameof(Something));
		public Task<IRouteInfo> GetRouteInfo(IRouteOptions options, IEnumerable<IWaypoint> waypoints) => throw new NotImplementedException(nameof(GetRouteInfo));
	}
}
