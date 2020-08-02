using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
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

	public class BaseRecord : AirRecord
	{
		public const string NAME = "Name";
		public const string BASE_ID = "BaseId";

		public BaseRecord() { }

		public string Name {
			get => GetField<string>(NAME);
			set => SetField(NAME, value);
		}

		public string BaseId {
			get => GetField<string>(BASE_ID);
			set => SetField(BASE_ID, value);
		}
	}
}
