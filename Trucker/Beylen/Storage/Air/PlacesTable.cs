using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
{
	public class PlacesTable : AirTable<PlaceRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PlacesTable));

		public PlacesTable(AirBase airBase) :
			base(airBase, "Places")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class PlaceRecord : AirRecord
	{
		public const string CODE_NAME = "Code name";
		public const string FULL_NAME = "Full name";
		public const string TAGS = "Tags";
		public const string ADDRESS = "Address";

		public string CodeName {
			get => GetField<string>(CODE_NAME);
			set => SetField(CODE_NAME, value);
		}

		public string FullName {
			get => GetField<string>(FULL_NAME);
			set => SetField(FULL_NAME, value);
		}

		public string Tags {
			get => GetField<string>(TAGS);
			set => SetField(TAGS, value);
		}

		public string Address {
			get => GetField<string>(ADDRESS);
			set => SetField(ADDRESS, value);
		}
	}
}
