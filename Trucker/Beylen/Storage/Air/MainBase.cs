using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
{
	public class MainBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(MainBase));

		public MainBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);

			BasesTable = new BasesTable(this);
			ContactsTable = new ContactsTable(this);
			CustomersTable = new CustomersTable(this);
			PlacesTable = new PlacesTable(this);
			RouteTable = new RouteTable(this);
		}

		public BasesTable BasesTable { get; }
		public ContactsTable ContactsTable { get; }
		public CustomersTable CustomersTable { get; }
		public PlacesTable PlacesTable { get; }
		public RouteTable RouteTable { get; }
	}
}
