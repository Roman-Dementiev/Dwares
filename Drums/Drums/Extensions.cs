using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Drums
{
	public static class Extensions
	{
		public static bool IsValidCoordinate(this ICoordinate c)
		{
			return c != null && GeoCoordinate.IsValidLatitude(c.Latitude) && GeoCoordinate.IsValidLongitude(c.Longitude);
		}

		public static bool IsValidLocation(this ILocation l)
		{
			return l != null && (!String.IsNullOrEmpty(l.Address) || IsValidCoordinate(l.Coordinate));
		}
	}
}
