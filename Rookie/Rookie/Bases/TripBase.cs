using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


namespace Dwares.Rookie.Bases
{
	public class TripBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(TripBase));

		public TripBase(string apiKey, string baseId, int year, int month) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);
			Year = year;
			Month = month;

			TripsTable = new TripsTable(this);
			LeaseTable = new LeaseTable(this);
			PeriodsTable = new PeriodsTable(this);
			VendorsTable = new VendorsTable(this);
		}


		public int Year { get; }
		public int Month { get; }
		public TripsTable TripsTable { get; }
		public LeaseTable LeaseTable { get; }
		public PeriodsTable PeriodsTable { get; }
		public VendorsTable VendorsTable { get; }

		public async Task<TripRecord[]> ListTrips(QyeryBuilder queryBuilder = null)
		{
			var list = await TripsTable.ListRecords(queryBuilder);
			return list.Records;
		}

		public async Task<PeriodRecord> GetPeriod(string recordId)
		{
			var record = await PeriodsTable.GetRecord(recordId);
			return record;
		}

		public async Task<PeriodRecord> GetLastCreatedPeriod()
		{
			var list = await PeriodsTable.ListRecords();
			if (list.Records.Length == 0)
				return null;

			var lastRecord = list.Records[0];
			for (int i = 0; i < list.Records.Length; i++) {
				if (list.Records[i].CreatedTime > lastRecord.CreatedTime) {
					lastRecord = list.Records[i];
				}
			}
			return lastRecord;
		}

		public async Task CopyVendors(MainBase mainBase)
		{
			//var vendors = await mainBase.ListVendors();

			//foreach (var record in vendors) {
			//	var fields = record.GetFiVendorelds("Brand", "Branch", "Location");
			//	await CreateRecord<BaseRecord>(TableVendors, fields);
			//}
			await mainBase.VendorsTable.CopyRecords(VendorsTable, "Brand", "Branch", "Location");
		}
	}

}
