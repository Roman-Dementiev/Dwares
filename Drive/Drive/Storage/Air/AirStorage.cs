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
		//static ClassRef @class = new ClassRef(typeof(AirTable));
		const string ApiKey = "keyn9n03pU21UkxTg";
		const string MainBaseId = "appPNHOUAm5jLrD5q";


		public AirStorage()
		{
			//Debug.EnableTracing(@class);

			AirClient.Instance = this;
		}

	
		public MainBase MainBase { get; private set; }
		public ContactsBase ContactsBase { get; private set; }

		public async Task Initialize()
		{
			MainBase = new MainBase(ApiKey, MainBaseId);
			await MainBase.Initialize();

			var baseRecords = await MainBase.BasesTable.ListBases();
			foreach (var record in baseRecords) {
				if (record.Year == 0) {
					if (record.Month == 2) {
						ContactsBase = new ContactsBase(ApiKey, record.BaseId);
					}
				}
			}
		}

		public Task LoadContacts(IList<IContact> contacts)
		{
			// TODO
			return Task.CompletedTask;
		}

		public Task LoadSchedule(IList<Ride> schedule)
		{
			// TODO
			return Task.CompletedTask;
		}

		public Task SaveContacts(IList<IContact> contacts)
		{
			// TODO
			return Task.CompletedTask;
		}

		public Task SaveSchedule(IList<Ride> schedule)
		{
			// TODO
			return Task.CompletedTask;
		}

	}
}
