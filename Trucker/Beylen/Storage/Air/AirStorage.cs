using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;
using Beylen.Models;
using System.Collections.Generic;

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
		public RouteTable RouteTable => MainBase.RouteTable;
		public PropertiesTable PropertiesTable => MainBase.PropertiesTable;

		public async Task Initialize()
		{
			MainBase = new MainBase(ApiKey, MainBaseId);
			await MainBase.Initialize();
		}

		public async Task LoadData()
		{
			await LoadProperties();
			await LoadProduce();
			await LoadContacts();
			await LoadCustomers();
			await LoadPlaces();
			await LoadInvoices();
			await LoadRoute();
		}

		public async Task LoadProperties()
		{
			var records = await PropertiesTable.ListRecords();
			foreach (var rec in records)
			{
				if (string.IsNullOrEmpty(rec.Name) || string.IsNullOrEmpty(rec.Value)) {
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
					AppScope.Instance.StartPoint = place;
				if (place.Tags == "endpoint")
					AppScope.Instance.EndPoint = place;
			}
		}

		public async Task LoadInvoices()
		{
			var invoices = AppScope.Instance.Invoices;
			var customers = AppScope.Instance.Customers;

			int ord = 0;
			var records = await InvoicesTable.ListRecords(sortField: InvoiceRecord.SEQ);
			foreach (var rec in records)
			{
				var customer = customers.GetByCodeName(rec.Customer);
				if (customer == null) {
					Debug.Print($"## AirStorage.LoadInvoices(): Unknown customer '{rec.Customer}'");
					continue;
				}

				var invoice = new Invoice {
					RecordId = rec.Id,
					Seq = rec.Seq,
					Ordinal = ++ord,
					Date = rec.Date,
					Number = rec.Number,
					Customer = customer,
					Notes = rec.Notes
				};
				invoices.Add(invoice);
			}
		}

		public Dictionary<string, PropertyRecord> Properties { get; } = new Dictionary<string, PropertyRecord>();

		public string GetProperty(string name)
		{
			string value = null;
			if (Properties.ContainsKey(name)) {
				value = Properties[name].Value;
			}
			return value;
		}

		public async Task SetProperty(string name, string value)
		{
			if (Properties.ContainsKey(name)) {
				var rec = Properties[name];
				rec.Value = value;
				await PropertiesTable.UpdateRecord(rec, PropertyRecord.VALUE);
			} else {
				var rec = new PropertyRecord {
					Name = name,
					Value = value
				};
				rec = await PropertiesTable.CreateRecord(rec);
				Properties[name] = rec;
			}
		}
	}
}
