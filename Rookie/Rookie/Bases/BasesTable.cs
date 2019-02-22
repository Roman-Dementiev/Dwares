using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


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

		public async Task<BaseRecord[]> ListBases(QyeryBuilder queryBuilder = null)
		{
			var list = await base.ListRecords(queryBuilder);
			return list.Records;
		}

		public async Task<BaseRecord> AddRecord(string baseId, int year, int month, string notes)
		{
			var record = new BaseRecord {
				BaseId = baseId,
				Year = year,
				Month = month,
				Notes = notes
			};
			record.CopyPropertiesToFields();

			return await CreateRecord(record.Fields);
		}
	}

	public class BaseRecord : AirRecord
	{
		const string BASE_ID = "BaseId";
		const string YEAR = "Year";
		const string MONTH = "Month";
		const string NOTES = "NOTES";

		public string BaseId { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public string Notes { get; set; }

		public override void CopyFieldsToProperties()
		{
			BaseId = GetField<string>("BaseId");
			Year = GetField<int>("Year");
			Month = GetField<int>("Month");
			Notes = GetField<string>("Notes");
		}

		public override void CopyPropertiesToFields()
		{
			Fields[BASE_ID] = BaseId;
			Fields[YEAR] = Year;
			Fields[MONTH] = Month;
			Fields[NOTES] = Notes;
		}
	}
}
