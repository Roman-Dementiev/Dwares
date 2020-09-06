using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;
using Beylen.Models;
using System.Collections.Generic;
using Xamarin.Forms;

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
		public ProduceTable ProduceTable => MainBase.ProduceTable;
		public ContactsTable ContactsTable => MainBase.ContactsTable;
		public CustomersTable CustomersTable => MainBase.CustomersTable;
		public PlacesTable PlacesTable => MainBase.PlacesTable;
		public InvoicesTable InvoicesTable => MainBase.InvoicesTable;
		public ArticlesTable ArticlesTable => MainBase.ArticlesTable;
		public RouteTable RouteTable => MainBase.RouteTable;
		public PropertiesTable PropertiesTable => MainBase.PropertiesTable;

		public async Task Initialize()
		{
			MainBase = new MainBase(ApiKey, MainBaseId);
			await MainBase.Initialize();
		}

		public async Task LoadData(string carId, bool initializing, bool resetProperties)
		{
			Properties.Clear();
			if (resetProperties) {
				await ClearProperties();
			} else {
				await LoadProperties();
			}

			await LoadProduce();
			await LoadContacts();
			await LoadCustomers();
			await LoadPlaces();
			await LoadInvoices(carId);
			await LoadRoute();
		}

		public async Task ClearProperties()
		{
			var records = await PropertiesTable.ListRecords();
			foreach (var rec in records) {
				await PropertiesTable.DeleteRecord(rec.Id);
			}
		}

		public async Task LoadProperties()
		{
			var records = await PropertiesTable.ListRecords();
			foreach (var rec in records)
			{
				if (string.IsNullOrEmpty(rec.Name) || string.IsNullOrEmpty(rec.Value) || Properties.ContainsKey(rec.Name)) {
					await PropertiesTable.DeleteRecord(rec.Id);
					continue;
				}

				Properties.Add(rec.Name, rec);
			}
		}

		public async Task LoadProduce()
		{
			var produce = AppScope.Instance.Produce;

			var records = await ProduceTable.ListRecords(sortField: ProduceRecord.NAME);
			foreach (var rec in records)
			{
				if (string.IsNullOrWhiteSpace(rec.Name)) {
					await PropertiesTable.DeleteRecord(rec.Id);
					continue;
				}

				var prod = new Produce {
					Name = rec.Name,
					Package = rec.Package,
					Cpp = rec.Cpp
				};

				produce.Add(prod);
			}
		}

		public async Task LoadContacts()
		{
			var contacts = AppScope.Instance.Contacts;

			var records = await ContactsTable.ListRecords(sortField: ContactRecord.SEQ);
			foreach (var rec in records)
			{
				if (string.IsNullOrWhiteSpace(rec.Name)) {
					await PropertiesTable.DeleteRecord(rec.Id);
					continue;
				}
				var contact = new Contact {
					RecordId = rec.Id,
					Name = rec.Name?.Trim(),
					Phone = rec.Phone?.Trim(),
					Info = rec.Info?.Trim()
				};
				contacts.Add(contact);
			}
		}

		public async Task LoadCustomers()
		{
			var contacts = AppScope.Instance.Contacts;
			var customers = AppScope.Instance.Customers;

			var records = await CustomersTable.ListRecords(sortField: CustomerRecord.CODE_NAME);
			foreach (var rec in records)
			{
				if (string.IsNullOrWhiteSpace(rec.CodeName)) {
					await PropertiesTable.DeleteRecord(rec.Id);
					continue;
				}


				var customer = new Customer {
					RecordId = rec.Id,
					CodeName = rec.CodeName?.Trim(),
					RealName = rec.RealName?.Trim(),
					Alias = rec.Alias?.Trim(),
					Tags = rec.Tags?.Trim(),
					Address = rec.Address?.Trim(),
					Phone = rec.Phone?.Trim()
				};

				var contactName = rec.ContactName?.Trim();
				var contactPhone = rec.ContactPhone?.Trim();
				if (!string.IsNullOrEmpty(contactName) && !string.IsNullOrEmpty(contactPhone)) {
					//var contact = new Contact {
					//	Name = contactName,
					//	Phone = contactPhone,
					//	Info = rec.RealName
					//};
					var contact = new Contact {
						Name = rec.CodeName,
						Phone = contactPhone,
						Info = contactName
					};
					contacts.Add(contact);
					customer.Contact = contact;

				}


				customers.Add(customer);
			}
		}

		public async Task LoadPlaces()
		{
			var places = AppScope.Instance.Places;

			var records = await PlacesTable.ListRecords();
			foreach (var rec in records) {
				if (string.IsNullOrWhiteSpace(rec.CodeName)) {
					await PropertiesTable.DeleteRecord(rec.Id);
					continue;
				}
				var place = new Place {
					RecordId = rec.Id,
					CodeName = rec.CodeName?.Trim(),
					RealName = rec.RealName?.Trim(),
					Tags = rec.Tags?.Trim(),
					Address = rec.Address?.Trim(),
				};
				places.Add(place);

				if (place.Tags == "startpoint")
					AppScope.Instance.Route.StartPoint = place;
				if (place.Tags == "endpoint")
					AppScope.Instance.Route.EndPoint = place;
			}
		}

		public Dictionary<string, PropertyRecord> Properties { get; } = new Dictionary<string, PropertyRecord>();

		static string PropertyKey(string name, Car car)
		{
			return car == null ? name : $"{name}:{car.Id}";
		}


		async Task<PropertyRecord> GetPropertyRecord(string key)
		{
			PropertyRecord rec = null;

			if (Properties.ContainsKey(key)) {
				rec = Properties[key];
				try {
					rec = await PropertiesTable.GetRecord(rec.Id);
				}
				catch (Exception exc) {
					Debug.ExceptionCaught(exc);
					rec = null;
				}
				if (rec == null) {
					Properties.Remove(key);
				}
			} else {
				try {
					var result = await PropertiesTable.FilterRecords($"Name = \"{key}\"");
					if (result?.Records != null && result.Records.Length > 0) {
						rec = result.Records[0];

						if (rec != null) {
							Properties[key] = rec;
						}
					}
				}
				catch (Exception exc) {
					Debug.ExceptionCaught(exc);
					rec = null;
				}
			}

			return rec;
		}

		public async Task<string> GetProperty(string name, Car car)
		{
			PropertyRecord rec = null;

			if (car != null) {
				var key = PropertyKey(name, car);
				rec = await GetPropertyRecord(key);
				if (rec != null)
					return rec.Value;
			}

			rec = await GetPropertyRecord(name);
			return rec?.Value;
		}

		public async Task SetProperty(string name, Car car, string value)
		{
			var key = PropertyKey(name, car);
			var rec = await GetPropertyRecord(key);

			try {
				if (rec != null) {
					rec.Value = value;
					await PropertiesTable.UpdateRecord(rec, PropertyRecord.VALUE);
				} else {
					rec = new PropertyRecord {
						Name = key,
						Value = value
					};
					rec = await PropertiesTable.CreateRecord(rec);
					Properties[name] = rec;
				}
			} catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
		}
	}
}
