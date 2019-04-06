using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;


namespace Dwares.Rookie.Bases
{
	public class BasesTable : AirTable<BaseRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(BasesTable));

		public BasesTable(AirBase airBase) :
			base(airBase, "Bases")
		{
			//Debug.EnableTracing(@class);
		}

		//public async Task<BaseRecord[]> ListBases()
		//{
		//	var list = await ListRecords();
		//	return list;
		//}

		//public async Task<BaseRecord> AddRecord(string baseId, int year, int month, string notes)
		//{
		//	var record = new BaseRecord {
		//		BaseId = baseId,
		//		Year = year,
		//		Month = month,
		//		Notes = notes
		//	};

		//	return await CreateRecord(record);
		//}
	}

	public class BaseRecord : AirRecord
	{
		const string BASE_ID = "BaseId";
		const string YEAR = "Year";
		const string MONTH = "Month";
		const string NOTES = "Notes";

		public BaseRecord() { }
		//public BaseRecord(IDataRecord record) : base(record) { }

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
