using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;



namespace Buffy.Storage.Air
{
	public class BasesTable : AirTable<BaseRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(BasesTable));

		public BasesTable(AirBase dataBase) :
			base(dataBase, "Bases")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class BaseRecord : AirRecord
	{
		public const string NAME = "Name";
		public const string START_DATE = "Start Date";
		public const string END_DATE = "End Date";
		public const string BASE_ID = "Base Id";

		public string Name {
			get => GetField<string>(NAME);
			set => SetField(NAME, value);
		}

		public DateOnly StartDate {
			get => GetField<DateOnly>(START_DATE);
			set => SetField(START_DATE, value);
		}

		public DateOnly EndDate {
			get => GetField<DateOnly>(END_DATE);
			set => SetField(START_DATE, value);
		}

		public string BaseId {
			get => GetField<string>(BASE_ID);
			set => SetField(BASE_ID, value);
		}
	}
}
