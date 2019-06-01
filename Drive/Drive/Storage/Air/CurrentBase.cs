using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Drive.Storage.Air
{
	public class CurrentBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsBase));

		public CurrentBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);

			ClientsTable = new ClientsTable(this);
			PlacesTable = new PlacesTable(this);
			PhonesTable = new PhonesTable(this);
			RidesTable = new RidesTable(this);
		}

		public ClientsTable ClientsTable { get; }
		public PlacesTable PlacesTable { get; }
		public PhonesTable PhonesTable { get; }

		public RidesTable RidesTable { get; }
	}
}
