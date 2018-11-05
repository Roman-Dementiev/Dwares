using System;
using System.Collections.Generic;
using System.Text;
using BingMapsRESTToolkit;


namespace Dwares.Drums
{
	public class Location: ILocation
	{
		public string Landmark { get; set; }
		public string Address { get; set; }
		public Coordinate Coordinate { get; }
		ICoordinate ILocation.Coordinate => Coordinate;
	}
}
