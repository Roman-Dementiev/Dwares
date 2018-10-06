using System;
using System.IO;
using System.Threading.Tasks;
using ACE.Services;
using Newtonsoft.Json;
using Dwares.Dwarf;


namespace ACE.Models
{
	public static partial class AppData
	{
#if DEBUG
		const StorageLocation kStorageLocation = StorageLocation.Pictures;
#else
		const StorageLocation kStorageLocation  = StorageLocation.AppData;
#endif
		const string kFilename = "ACE.json";
		const int kVersion1 = 1;

		struct ContactRec
		{
			public ContactType ContactType { get; set; }
			public string Name { get; set; }
			public string Phone { get; set; }
			public string AltPhone { get; set; }
			public string Address { get; set; }
			public string AltAddress { get; set; }
			public string Comment { get; set; }
		}

		struct PickupRec
		{
			public string ClientPhone { get; set; }
			public string OfficePhone { get; set; }
			public DateTime PickupTime { get; set; }
			public DateTime AppoitmentTime { get; set; }
		}

		struct Json
		{
			public int Version { get; set; }
			public ContactRec[] Contacts { get; set; }
			public PickupRec[] Pickups { get; set; }
		}

		static async Task<Json> LoadJsonAsync()
		{
			var folder = await DeviceStorage.GetFolder(kStorageLocation);
			string text = await folder.ReadTextAsync(kFilename);

			Json json;
			try {
				json = JsonConvert.DeserializeObject<Json>(text);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				json = new Json {
					Version = kVersion1,
					Pickups = new PickupRec[0],
					Contacts = new ContactRec[0]
				};
			}

			if (json.Contacts.Length == 0) {
				json.Contacts = new ContactRec[] {
					new ContactRec { ContactType = ContactType.Company, Name = "Alla", Phone = "267-938-1300" },
					new ContactRec { ContactType = ContactType.Company, Name = "Semen", Phone = "215-715-8551" },
					new ContactRec { ContactType = ContactType.Company, Name = "Oleg", Phone = "267-255-0268" },
					new ContactRec { ContactType = ContactType.Company, Name = "Pavel", Phone = "267-761-7237" },
					new ContactRec { ContactType = ContactType.Company, Name = "Alexander", Phone = "267-679-5955" },
					new ContactRec { ContactType = ContactType.Company, Name = "Volodya", Phone = "267-469-7961" },
					new ContactRec { ContactType = ContactType.Company, Name = "Carlos", Phone = "267-269-8448" },
					new ContactRec { ContactType = ContactType.Company, Name = "Rita", Phone = "267-778-7146" },
					new ContactRec { ContactType = ContactType.Company, Name = "ACE", Phone = "267-709-9702" }
				};

				await SaveJsonAsync(json);
			}

			return json;
		}

		static async Task SaveJsonAsync(Json json)
		{
			try {
				var folder = await DeviceStorage.GetFolder(kStorageLocation);
				var serializer = new JsonSerializer();
				serializer.NullValueHandling = NullValueHandling.Ignore;
				serializer.Formatting = Formatting.Indented;

				using (var sw = new StringWriter())
				using (var writer = new JsonTextWriter(sw)) {
					serializer.Serialize(writer, json);
					var text = sw.ToString();

					await folder.WriteTextAsync(kFilename, text);
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
		}


		public static async Task LoadAsync()
		{
			var json = await LoadJsonAsync();
			var contacts = Contacts;
			var pickups = Pickups;

			foreach (var rec in json.Contacts) {
				AddContactInternal(contacts, new Contact(rec.ContactType) {
					Name = rec.Name,
					Phone = rec.Phone,
					AltPhone = rec.AltPhone,
					Address = rec.Address,
					AltAddress = rec.AltAddress,
					Comment = rec.Comment
				});
			}

			foreach (var rec in json.Pickups)
			{
				var client = AppData.GetContactByPhone(rec.ClientPhone);
				if (client == null)
					continue;

				var office = AppData.GetContactByPhone(rec.OfficePhone);
				if (office == null)
					continue;

				AddPickupInternal(pickups, new Pickup {
					Client = client,
					Office = office,
					PickupTime = rec.PickupTime,
					AppoitmentTime = rec.AppoitmentTime
				});
			}

		}

		public static async Task SaveAsync()
		{
			var contactCount = AppData.Contacts.Count;
			var contacts = new ContactRec[contactCount];
			for (int i = 0; i < contactCount; i++) {
				var c = AppData.Contacts[i];
				contacts[i] = new ContactRec {
					ContactType = c.ContactType,
					Name = c.Name,
					Phone = c.Phone,
					AltPhone = c.AltPhone,
					Address = c.Address,
					AltAddress = c.AltAddress,
					Comment = c.Comment
				};
			}

			var pickupCount = AppData.Pickups.Count;
			var pickups = new PickupRec[pickupCount];
			for (int i = 0; i < pickupCount; i++) {
				var p = AppData.Pickups[i];
				pickups[i] = new PickupRec {
					ClientPhone = p.Client.Phone,
					OfficePhone = p.Office.Phone,
					PickupTime = p.PickupTime,
					AppoitmentTime = p.AppoitmentTime
				};
			}

			var json = new Json{
				Version = kVersion1,
				Contacts = contacts,
				Pickups = pickups
			};

			await SaveJsonAsync(json);
		}
	}
}
