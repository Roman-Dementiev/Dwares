using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;

namespace Drive.Storage.Air
{
	public class MainBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(MainBase));

		public MainBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);

			BasesTable = new BasesTable(this);
		}

		public BasesTable BasesTable { get; }

		public override async Task Initialize()
		{
			await BasesTable.Initialize();
		}
	}
}
