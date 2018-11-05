using System;
using System.Collections.Generic;
using BingMapsRESTToolkit;


namespace Dwares.Drums.Bing
{
	public class BingCoordinate : BingMapsRESTToolkit.Coordinate, ICoordinate
	{
		public BingCoordinate() {}

		public BingCoordinate(double latitude, double longitude) :
			base(latitude, longitude)
		{
		}

		public BingCoordinate(ICoordinate c) :
			base(c.Latitude, c.Longitude)
		{
		}

		public static BingCoordinate ToBing(ICoordinate c)
		{
			if (c == null)
				return null;

			if (c is BingCoordinate bc)
				return bc;

			return new BingCoordinate(c.Latitude, c.Longitude);
		}
	}
}
