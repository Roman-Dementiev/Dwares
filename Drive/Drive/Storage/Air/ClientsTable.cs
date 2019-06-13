using System;
using System.Threading.Tasks;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf;


namespace Drive.Storage.Air
{
	public class ClientsTable : AirTable<ClientRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(ClientsTable));

		public ClientsTable(AirBase airBase) :
			base(airBase, "Clients")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class ClientRecord : ContactRecord
	{
		public const string PLACE = "Place";
		public const string REGULAR = "Regular destination";

		public ClientRecord() { }

		public string PlaceId {
			get => GetLinkId(PLACE);
			//set => SetLinkId(PLACE, value);
		}

		public string RegularId {
			get => GetLinkId(REGULAR);
			//set => SetLinkId(REGULAR, value);
		}
	}
}
