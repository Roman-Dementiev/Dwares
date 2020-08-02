using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
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
		public const string NAME = "Name";
		public const string PHONE = "Phone";

		public string Name {
			get => GetField<string>(NAME);
			set => SetField(NAME, value);
		}

		public string Phone {
			get => GetField<string>(PHONE);
			set => SetField(PHONE, value);
		}
	}
}
