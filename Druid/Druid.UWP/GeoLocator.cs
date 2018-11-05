using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Dwares.Druid.Essential;
using Dwares.Druid.Services;
using Xamarin.Forms;


[assembly: Dependency(typeof(Dwares.Druid.UWP.GeoLocator))]

namespace Dwares.Druid.UWP
{
	public class GeoLocator : IGeoLocator
	{
		public async Task<GeoPosition> GetPosition()
		{
			return await GetPosition(GeolocationAccuracy.Medium, TimeSpan.Zero);
		}

		public async Task<GeoPosition> GetPosition(GeolocationAccuracy accuracy, TimeSpan timeout)
		{
			var coordinate = await GetGeocoordinate(AccuracyInMeters(accuracy), timeout);
			return ToGeoPosition(coordinate);

		}

		public static async Task<Geocoordinate> GetGeocoordinate(uint accuracyInMeters, TimeSpan timeout)
		{
			await Permissions.Require(PermissionType.LocationWhenInUse);

			var geolocator = new Geolocator {
				DesiredAccuracyInMeters = accuracyInMeters
			};

			switch (geolocator.LocationStatus)
			{
			case PositionStatus.Disabled:
			case PositionStatus.NotAvailable:
				throw new FeatureNotEnabledException("Geolocation service", "");
			}

			var cancellationToken = TimeoutToken(timeout);
			var geoposition = await geolocator.GetGeopositionAsync().AsTask(cancellationToken);
			return geoposition?.Coordinate;
		}

		static CancellationToken TimeoutToken(TimeSpan timeout, CancellationToken token = default(CancellationToken))
		{
			//create a new linked cancellation token source
			//var cancelTokenSrc = CancellationTokenSource.CreateLinkedTokenSource(token);
			var cancelTokenSrc = new CancellationTokenSource();

			// if a timeout was given, make the token source cancel after it expires
			if (timeout > TimeSpan.Zero)
				cancelTokenSrc.CancelAfter(timeout);

			// our Cancel method will handle the actual cancellation logic
			return cancelTokenSrc.Token;
		}

		GeoPosition ToGeoPosition(Geocoordinate coord)
		{
			if (coord == null)
				return null;

			return new GeoPosition {
				Latitude = coord.Point.Position.Latitude,
				Longitude = coord.Point.Position.Longitude,
				Timestamp = coord.Timestamp,
				Altitude = coord.Point.Position.Altitude,
				Accuracy = coord.Accuracy,
				Speed = (!coord.Speed.HasValue || double.IsNaN(coord.Speed.Value)) ? null : coord.Speed,
				Course = (!coord.Heading.HasValue || double.IsNaN(coord.Heading.Value)) ? null : coord.Heading
			};
		}

		public static uint AccuracyInMeters(GeolocationAccuracy accuracy)
		{
			switch (accuracy)
			{
			case GeolocationAccuracy.Lowest:
				return 3000;
			case GeolocationAccuracy.Low:
				return 1000;
			case GeolocationAccuracy.Medium:
				return 100;
			case GeolocationAccuracy.High:
				return 10; // Equivalent to PositionAccuracy.High
			case GeolocationAccuracy.Best:
				return 1;
			default:
				return 500; // Equivalent to PositionAccuracy.Default
			}
		}
	}
}
