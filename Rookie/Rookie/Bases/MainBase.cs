using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


namespace Dwares.Rookie.Bases
{
	public class MainBase
	{
		//static ClassRef @class = new ClassRef(typeof(MainBase));

		public MainBase(string apiKey, string baseId)
		{
			//Debug.EnableTracing(@class);C:\Dev\Dwares\Rookie\Rookie\ViewModels\AddAccountViewModel.cs

			DataBase = new AirBase(apiKey, baseId);

			BasesTable = new BasesTable(DataBase);
			PropertiiesTable = new PropertiiesTable(DataBase);
			VendorsTable = new VendorsTable(DataBase);
		}

		public async Task Initialize()
		{
			await BasesTable.Initialize();
			await PropertiiesTable.Initialize();
			await VendorsTable.Initialize();
		}

		public AirBase DataBase { get; }
		public BasesTable BasesTable { get; }
		public PropertiiesTable PropertiiesTable { get; }
		public VendorsTable VendorsTable { get; }

		public string ApiKey => DataBase?.ApiKey;

		//public async Task<BaseRecord[]> ListBaseRecords(QyeryBuilder queryBuilder = null)
		//{
		//	var list = await BasesTable.ListRecords(queryBuilder);
		//	return list.Records;
		//}

		//public async Task<BaseRecord> AddBaseRecord(string baseId, int year, int month, string notes)
		//{
		//	var record = new BaseRecord {
		//		BaseId = baseId,
		//		Year = year,
		//		Month = month,
		//		Notes = notes
		//	};
		//	record.CopyPropertiesToFields();

		//	return await BasesTable.CreateRecord(record.Fields);
		//}

		//public async Task<VendorRecord[]> ListVendors(QyeryBuilder queryBuilder = null)
		//{
		//	var list = await BasesTable.ListRecords<VendorRecord>(queryBuilder);
		//	return list.Records;
		//}

	}

}
