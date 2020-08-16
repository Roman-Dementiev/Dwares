using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;
using Beylen.Models;


namespace Beylen.Storage.Air
{
	public partial class AirStorage : AirClient, IAppStorage
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
		public BasesTable BasesTable => MainBase.BasesTable;
		public ContactsTable ContactsTable => MainBase.ContactsTable;
		public CustomersTable CustomersTable => MainBase.CustomersTable;
		public PlacesTable PlacesTable => MainBase.PlacesTable;
		public RouteTable RouteTable => MainBase.RouteTable;


		public async Task Initialize()
		{
			MainBase = new MainBase(ApiKey, MainBaseId);
			await MainBase.Initialize();
		}

		public async Task LoadContacts()
		{
			var contacts = AppScope.Instance.Contacts;

			var records = await ContactsTable.ListRecords();
			foreach (var rec in records)
			{
				if (string.IsNullOrWhiteSpace(rec.Name)) {
					//TODO: delete empty record
					continue;
				}
				var contact = new Contact {
					RecordId = rec.Id,
					Name = rec.Name?.Trim(),
					Phone = rec.Phone?.Trim()
				};
				contacts.Add(contact);
			}
		}

		public async Task LoadCustomers()
		{
			var customers = AppScope.Instance.Customers;

			var records = await CustomersTable.ListRecords();
			foreach (var rec in records)
			{
				if (string.IsNullOrWhiteSpace(rec.CodeName)) {
					//TODO: delete empty record
					continue;
				}
				var customer = new Customer {
					RecordId = rec.Id,
					CodeName = rec.CodeName?.Trim(),
					FullName = rec.FullName?.Trim(),
					Tags = rec.Tags?.Trim(),
					Address = rec.Address?.Trim(),
					Phone = rec.Phone?.Trim(),
					ContactName = rec.ContactName?.Trim(),
					ContactPhone = rec.ContactPhone?.Trim()
				};
				customers.Add(customer);
			}
		}

		public async Task LoadPlaces()
		{
			var places = AppScope.Instance.Places;

			var records = await PlacesTable.ListRecords();
			foreach (var rec in records) {
				if (string.IsNullOrWhiteSpace(rec.CodeName)) {
					//TODO: delete empty record
					continue;
				}
				var place = new Place {
					RecordId = rec.Id,
					CodeName = rec.CodeName?.Trim(),
					FullName = rec.FullName?.Trim(),
					Tags = rec.Tags?.Trim(),
					Address = rec.Address?.Trim(),
				};
				places.Add(place);

				if (place.Tags == "startpoint")
					AppScope.Instance.StartPoint = place;
				if (place.Tags == "endpoint")
					AppScope.Instance.EndPoint = place;
			}
		}
	}
}
