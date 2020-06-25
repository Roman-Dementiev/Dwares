using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Lost.Storage.Air
{
	public class DataBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(DataBase));

		public DataBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);

			DailyTable = new DailyTable(this);
			PropertiiesTable = new PropertiiesTable(this);
		}

		public DailyTable DailyTable { get; }
		public PropertiiesTable PropertiiesTable { get; }

		public override async Task Initialize()
		{
			await DailyTable.Initialize();
			await PropertiiesTable.Initialize();


		}
	}
}
