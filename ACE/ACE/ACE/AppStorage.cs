using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Dwares.Druid.Services;
using Dwares.Dwarf;
using ACE.Models;
using System.Collections.Generic;

namespace ACE
{
	public static class AppStorage
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

		class ScheduleRec
		{
			public string Type { get; set; }
			public string ClientName { get; set; }
			public string ClientPhone { get; set; }
			public string OfficeName { get; set; }
			public string OfficePhone { get; set; }
			public string OfficeAddress { get; set; }
			public DateTime PickupTime { get; set; }
			public DateTime DropoffTime { get; set; }
		}

		struct Json
		{
			public int Version { get; set; }
			public ContactRec[] Contacts { get; set; }
			public ScheduleRec[] Schedule { get; set; }
		}

		class Convert
		{
			public Action<ContactRec> ConvertContact { get; set; }
			public Action<ScheduleRec> ConvertSchedule { get; set; }
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
					Contacts = new ContactRec[0],
					Schedule = new ScheduleRec[0]
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
			if (filename == null) {
				filename = kFilename;
			}

			//	await ClearSchedule();

			var contacts = AppData.Contacts;
			var schedule = AppData.Schedule;
			var route = AppData.Route;

			schedule.Clear();
			contacts.Clear();

			var json = await LoadJsonAsync(filename);

			if (json.Contacts != null) {
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
					contacts.Add(newContact);
				}
			}

			if (json.Schedule != null) {
				foreach (var rec in json.Schedule)
				{
					// TODO
					if (rec.Type != SheduleRunType.Appoitment.ToString())
						continue;

					if (convert != null) {
						convert.ConvertSchedule?.Invoke(rec);
					}

					var client = contacts.GetContact(rec.ClientName, rec.ClientPhone);
					if (client == null)
						continue;

					var office = contacts.GetContact(rec.OfficeName, rec.OfficeName);
					if (office == null) {
						office = new Contact {
							ContactType = ContactType.Office,
							Name = rec.OfficeName,
							Phone = rec.OfficePhone,
							Address = rec.OfficeAddress
						};
						contacts.Add(office);
					}

					var appoitment = new Appoitment(client, office, rec.PickupTime, rec.DropoffTime);
					schedule.Add(appoitment);
				}
			}

			if (convert != null) {
				await SaveAsync(filename);
			}
		}

		public static async Task SaveAsync(string filename = null)
		{
			var contacts = new List<ContactRec>();
			foreach (var c in AppData.Contacts)
			{
				if (string.IsNullOrEmpty(c.Name) && string.IsNullOrEmpty(c.Phone))
					continue;

				var rec = new ContactRec {
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
				contacts.Add(rec);
			}

			var schedule = new List<ScheduleRec>();
			foreach (var p in AppData.Schedule)
			{
				var rec = new ScheduleRec {
					Type = p.SheduleRunType.ToString(),
					ClientName = p.Client.Name,
					ClientPhone = p.Client.Phone,
					OfficeName = p.Office.Name,
					OfficePhone = p.Office.Phone,
					OfficeAddress = p.Office.Address,
					PickupTime = p.PickupTime,
					DropoffTime = p.DropoffTime
				};
				schedule.Add(rec);
			}

			var json = new Json {
				Version = kVersion1,
				Contacts = contacts.ToArray(),
				Schedule = schedule.ToArray()
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
