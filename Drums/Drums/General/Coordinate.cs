using System;
using System.Threading.Tasks;
using System.Globalization;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;
//using Dwares.Druid.Services;
using Xamarin.Essentials;


namespace Dwares.Drums
{
	public class Coordinate: GeoCoordinate, ICoordinate
	{
		public const string DefaultFormat = "{0:0.#####},{1:0.#####}";
		
		#region Constructor
		/// <summary>
		/// A location coordinate.
		/// </summary>
		public Coordinate()
		{
		}

		/// <summary>
		/// A location coordinate.
		/// </summary>
		/// <param name="latitude">Latitude coordinate vlaue.</param>
		/// <param name="longitude">Longitude coordinate value.</param>
		public Coordinate(double latitude, double longitude) :
			base(latitude, longitude)
		{
		}

		public Coordinate(ICoordinate c)
		{
			if (c != null) {
				Latitude = c.Latitude;
				Longitude = c.Longitude;
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Latitude coordinate.
		/// </summary>
		public new double Latitude {
			get => base.Latitude;
			set {
				if (IsValidLatitude(value)) {
					//Only need to keep the first 5 decimal places. Any more just adds more data being passed around.
					base.Latitude = Math.Round(value, 5, MidpointRounding.AwayFromZero);
				}
			}
		}

		/// <summary>
		/// Longitude coordinate.
		/// </summary>
		public new double Longitude {
			get => base.Longitude;
			set {
				if (IsValidLongitude(value)) {
					//Only need to keep the first 5 decimal places. Any more just adds more data being passed around.
					Longitude = Math.Round(value, 5, MidpointRounding.AwayFromZero);
				}
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Returns a formatted string of the coordinate in the format "latitude,longitude", with the values rounded off to 5 decimal places.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => ToString(DefaultFormat);

		public string ToString(string format)
		{
			return string.Format(CultureInfo.InvariantCulture, format, Latitude, Longitude);
		}

		/// <summary>
		/// Compares two coordinates. Only compares the first 6 decimal places to avoid floating point errors.
		/// </summary>
		/// <param name="obj">Coordinate to compare to.</param>
		/// <returns>A boolean indicating if the two coordinates are equal.</returns>
		public override bool Equals(object obj)
		{
			if (obj != null && obj is ICoordinate c) {
				return Equals(c, 6);
			}

			return false;
		}

		/// <summary>
		/// Compares two coordinates. 
		/// </summary>
		/// <param name="c">Coordinate to compare to.</param>
		/// <param name="decimals">The number of decimal places to compare to.</param>
		/// <returns>A boolean indicating if the two coordinates are equal.</returns>
		public bool Equals(ICoordinate c, int decimals)
		{
			return Math.Round(Latitude, decimals) == Math.Round(c.Latitude, decimals) && Math.Round(Longitude, decimals) == Math.Round(c.Longitude, decimals);
		}

		/// <summary>
		/// Get hash for coordinate.
		/// </summary>
		/// <returns>Hash for coordinate.</returns>
		public override int GetHashCode()
		{
			return ToString(DefaultFormat).GetHashCode();
		}

		#endregion

		public static implicit operator Coordinate(GeoPosition pos)
		{
			return new Coordinate(pos.Latitude, pos.Longitude);
		}

		public static implicit operator GeoPosition(Coordinate c)
		{
			return new GeoPosition { Latitude = c.Latitude, Longitude = c.Longitude };
		}

		public static async Task<Coordinate> GetCurrentCoordinate()
		{
			var location = await Geolocation.GetLocationAsync();
			//var geoPosition = await GeoLocator.GetPosition();
			return new Coordinate(location.Latitude, location.Longitude);
		}
	}

}
