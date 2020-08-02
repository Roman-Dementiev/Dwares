using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;
using Beylen.Models;

namespace Beylen.Storage.Air
{
	public class AirStorage : AirClient, IAppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AirStorage));
		const string ApiKey = "keyn9n03pU21UkxTg";
		const string MainBaseId = "appZpwH5Y6T6RbRHK";

		public AirStorage()
		{
			//Debug.EnableTracing(@class);
			AirClient.Instance = this;
		}

		public MainBase MainBase { get; private set; }

		public async Task Initialize()
		{
			MainBase = new MainBase(ApiKey, MainBaseId);
			await MainBase.Initialize();
		}

		public async Task LoadContacts()
		{
			var contacts = AppScope.Instance.Contacts;

			var records = await MainBase.ContactsTable.ListRecords();
			foreach (var rec in records)
			{
				if (string.IsNullOrWhiteSpace(rec.Name)) {
					//TODO: delete empty record
					continue;
				}
				var contact = new Contact {
					RecordId = rec.Id,
					Name = rec.Name,
					Phone = rec.Phone
				};
				contacts.Add(contact);
			}
		}

		public async Task LoadCustomers()
		{
			var customers = AppScope.Instance.Customers;

			var records = await MainBase.CustomersTable.ListRecords();
			foreach (var rec in records)
			{
				if (string.IsNullOrWhiteSpace(rec.ShortName)) {
					//TODO: delete empty record
					continue;
				}
				var customer = new Customer {
					RecordId = rec.Id,
					Name = rec.ShortName,
					Tags = rec.Tags,
					FullName = rec.FullName,
					Address = rec.Address,
					Phone = rec.Phone,
					ContactName = rec.ContactName,
					ContactPhone = rec.ContactPhone
				};
				customers.Add(customer);
			}
		}
	}
}
