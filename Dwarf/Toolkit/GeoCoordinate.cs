//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Dwares.Dwarf.Toolkit
//{
//	public class GeoCoordinate
//	{
//		public GeoCoordinate() { }

//		public GeoCoordinate(double latitude, double longitude, double? altitude = null)
//		{
//			Latitude = latitude;
//			Longitude = longitude;
//			Altitude = altitude;
//		}

//		public double Latitude { get; set; }
//		public double Longitude { get; set; }
//		public double? Altitude { get; set; }

//		//public bool IsValidCoordinate {
//		//	get => IsValidLatitude(Latitude) && IsValidLongitude(Longitude);
//		//}

//		public override string ToString()
//		{
//			return Strings.Properties(this, skipNull: true);
//		}

//		public static bool IsValidLatitude(double latitude)
//		{
//			return !double.IsNaN(latitude) && latitude <= 90 && latitude >= -90;
//		}

//		public static bool IsValidLongitude(double longitude)
//		{
//			return !double.IsNaN(longitude) && longitude <= 90 && longitude >= -90;
//		}
//	}
//}
