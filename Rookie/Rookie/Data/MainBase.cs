using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


namespace Dwares.Rookie.Data
{
	public class MainBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(MainBase));
		const string TableBases = "Bases";
		const string TableVendors = "Vendors";

		public MainBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);C:\Dev\Dwares\Rookie\Rookie\ViewModels\AddAccountViewModel.cs
		}

		public async Task<BaseRecord[]> ListBaseRecords(QyeryBuilder queryBuilder = null)
		{
			var list = await ListRecords<BaseRecord>(TableBases, queryBuilder);
			return list.Records;
		}

		public async Task<BaseRecord> AddBaseRecord(BaseRecord record)
		{
			record.CopyPropertiesToFields();
			return await CreateRecord<BaseRecord>(TableBases, record.Fields);
		}

		public async Task<VendorRecord[]> ListVendors(QyeryBuilder queryBuilder = null)
		{
			var list = await ListRecords<VendorRecord>(TableVendors, queryBuilder);
			return list.Records;
		}

	}

	public class BaseRecord : AirRecord
	{
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
			Fields["BaseId"] = BaseId;
			Fields["Year"] = Year;
			Fields["Month"] = Month;
			Fields["Notes"] = Notes;
		}
	}

	public class VendorRecord : AirRecord
	{
		public string Brand { get; set; }
		public string Branch { get; set; }
		public string Location { get; set; }
		//public string[] Category { get; set; }

		public override void CopyFieldsToProperties()
		{
			Brand = GetField<string>("Brand");
			Branch = GetField<string>("Branch");
			Location = GetField<string>("Location");
		}

		public override void CopyPropertiesToFields()
		{
			Fields["Brand"] = Brand;
			Fields["Branch"] = Branch;
			Fields["Location"] = Location;
		}
	}
}
