using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using BingMapsRESTToolkit;
using Dwares.Druid;
using Dwares.Dwarf;
using Ziply.Models;


namespace Ziply.ViewModels
{
	public partial class EtaViewModel : BaseViewModel
	{
		public EtaViewModel() :
			base(expireMinutes: 10)
		{
			Clear();
			SetButtonText(null);

			Address = Settings.DestinationAddress;
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


		public override async Task Refresh(bool silent)
		{
			if (string.IsNullOrEmpty(Address)) {
				if (!silent) {
					await Alerts.DisplayAlert(null, "Please enter address to see ETA");
				}
				return;
			}

			Settings.DestinationAddress = Address;

			Clear();

			IsBusy = true;
			try {
				var geolocation = await Geolocation.GetLocationAsync();

				var timeZone = await GetTimeZone(geolocation.Latitude, geolocation.Longitude);
				var route = await SendRequest(geolocation.Latitude, geolocation.Longitude);
				LastRefreshed = DateTime.Now;

				UpdateFromRoute(route, timeZone);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			finally {
				IsBusy = false;
			}
		}

		public override void Clear()
		{
			Eta = Duration = Distance = string.Empty;
		}

		void UpdateFromRoute(Route route, TimeZoneInfo currentTZ)
		{
			if (route == null) {
				Clear();
				SetButtonText(null);
			} else {
				double duration = route.TravelDuration;
				if (route.TimeUnitType == TimeUnitType.Second)
					duration /= 60;


				var eta = DateTime.Now.AddMinutes(duration);
				eta = AdjustETA(eta, currentTZ);

				int hh = (int)duration / 60;
				int mm = (int)duration - hh * 60;
				if (hh > 0) {
					Duration = $"{hh} h  {mm} min";
				} else {
					if (mm == 0)
						mm = 1;
					Duration = $"{mm} min";
				}

				double distance = route.TravelDistance;
				if (route.DistanceUnitType == DistanceUnitType.Kilometers)
					distance *= 0.621371;
				distance = Math.Round(distance);
				Distance = $"{(int)distance} mi";

				//SetButtonText(Eta);
				SetButtonText($"{eta.ToShortDateString()} \n {Eta}");
			}
		}

		DateTime AdjustETA(DateTime eta, TimeZoneInfo currentTZ)
		{
			if (currentTZ != null && DestinationTimeZone != null) {
				var adjustment = DestinationTimeZone.UtcOffset - currentTZ.UtcOffset;
				eta = eta.AddHours(adjustment);
				Eta = eta.ToString("t") + " " + DestinationTimeZone.Abbr;
			} else {
				Eta = eta.ToString("t");
			}
			return eta;
		}


		public async Task<Route> SendRequest(double latitude, double longitude)
		{
			var startPoint = new SimpleWaypoint(latitude, longitude);
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

		public void OnDestinationChanged()
		{
			DestinationTimeZone = null;
		}
	}
}
