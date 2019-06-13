using System;
using System.Threading.Tasks;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf;
using Drive.Models;


namespace Drive.Storage.Air
{
	public class PlacesTable : ContactsTable<PlaceRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PlacesTable));

		public PlacesTable(AirBase airBase) :
			base(airBase, "Places")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class PlaceRecord : ContactRecord
	{
		public PlaceRecord() { }

	}
}
