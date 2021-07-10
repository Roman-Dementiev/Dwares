using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using Drive.Models;


namespace Drive.Storage.Air
{
	public class AirStorage : AirClient, IAppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AirStorage));

		public AirStorage()
		{
			//Debug.EnableTracing(@class);
			AirClient.Instance = this;
		}

		public AirBase CurrentDb { get; private set; }
		public GasTable GasTable { get; private set; }


		List<string> BaseIds { get; } = new List<string>();

		public async Task Initialize()
		{
			var ApiKey = Settings.ApiKey;
			var BaseId = Settings.BaseId;

			CurrentDb = new AirBase(ApiKey, BaseId);
			await CurrentDb.Initialize();

			GasTable = new GasTable(CurrentDb);
			await GasTable.Initialize();

			var basesTable = new BasesTable(CurrentDb);
			await basesTable.Initialize();

			await basesTable.ForEach((rec) => {
					if (rec.BaseId != CurrentDb.BaseId) {
						BaseIds.Add(rec.BaseId);
					} 
				},
				sortField: BaseRecord.START_DATE
			);
			BaseIds.Add(CurrentDb.BaseId);
		}

		public Task LoadData()
		{
			return Task.CompletedTask;
		}

		public new void Dispose()
		{
			base.Dispose();
		}
	}
}
