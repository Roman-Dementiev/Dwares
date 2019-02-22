using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;


namespace Dwares.Rookie.Bases
{
	public class VendorsTable : AirTable<VendorRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(VendorsTable));

		public VendorsTable(AirBase airBase) :
			base(airBase, "Vendors")
		{
			//Debug.EnableTracing(@class);
		}

		public async Task<VendorRecord[]> ListVendors(QyeryBuilder queryBuilder = null)
		{
			var list = await base.ListRecords(queryBuilder);
			return list.Records;
		}
	}


	public class VendorRecord : AirRecord
	{
		const string BRAND ="Brand";
		const string BRANCH = "Branch";
		const string LOCATION = "Location";

		public string Brand { get; set; }
		public string Branch { get; set; }
		public string Location { get; set; }
		//public string[] Category { get; set; }

		public override void CopyFieldsToProperties()
		{
			Brand = GetField<string>(BRAND);
			Branch = GetField<string>(BRANCH);
			Location = GetField<string>(LOCATION);
		}

		public override void CopyPropertiesToFields()
		{
			Fields[BRAND] = Brand;
			Fields[BRANCH] = Branch;
			Fields[LOCATION] = Location;
		}
	}
}
