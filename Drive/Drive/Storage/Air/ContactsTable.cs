using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Drive.Storage.Air
{
	public class ContactsTable : AirTable<ContactRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsTable));

		public ContactsTable(AirBase airBase) :
			base(airBase, "Contacts")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class ContactRecord : AirRecord
	{
	}
}
