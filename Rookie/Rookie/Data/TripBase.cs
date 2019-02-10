using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;
using System.Collections.Generic;

namespace Dwares.Rookie.Data
{
	public class TripBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(TripBase));
		public const string TableTrips = "Trips";
		const string TableVendors = "Vendors";


		public TripBase(string apiKey, string baseId, int year, int month) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);
			Year = year;
			Month = month;
		}

		public int Year { get; }
		public int Month { get; }

		public async Task<TripRecord[]> ListTrips(QyeryBuilder queryBuilder = null)
		{
			var list = await ListRecords<TripRecord>(TableTrips, queryBuilder);
			return list.Records;
		}

		public async Task CopyVendors(MainBase mainBase)
		{
			//var vendors = await mainBase.ListVendors();

			//foreach (var record in vendors) {
			//	var fields = record.GetFields("Brand", "Branch", "Location");
			//	await CreateRecord<BaseRecord>(TableVendors, fields);
			//}
			await mainBase.CopyRecords<BaseRecord>(TableVendors, this, "Brand", "Branch", "Location");
		}
	}

	public class TripRecord : AirRecord
	{
		int TripNumber { get; set; }

		public override void CopyFieldsToProperties()
		{
			TripNumber = GetField<int>("");
		}

		public override void CopyPropertiesToFields()
		{
			Fields["Trip #"] = TripNumber;
		}
	}
}
