using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using Buffy.Models;
using System.Collections.Generic;

namespace Buffy.Storage.Air
{
	public class AirStorage : AirClient, IAppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AirStorage));
		//const string ApiKey = "keyn9n03pU21UkxTg";
		//const string BaseId = "appzMiVx1IkpFjato";
		const int MaxPeriod = 30; // days

		public AirStorage()
		{
			//Debug.EnableTracing(@class);
			AirClient.Instance = this;

		}

		public AirBase DataBase { get; private set; }
		public GasTable GasTable { get; private set; }
		public BasesTable BasesTable { get; private set; }

		public async Task Initialize()
		{
			var ApiKey = Settings.ApiKey;
			var BaseId = Settings.BaseId;

			DataBase = new AirBase(ApiKey, BaseId);
			await DataBase.Initialize();

			GasTable = new GasTable(DataBase);
			await GasTable.Initialize();

			BasesTable = new BasesTable(DataBase);
			await BasesTable.Initialize();
		}


		public async Task LoadData()
		{
			await LoadFuelings(30);
			await LoadSummaries();
		}

		public async Task LoadFuelings(int days)
		{
			await LoadFuelings(GasTable, App.Fuelings, $"DATETIME_DIFF(TODAY(), {{Date}}, 'days') < {days + 1}");
		}

		async Task LoadFuelings(GasTable table, IList<Fueling> fuelings, string filter = null)
		{
			await table.ForEach((rec) => {
				var fueling = new Fueling {
					Id = rec.Id,
					Date = rec.Date,
					Vendor = App.GetVendor(rec.Vendor),
					State = rec.State,
					Gallons = rec.Gallons,
					Price = rec.Price,
					Total = rec.Total
				};
				fuelings.Add(fueling);
			},
			filter, sortField: "Date");
		}

		public async Task LoadSummaries()
		{
#if false
			var fuelings = new List<Fueling>();
			await LoadFuelings(GasTable, fuelings);

			var summary = App.Summary;
			foreach (var fueling in fuelings) {
				summary.Add(fueling);
			}
#else
			var ApiKey = Settings.ApiKey;
			await BasesTable.ForEach(async (rec) => {
				if (rec.BaseId != DataBase.BaseId) {
					var dataBase = new AirBase(ApiKey, rec.BaseId);
					await dataBase.Initialize();

					var gasTable = new GasTable(dataBase);
					await gasTable.Initialize();

					await LoadSummaries(gasTable);
				}
			});
			await LoadSummaries(GasTable);
#endif
		}

		public async Task LoadSummaries(GasTable table)
		{
			var fuelings = new List<Fueling>();
			await LoadFuelings(table, fuelings);

			var summary = App.Summary;
			foreach (var fueling in fuelings) {
				summary.Add(fueling);
			}
		}

		public async Task AddFueling(Fueling fueling)
		{
			var rec = new GasRecord {
				Date = fueling.Date,
				Vendor = fueling.Vendor.Name,
				State = fueling.State,
				Gallons= fueling.Gallons,
				Price = fueling.Price,
				Total = fueling.Total
			};

			var fields = new List<string>();
			fields.Add(GasRecord.DATE);
			fields.Add(GasRecord.VENDOR);
			fields.Add(GasRecord.STATE);
			if (rec.Gallons > 0)
				fields.Add(GasRecord.GALLONS);
			if (rec.Price > 0)
				fields.Add(GasRecord.PRICE);
			if (rec.Total > 0)
				fields.Add(GasRecord.TOTAL);

			rec = await GasTable.CreateRecord(rec, fields.ToArray());
			fueling.Id = rec.Id;
		}
		public async Task UpdateFueling(Fueling oldFueling, Fueling newFueling)
		{
			var rec = new GasRecord {
				Id = oldFueling.Id,
				Date = newFueling.Date,
				Vendor = newFueling.Vendor.Name,
				State = newFueling.State,
				Gallons = newFueling.Gallons,
				Price = newFueling.Price,
				Total = newFueling.Total
			};

			var fields = new List<string>();
			fields.Add(GasRecord.DATE);
			fields.Add(GasRecord.VENDOR);
			fields.Add(GasRecord.STATE);
			if (newFueling.Gallons != oldFueling.Gallons)
				fields.Add(GasRecord.GALLONS);
			if (newFueling.Price != oldFueling.Price)
				fields.Add(GasRecord.PRICE);
			if (newFueling.Total != oldFueling.Total)
				fields.Add(GasRecord.TOTAL);

			rec = await GasTable.UpdateRecord(rec, fields.ToArray());
		}

		public async Task DeleteFueling(Fueling fueling)
		{
			await GasTable.DeleteRecord(fueling.Id);
		}
	}
}
