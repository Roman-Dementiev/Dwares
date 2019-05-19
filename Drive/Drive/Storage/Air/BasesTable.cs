using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


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

		public async Task<BaseRecord[]> ListBases()
		{
			var list = await ListRecords();
			return list.Records;
		}
	}

	public class BaseRecord : AirRecord
	{
		const string BASE_ID = "BaseId";
		const string TYPE = "Type";
		const string YEAR = "Year";
		const string MONTH = "Month";
		const string NOTES = "Notes";

		public BaseRecord() { }

		public string BaseId {
			get => GetField<string>(BASE_ID);
			set => SetField(BASE_ID, value);
		}

		public int Year {
			get => GetField<int>(YEAR);
			set => SetField(YEAR, value);
		}

		public int Month {
			get => GetField<int>(MONTH);
			set => SetField(MONTH, value);
		}

		public string Notes {
			get => GetField<string>(NOTES);
			set => SetField(NOTES, value);
		}
	}
}
