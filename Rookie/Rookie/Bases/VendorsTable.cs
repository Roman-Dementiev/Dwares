using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


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

		public async Task<VendorRecord[]> ListVendors()
		{
			var list = await ListRecords();
			return list.Records;
		}
	}


	public class VendorRecord : AirRecord
	{
		const string BRAND ="Brand";
		const string BRANCH = "Branch";
		const string LOCATION = "Location";

		public string Brand {
			get => GetField<string>(BRAND);
			set => SetField(BRAND, value);
		}

		public string Branch {
			get => GetField<string>(BRANCH);
			set => SetField(BRANCH, value);
		}

		public string Location {
			get => GetField<string>(LOCATION);
			set => SetField(LOCATION, value);
		}
	}
}
