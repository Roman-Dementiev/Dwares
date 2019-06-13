using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;

namespace Drive.Storage.Air
{
	public class MainBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(MainBase));

		public MainBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);

			BasesTable = new BasesTable(this);
			TagsTable = new TagsTable(this);
			PhonesTable = new PhonesTable(this);
			PlacesTable = new PlacesTable(this);
			ClientsTable = new ClientsTable(this);
		}

		public BasesTable BasesTable { get; }
		public TagsTable TagsTable { get; }
		public PhonesTable PhonesTable { get; }
		public PlacesTable PlacesTable { get; }
		public ClientsTable ClientsTable { get; }


		public override async Task Initialize()
		{
			await TagsTable.Initialize();
			await BasesTable.Initialize();
			await PhonesTable.Initialize();
			await PlacesTable.Initialize();
			await ClientsTable.Initialize();
		}
	}
}
