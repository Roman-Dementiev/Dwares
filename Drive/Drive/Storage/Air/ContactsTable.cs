using System;
using System.Threading.Tasks;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf;


namespace Drive.Storage.Air
{
	public class ContactsTable<TRecord> : AirTable<TRecord> where TRecord : ContactRecord
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsTable));

		public ContactsTable(AirBase airBase, string tableName) :
			base(airBase, tableName)
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class ContactRecord : ARecord
	{
		public const string PHONE = "Phone";
		public const string PHONE_TYPE = "Phone type";
		public const string ADDRESS = "Address";

		public string Phone {
			get => GetField<string>(PHONE);
			set => SetField(PHONE, value);
		}

		public string PhoneType {
			get => GetField<string>(PHONE_TYPE);
			set => SetField(PHONE_TYPE, value);
		}
		public string Address {
			get => GetField<string>(ADDRESS);
			set => SetField(ADDRESS, value);
		}
	}
}
