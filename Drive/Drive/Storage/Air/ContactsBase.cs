using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Drive.Storage.Air
{
	public class ContactsBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsBase));

		public ContactsBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);

			ContactsTable = new ContactsTable(this);
		}

		public ContactsTable ContactsTable { get; }
	}
}
