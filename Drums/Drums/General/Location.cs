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
			if (location.HasAddress) {
				Address = location.GetAddress();
			}

			if (location.HasCoordinate) {
				var coord = location.GetCoordinate();
				Coordinate = new Coordinate(coord.Latitude, coord.Longitude);
			}
		}

		//public string Landmark { get; set; }
		public string Address { get; set; }
		public Coordinate Coordinate { get; set; }


		bool ILocation.HasAddress => !string.IsNullOrEmpty(Address);
		string ILocation.GetAddress() => Address;
		bool ILocation.HasCoordinate => Coordinate != null;
		ICoordinate ILocation.GetCoordinate() => Coordinate;

		public static async Task<Location> GetCurrentLocation()
		{
			var coord = await Coordinate.GetCurrentCoordinate();
			return new Location { Coordinate = coord };
		}
	}
}
