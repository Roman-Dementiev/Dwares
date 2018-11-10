using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using ACE.Models;


namespace ACE
{
	public static partial class AppData
	{
#if DEBUG
		const StorageLocation kStorageLocation = StorageLocation.Pictures;
#else
		const StorageLocation kStorageLocation  = StorageLocation.AppData;
#endif
		const string kFilename = "ACE.json";
		const string kBackupFilename = "ACE.backup.json";
		const int kVersion1 = 1;

		const string ACEAddress = "10162 Bustleton Ave\nPhiladelphia, PA 19116";

		class ContactRec
		{
			public ContactType ContactType { get; set; }
			public string Name { get; set; }
			public string ShortName { get; set; }
			public string Phone { get; set; }
			public string AltPhone { get; set; }
			public string Address { get; set; }
			public string AltAddress { get; set; }
			public string Comment { get; set; }
			public string[] Tags { get; set; }
		}

		class PickupRec
		{
			public string ClientName { get; set; }
			public string ClientPhone { get; set; }
			public string OfficeName { get; set; }
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

		class Convert
		{
			public Action<ContactRec> ConvertContact { get; set; }
			public Action<PickupRec> ConvertPickup { get; set; }
		}

		static async Task<Json> LoadJsonAsync(string filename)
		{
			var folder = await DeviceStorage.GetFolder(kStorageLocation);
			string text = await folder.ReadTextAsync(filename);

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
				//json.Contacts = new ContactRec[] {
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Alla", Phone = "267-938-1300" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Semen", Phone = "215-715-8551" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Oleg", Phone = "267-255-0268" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Pavel", Phone = "267-761-7237" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Alexander", Phone = "267-679-5955" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Volodya", Phone = "267-469-7961" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Carlos", Phone = "267-269-8448" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "Rita", Phone = "267-778-7146" },
				//	new ContactRec { ContactType = ContactType.ACE, Name = "ACE", Phone = "267-709-9702" , Address =  ACEAddress },

				//	new ContactRec { ContactType = ContactType.Office, Name = "Temple University Hospital", ShortName = "Temple Univ", Phone = "(215) 707-2000", Address =  "3401 N Broad St\nPhiladelphia, PA 19140" },
				//	new ContactRec { ContactType = ContactType.Office, Name = "Temple University Boyer Pavillion", ShortName = "Temple Boyer", Phone = "(215) 707-6000", Address =  "3509 N Broad St\nPhiladelphia, PA 19140" },
				//	new ContactRec { ContactType = ContactType.Office, Name = "Einstein Physicians Broad Street", ShortName = "Einstein Broad St", Phone = "(215) 457-7700", Address =  "4817 North Broad Street\nPhiladelphia, PA 19141" },
				//	new ContactRec { ContactType = ContactType.Office, Name = "Einstein Physicians Old York Road", ShortName = "Einstein Old York Rd", Phone = "(215) 924-1234", Address =  "5325  North Broad Street\nPhiladelphia, PA 19141" },
				//	new ContactRec { ContactType = ContactType.Office, Name = "Einstein Medical Center", ShortName = "Einstein Frankford", Phone = "(215) 456-7890", Address =  "7131 Frankford Ave\nPhiladelphia, PA 19135" },
				//	new ContactRec { ContactType = ContactType.Office, Name = "Einstein Physicians Mayfair ", ShortName = "Einstein Mayfair", Phone = "(215) 332-4164", Address =  "5501 Old York Rd\n Philadelphia, PA 19141" }
				//};

				//await SaveJsonAsync(json, filename);
			}

			return json;
		}
	
		static async Task SaveJsonAsync(Json json, string filename)
		{
			try {
				var folder = await DeviceStorage.GetFolder(kStorageLocation);
				var serializer = new JsonSerializer {
					NullValueHandling = NullValueHandling.Ignore,
					Formatting = Formatting.Indented
				};

				using (var sw = new StringWriter())
				using (var writer = new JsonTextWriter(sw)) {
					serializer.Serialize(writer, json);
					var text = sw.ToString();

					await folder.WriteTextAsync(filename, text);
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
		}

		//static void ConvertType(ContactRec rec)
		//{
		//	switch ((int)rec.ContactType) {
		//	case 0:
		//		if (rec.Name == "ACE") {
		//			rec.ContactType = ContactType.Address;
		//		} else {
		//			rec.ContactType = ContactType.Member;
		//		}
		//		break;
		//	case 1:
		//		rec.ContactType = ContactType.Client;
		//		break;
		//	case 2:
		//		rec.ContactType = ContactType.Office;
		//		break;
		//	case 3:
		//		rec.ContactType = ContactType.Address;
		//		break;
		//	}
		//}


		public static async Task LoadAsync(string filename = null) => await LoadAsync(filename, null);

		private static async Task LoadAsync(string filename, Convert convert)
		{
			//	await ClearSchedule();

			var contacts = Contacts;
			var pickups = Pickups;
			var route = Route;

			await ClearSchedule(false);
			contacts.Clear();

			var json = await LoadJsonAsync(filename ?? kFilename);
			foreach (var rec in json.Contacts) {
				if (convert != null) {
					convert.ConvertContact?.Invoke(rec);
				}
				var newContact = new Contact {
					ContactType = rec.ContactType,
					Name = rec.Name,
					ShortName = rec.ShortName,
					Phone = rec.Phone,
					AltPhone = rec.AltPhone,
					Address = rec.Address,
					AltAddress = rec.AltAddress,
					Comment = rec.Comment
				};
				if (rec.Tags != null) {
					newContact.AddTags(rec.Tags);
				}
				AddContact(contacts, newContact);
			}

			foreach (var rec in json.Pickups)
			{
				if (convert != null) {
					convert.ConvertPickup?.Invoke(rec);
				}

				var client = GetContact(rec.ClientName, rec.ClientPhone);
				if (client == null)
					continue;

				var office = GetContact(rec.OfficeName, rec.OfficeName);
				//if (office == null)
				//	continue;

				var pickup = new Pickup(client, office, rec.PickupTime, rec.AppoitmentTime);
				AddPickup(pickups, pickup);
				AddRouteRun(route, pickup);
				
			}

			if (convert != null) {
				await SaveAsync(filename);
			}
		}

		public static async Task SaveAsync(string filename = null)
		{
			var contactCount = AppData.Contacts.Count;
			var contacts = new ContactRec[contactCount];
			for (int i = 0; i < contactCount; i++) {
				var c = AppData.Contacts[i];
				contacts[i] = new ContactRec {
					ContactType = c.ContactType,
					Name = c.Name,
					ShortName = c.ShortName,
					Phone = c.Phone,
					AltPhone = c.AltPhone,
					Address = c.Address,
					AltAddress = c.AltAddress,
					Tags = c.GetTags(),
					Comment = c.Comment
				};
			}

			var pickupCount = AppData.Pickups.Count;
			var pickups = new PickupRec[pickupCount];
			for (int i = 0; i < pickupCount; i++) {
				var p = AppData.Pickups[i];
				pickups[i] = new PickupRec {
					ClientName = p.ClientName,
					ClientPhone = p.Client.Phone,
					OfficeName = p.Office.Name,
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

			await SaveJsonAsync(json, filename ?? kFilename);
		}

		public static async Task BackupAsync()
		{
			await SaveAsync(kBackupFilename);
		}

		public static async Task RestoreAsync()
		{
			await LoadAsync(kBackupFilename);
			await SaveAsync();
		}
	}
}
