using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Drive.Storage.Air
{
	public class ContactsTable<Contact> : AirTable<Contact> where Contact : ContactRecord
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsTable));

		public ContactsTable(AirBase airBase, string tableName) :
			base(airBase, tableName)
		{
			//Debug.EnableTracing(@class);
		}

        public async Task<Contact[]> ListContacts()
        {
            var list = await ListRecords();
            return list.Records;
        }
    }

	public class ClientsTable : ContactsTable<ClientRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(ClientsTable));

		public ClientsTable(AirBase airBase) :
			base(airBase, "Clients")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class PlacesTable : ContactsTable<PlaceRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PlacesTable));

		public PlacesTable(AirBase airBase) :
			base(airBase, "Places")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class PhonesTable : ContactsTable<PhoneRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PhonesTable));

		public PhonesTable(AirBase airBase) :
			base(airBase, "Phones")
		{
			//Debug.EnableTracing(@class);
		}
	}


	public class ContactRecord : AirRecord
	{
        const string NAME = "Name";
        const string TAGS = "Tags";
        const string PHONE = "Phone";
        const string PHONE_TYPE = "Phone type";
        const string NOTES = "Notes";
		const string ATTACHMENTS = "Attachments";

		public string Name {
            get => GetField<string>(NAME);
            set => SetField(NAME, value);
        }

        public string Tags {
            get => GetField<string>(TAGS);
            set => SetField(TAGS, value);
        }

        public string Phone {
            get => GetField<string>(PHONE);
            set => SetField(PHONE, value);
        }

        public string PhoneType {
            get => GetField<string>(PHONE_TYPE);
            set => SetField(PHONE_TYPE, value);
        }

        public string Notes {
            get => GetField<string>(NOTES);
            set => SetField(NOTES, value);
        }
    }

	public class ClientRecord : ContactRecord
	{
		const string ADDRESS = "Address";
		const string LINK_TO_REGULAR_PLACE = "Link to Regular Place";

		public string Address {
			get => GetField<string>(ADDRESS);
			set => SetField(ADDRESS, value);
		}

		public string RegularPlaceId {
			get => GetLinkId(LINK_TO_REGULAR_PLACE);
		}
	}

	public class PlaceRecord : ContactRecord
	{
		const string FULL_TITLE = "Full Title";
		const string ADDRESS = "Address";

		public string FullTitle {
			get => GetField<string>(FULL_TITLE);
			set => SetField(FULL_TITLE, value);
		}

		public string Address {
			get => GetField<string>(ADDRESS);
			set => SetField(ADDRESS, value);
		}
	}

	public class PhoneRecord : ContactRecord
	{

	}
}
