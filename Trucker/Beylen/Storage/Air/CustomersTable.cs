using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
{
	public class CustomersTable : AirTable<CustomerRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(CustomersTable));

		public CustomersTable(AirBase airBase) :
			base(airBase, "Customers")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class CustomerRecord : AirRecord
	{
		public const string CODE_NAME = "Code name";
		public const string REAL_NAME = "Real name";
		public const string ALIAS = "Alias";
		public const string TAGS = "Tags";
		public const string ADDRESS = "Address";
		public const string PHONE = "Phone";
		public const string CONTACT_PHONE = "Contact phone";
		public const string CONTACT_NAME = "Contact name";

		public string CodeName {
			get => GetField<string>(CODE_NAME);
			set => SetField(CODE_NAME, value);
		}

		public string RealName {
			get => GetField<string>(REAL_NAME);
			set => SetField(REAL_NAME, value);
		}

		public string Alias {
			get => GetField<string>(ALIAS);
			set => SetField(ALIAS, value);
		}

		public string Tags {
			get => GetField<string>(TAGS);
			set => SetField(TAGS, value);
		}

		public string Address {
			get => GetField<string>(ADDRESS);
			set => SetField(ADDRESS, value);
		}

		public string Phone {
			get => GetField<string>(PHONE);
			set => SetField(PHONE, value);
		}

		public string ContactPhone {
			get => GetField<string>(CONTACT_PHONE);
			set => SetField(CONTACT_PHONE, value);
		}

		public string ContactName {
			get => GetField<string>(CONTACT_NAME);
			set => SetField(CONTACT_NAME, value);
		}
	}
}
