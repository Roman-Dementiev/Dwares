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
			ProduceTable = new ProduceTable(this);
			ContactsTable = new ContactsTable(this);
			CustomersTable = new CustomersTable(this);
			PlacesTable = new PlacesTable(this);
			InvoicesTable = new InvoicesTable(this);
			RouteTable = new RouteTable(this);
			PropertiesTable = new PropertiesTable(this);
		}

		public BasesTable BasesTable { get; }
		public ProduceTable ProduceTable { get; }
		public ContactsTable ContactsTable { get; }
		public CustomersTable CustomersTable { get; }
		public PlacesTable PlacesTable { get; }
		public InvoicesTable InvoicesTable { get; }
		public RouteTable RouteTable { get; }
		public PropertiesTable PropertiesTable { get; }
	}
}
