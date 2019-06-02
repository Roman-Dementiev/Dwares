using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;
using Dwares.Drudge;
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
		public CurrentBase CurrentBase { get; private set; }

		public async Task Initialize()
		{
			MainBase = new MainBase(ApiKey, MainBaseId);
			await MainBase.Initialize();

			var baseRecords = await MainBase.BasesTable.ListBases();
			foreach (var record in baseRecords) {
				if (record.Year == 0) {
					if (record.Month == 2) {
						CurrentBase = new CurrentBase(ApiKey, record.BaseId);
					}
				}
			}

			if (CurrentBase == null)
				throw new AppStorageError("Contacts base not found");
		}

		public static PhoneNumber PhoneNumber(string number, string type)
		{
			var phone = new PhoneNumber(number);
			if (type == "Mobile") {
				phone.PhoneType = PhoneType.Mobile;
			} else if (type == "Home") {
				phone.PhoneType = PhoneType.Home;
			} else if (type == "Work") {
				phone.PhoneType = PhoneType.Work;
			}
			return phone;
		}

		public async Task LoadContacts()
		{
			var contacts = AppScope.Instance.Contacts;

			var pleacesRecords = await CurrentBase.PlacesTable.ListContacts();
			foreach (var placeRecord in pleacesRecords)
			{
				var place = new Place {
					Id = placeRecord.Id,
					Title = placeRecord.Name,
					FullTitle = placeRecord.FullTitle,
					PhoneNumber = PhoneNumber(placeRecord.Phone, placeRecord.PhoneType),
					Address = placeRecord.Address
				};
				contacts.Add(place);
			}

			var clienstRecords = await CurrentBase.ClientsTable.ListContacts();
			foreach (var clientRecord in clienstRecords)
			{
				var client = new Client {
					Id = clientRecord.Id,
					FullName = clientRecord.Name,
					PhoneNumber = PhoneNumber(clientRecord.Phone, clientRecord.PhoneType),
					Home = Home.ForAddress(clientRecord.Address),
					Escort = false // TODO
				};
				contacts.Add(client);

				var regularPlaceId = clientRecord.RegularPlaceId;
				if (!string.IsNullOrEmpty(regularPlaceId))
				{
					client.RegularPlace = contacts.GetById<Place>(regularPlaceId);
				}
			}

			var phonesRecords = await CurrentBase.PhonesTable.ListContacts();
			foreach (var phoneRecord in phonesRecords)
			{
				var phone = new Person {
					Id = phoneRecord.Id,
					FullName = phoneRecord.Name,
					PhoneNumber = PhoneNumber(phoneRecord.Phone, phoneRecord.PhoneType),
				};
				contacts.Add(phone);
			}

		}

		static Client GetClient(Contacts contacts, RideRecord record)
		{
			var clientId = record.ClientId;
			if (string.IsNullOrEmpty(clientId))
			{
				string message = $"Client link is missing for recordId={record.Id}";
				//throw new AppStorageError("message");
				Debug.Print("AirStorage.LoadSchedule(): {0}", message);
				return null;
			}

			var client = contacts.GetById<Client>(clientId);
			if (client == null)
			{
				string message = $"Unknown clientId={clientId}";
				//throw new AppStorageError("message");
				Debug.Print("AirStorage.LoadSchedule(): {0}", message);
			}
			
			return client;
		}

		static Place GetPlace(Contacts contacts, RideRecord record, string placeId)
		{
			if (string.IsNullOrEmpty(placeId))
			{
				string message = $"Place link is missing for recordId={record.Id}";
				//throw new AppStorageError("message");
				Debug.Print("AirStorage.LoadSchedule(): {0}", message);
				return null;
			}

			var place = contacts.GetById<Place>(placeId);
			if (place == null)
			{
				string message = $"Unknown clientId={placeId}";
				//throw new AppStorageError("message");
				Debug.Print("AirStorage.LoadSchedule(): {0}", message);
			}

			return place;
		}

		public async Task LoadSchedule()
		{
			var appScope = AppScope.Instance;
			var contacts = appScope.Contacts;
			var schedule = appScope.Schedule;

			var ridesRecords = await CurrentBase.RidesTable.ListRides();
			foreach (var record in ridesRecords)
			{
				var client = GetClient(contacts, record);
				if (client == null)
					continue;

				Ride ride;
				var rideType = record.RideType;
				if (rideType == "From Home")
				{
					var dropoffPlace = GetPlace(contacts, record, record.DropoffPlaceId);
					if (dropoffPlace == null)
						continue;

					ride = CompleteRide.FromHome(client, record.PickupTime, dropoffPlace, record.DropoffTime);
				}
				else if (rideType == "To Home")
				{
					var pickupPlace = GetPlace(contacts, record, record.PickupPlaceId);
					if (pickupPlace == null)
						continue;

					ride = CompleteRide.ToHome(client, pickupPlace, record.PickupTime, record.DropoffTime);
				}
				else if (rideType == "Pickup at home")
				{
					ride = PickupRide.AtHome(client, record.PickupTime);
				}
				else if (rideType == "Dropoff at home")
				{
					ride = DropoffRide.ToHome(client, record.DropoffTime);
				}
				else if (rideType == "Pickup")
				{
					var pickupPlace = GetPlace(contacts, record, record.PickupPlaceId);
					if (pickupPlace == null)
						continue;

					ride = new PickupRide(client, pickupPlace, record.PickupTime);
				}
				else if (rideType == "Dropoff")
				{
					var dropoffPlace = GetPlace(contacts, record, record.DropoffPlaceId);
					if (dropoffPlace == null)
						continue;

					ride = new DropoffRide(client, dropoffPlace, record.DropoffTime);
				}
				else
				{
					var pickupPlace = GetPlace(contacts, record, record.PickupPlaceId);
					if (pickupPlace == null)
						continue;
					var dropoffPlace = GetPlace(contacts, record, record.DropoffPlaceId);
					if (dropoffPlace == null)
						continue;

					ride = new CompleteRide(client, pickupPlace, record.PickupTime, dropoffPlace, record.DropoffTime);
				}

				ride.Id = record.Id;
				ride.Seq = record.Seq;
				schedule.Add(ride);
			}
		}

		public Task SaveContacts()
		{
			// TODO
			return Task.CompletedTask;
		}

		public Task SaveSchedule()
		{
			// TODO
			return Task.CompletedTask;
		}

	}
}
