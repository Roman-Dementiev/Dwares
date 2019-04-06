using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Rookie.Models;
using Dwares.Dwarf;

namespace Dwares.Rookie.Bases
{
	public class TripBase : AirBase, ITripDataSource
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

		public async Task<TripRecord[]> ListTrips()
		{
			var list = await TripsTable.ListRecords();
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
			var records = list.Records;
			if (records.Length == 0)
				return null;

			var lastRecord = records[0];
			for (int i = 0; i < records.Length; i++) {
				if (records[i].StartTime > lastRecord.StartTime) {
					lastRecord = records[i];
				}
			}
			return lastRecord;
		}

		//public async Task CopyVendors(MainBase mainBase)
		//{
		//	await mainBase.VendorsTable.CopyRecords(VendorsTable, "Brand", "Branch", "Location");
		//}
	}

}
