using System;
using System.Threading.Tasks;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf;


namespace Drive.Storage.Air
{
	public class BasesTable : AirTable<BaseRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(BasesTable));

		public BasesTable(AirBase airBase) :
			base(airBase, "Bases")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class BaseRecord : ARecord
	{
		//public const string KIND = "Kind";
		public const string BASE_ID = "BaseId";

		public BaseRecord() { }

		//public string Kind {
		//	get => GetField<string>(KIND);
		//	set => SetField(KIND, value);
		//}

		public string BaseId {
			get => GetField<string>(BASE_ID);
			set => SetField(BASE_ID, value);
		}

	}
}
