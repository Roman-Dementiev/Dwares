using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Drums
{
	public static class Extensions
	{
		public static bool IsValidCoordinate(this ICoordinate c)
		{
			return c != null && Coordinate.IsValidLatitude(c.Latitude) && Coordinate.IsValidLongitude(c.Longitude);
		}

		public static bool IsValidLocation(this ILocation l)
		{
			if (l == null)
				return false;

			if (l.HasAddress && !string.IsNullOrEmpty(l.GetAddress()))
				return true;

			return l.HasCoordinate && IsValidCoordinate(l.GetCoordinate());
		}
	}
}
