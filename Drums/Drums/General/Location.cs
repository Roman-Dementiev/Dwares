using System;
using System.Threading.Tasks;


namespace Dwares.Drums
{
	public class Location: ILocation
	{
		public Location() { }
		public Location(ILocation location)
		{
			if (location == null)
				return;

			//Landmark = location.Landmark;
			Address = location.Address;

			if (location.Coordinate != null) {
				Coordinate = new Coordinate(location.Coordinate.Latitude, location.Coordinate.Longitude);
			}
		}

		//public string Landmark { get; set; }
		public string Address { get; set; }
		public Coordinate Coordinate { get; set; }
		ICoordinate ILocation.Coordinate => Coordinate;

		public static async Task<Location> GetCurrentLocation()
		{
			var coord = await Coordinate.GetCurrentCoordinate();
			return new Location { Coordinate = coord };
		}
	}
}
