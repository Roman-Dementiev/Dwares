using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BingMapsRESTToolkit;
using Dwares.Druid;
using Dwares.Dwarf;
using Xamarin.Essentials;

namespace Ziply.ViewModels
{
	public class EtaViewModel : BaseViewModel
	{
		public EtaViewModel() :
			base(expireMinutes: 10)
		{
			Route = null;
			UpdateFromRoute();
		}

		public string Eta {
			get => eta;
			set => SetProperty(ref eta, value);
		}
		string eta;

		public string Duration {
			get => duration;
			set => SetProperty(ref duration, value);
		}
		string duration;

		public string Distance {
			get => distance;
			set => SetProperty(ref distance, value);
		}
		string distance;

		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address;

		public Route Route {
			get => route;
			set => SetProperty(ref route, value);
		}
		Route route;

		public override async Task Refresh(bool silent)
		{
			if (string.IsNullOrEmpty(Address)) {
				if (!silent) {
					await Alerts.DisplayAlert(null, "Please enter address to see ETA");
				}
				return;
			}

			Route = null;
			UpdateFromRoute();

			IsBusy = true;
			try {
				Route = await SendRequest();
				LastRefreshed = DateTime.Now;

				UpdateFromRoute();
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			finally {
				IsBusy = false;
			}
		}

		void UpdateFromRoute()
		{
			if (Route == null) {
				Eta = Duration = ButtonText = string.Empty;
			} else {
				double duration = Route.TravelDuration;
				if (Route.TimeUnitType == TimeUnitType.Second)
					duration /= 60;


				DateTime eta = DateTime.Now.AddMinutes(duration);
				Eta = eta.ToString("t");

				int hh = (int)duration / 60;
				int mm = (int)duration - hh * 60;
				if (hh > 0) {
					Duration = $"{hh} h  {mm} min";
				} else {
					if (mm == 0)
						mm = 1;
					Duration = $"{mm} min";
				}

				double distance = Route.TravelDistance;
				if (Route.DistanceUnitType == DistanceUnitType.Kilometers)
					distance *= 0.621371;
				distance = Math.Round(distance);
				Distance = $"{(int)distance} mi";
			}

			SetButtonText(Eta);
		}


		public async Task<Route> SendRequest()
		{
			var geolocation = await Geolocation.GetLocationAsync();
			var startPoint = new SimpleWaypoint(geolocation.Latitude, geolocation.Longitude);
			var endPoint = new SimpleWaypoint(Address);

			var request = new RouteRequest() {
				RouteOptions = new RouteOptions() {
					TravelMode = TravelModeType.Driving,
					DistanceUnits = DistanceUnitType.Miles,
					//Heading = 45,
					RouteAttributes = new List<RouteAttributeType>() {
						RouteAttributeType.RouteSummariesOnly
					},
					Optimize = RouteOptimizationType.TimeWithTraffic
				},
				Waypoints = new List<SimpleWaypoint> { startPoint, endPoint },
				BingMapsKey = BMK
			};

			var response = await request.Execute();

			foreach (var rs in response.ResourceSets) {
				int i = 0;
				foreach (var resource in rs.Resources) {
					var route = resource as Route;
					if (route != null) {
						return route;
					}
				}
			}

			return null;
		}

		protected override string GetMessageText()
		{
			if (string.IsNullOrEmpty(Eta))
				return null;

			return "ETA: " + Eta;
		}
	}
}
