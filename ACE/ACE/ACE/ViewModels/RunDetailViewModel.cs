using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Dwarf.Validation;
using Dwares.Druid.Satchel;
using Dwares.Druid.Forms;
using ACE.Models;


namespace ACE.ViewModels
{
	public class RunDetailViewModel : FormViewModel<ScheduleRun>
	{
		//ClassRef @class = new ClassRef(typeof(RunDetailViewModel));
		const string HomeStopName = "Home";

		protected RunDetailViewModel(ScheduleRun source, SheduleRunType runType)
		{
			//Debug.EnableTracing(@class);,
			Source = source;
			//SheduleRunType = runType;

			clientName = new PersonNameField();
			clientPhone = new PhoneField("Client phone number is invalid", "Client phone number is required");

			pickupStopName = new GeneralNameField();
			pickupAddress = new Field<string>();
			pickupTime = new TimeField();

			dropoffStopName = new GeneralNameField();
			dropoffAddress = new Field<string>();
			dropoffTime = new TimeField();
			
			wheelchair = new Field<bool>();
			escort = new Field<bool>();

			if (source != null) {
				clientName.Value = source.Client.Name;
				clientPhone.Value = source.Client.Phone;
				wheelchair.Value = source.Client.Wheelchair;
				escort.Value = source.Client.Escort;

				pickupStopName.Value = source.PickupStop.Name;
				pickupAddress.Value = source.PickupStop.Address;
				pickupTime.Value = source.PickupTime.Time;

				dropoffStopName.Value = source.DropoffStop.Name;
				dropoffAddress.Value = source.DropoffStop.Address;
				dropoffTime.Value = source.DropoffTime.Time;
			}

			validatables = new Validatables(clientPhone /*, pickupAddress, dropoffAddress*/);
		}

		//public SheduleRunType SheduleRunType { get; }

		PersonNameField clientName;
		public string ClientName {
			get => clientName;
			set => SetProperty(clientName, value);
		}

		PhoneField clientPhone;
		public string ClientPhone {
			get => clientPhone;
			set => SetProperty(clientPhone, value);
		}

		GeneralNameField pickupStopName;
		public string PickupStopName {
			get => pickupStopName;
			set => SetProperty(pickupStopName, value);
		}

		Field<string> pickupAddress;
		public string PickupAddress {
			get => pickupAddress;
			set => SetProperty(pickupAddress, value);
		}

		string dropoffNamePlaceholder;
		public string DropoffNamePlaceholder {
			get => dropoffNamePlaceholder;
			set => SetProperty(ref dropoffNamePlaceholder, value);
		}

		GeneralNameField dropoffStopName;
		public string DropoffStopName {
			get => dropoffStopName;
			set => SetProperty(dropoffStopName, value);
		}

		Field<string> dropoffAddress;
		public string DropoffAddress {
			get => dropoffAddress;
			set => SetProperty(dropoffAddress, value);
		}

		TimeField pickupTime;
		public TimeSpan PickupTime {
			get => pickupTime;
			set => SetProperty(pickupTime, value);
		}

		TimeField dropoffTime;
		public TimeSpan DropoffTime {
			get => dropoffTime;
			set => SetProperty(dropoffTime, value);
		}

		Field<bool> wheelchair;
		public bool Wheelchair {
			get => wheelchair;
			set => SetProperty(wheelchair, value);
		}

		Field<bool> escort;
		public bool Escort {
			get => escort;
			set => SetProperty(escort, value);
		}

		AutoSuggestions clientNameSuggestions;
		public AutoSuggestions ClientNameSuggestions {
			get => clientNameSuggestions;
			set => SetProperty(ref clientNameSuggestions, value);
		}

		AutoSuggestions clientPhoneSuggestions;
		public AutoSuggestions ClientPhoneSuggestions {
			get => clientPhoneSuggestions;
			set => SetProperty(ref clientPhoneSuggestions, value);
		}

		//AutoSuggestions officeNameSuggestions;
		//public AutoSuggestions OfficeNameSuggestions {
		//	get => officeNameSuggestions;
		//	set => SetProperty(ref officeNameSuggestions, value);
		//}

		//AutoSuggestions officePhoneSuggestions;
		//public AutoSuggestions OfficePhoneSuggestions {
		//	get => officePhoneSuggestions;
		//	set => SetProperty(ref officePhoneSuggestions, value);
		//}

		AutoSuggestions pickupNameSuggestions;
		public AutoSuggestions PickupNameSuggestions {
			get => pickupNameSuggestions;
			set => SetProperty(ref pickupNameSuggestions, value);
		}

		AutoSuggestions dropoffNameSuggestions;
		public AutoSuggestions DropoffNameSuggestions {
			get => dropoffNameSuggestions;
			set => SetProperty(ref dropoffNameSuggestions, value);
		}

		protected static AutoSuggestions GetNameSuggestions(List<Contact> contacts)
		{
			return new AutoSuggestions() {
				SuggestionSource = contacts,
				DisplayProperty = nameof(Contact.NameSuggestion)
			};

		}

		protected static AutoSuggestions GetPhoneSuggestions(List<Contact> contacts)
		{
			return new AutoSuggestions() {
				SuggestionSource = contacts,
				DisplayProperty = nameof(Contact.PhoneSuggestion)
			};
		}

		public virtual void UpdateAutoSuggestions()
		{
			var clients = AppData.Contacts.GetClients();
			//var offices = AppData.Contacts.GetOffices();

			ClientNameSuggestions = GetNameSuggestions(clients);
			ClientPhoneSuggestions = GetPhoneSuggestions(clients);
			//OfficeNameSuggestions = GetNameSuggestions(offices);
			//OfficePhoneSuggestions = GetPhoneSuggestions(offices);
		}

		public virtual void OnClientSelected(Contact client)
		{
			if (Source != null || client == null || client.ContactType != ContactType.Client)
				return;

			ClientName = client.Name;
			ClientPhone = client.Phone;
			Wheelchair = client.Wheelchair;
			Escort = client.Escort;
		}

		public virtual void OnPickupSelected(Contact contact)
		{
			if (Source != null || contact == null)
				return;

			PickupStopName = contact.Name;
			PickupAddress = contact.Address;
		}

		public virtual void OnDropoffSelected(Contact contact)
		{
			if (Source != null || contact == null)
				return;

			DropoffStopName = contact.Name;
			DropoffAddress = contact.Address;
		}

		protected override async Task<bool> Validate()
		{
			bool valid = await base.Validate();

			if (valid /*&& Source == null*/) {
				valid = await DoValidate();
			}

			return valid;
		}

		public virtual Task<bool> DoValidate()
		{
			return Task.FromResult(true);
		}

		protected async Task<bool> ValidateClient(string clientName, string clientPhone, string clientAddress)
		{
			var client = AppData.Contacts.GetContactByPhone(clientPhone);
			if (client != null && client.NeedUpdate(clientName, clientAddress)) {
				bool update = await Alerts.ConfirmAlert("Client name or address is different from the record.\nDo you want to update client information?");
				if (!update)
					return false;
			}

			return true;
		}

		protected async Task<bool> ValidateOffice(string officeName, string officeAddress)
		{
			var office = AppData.Contacts.GetContactByName(officeName);
			if (office != null) {
				if (office.NeedUpdate(officeName, officeAddress)) {
					bool update = await Alerts.ConfirmAlert("Office address is different from the record.\nDo you want update office information?");
					if (!update)
						return false;
				}
			}

			return true;
		}

		protected Contact UpdateClient(string clientName, string clientPhone, string clientAddress)
		{
			if (Source == null) {
				var client = AppData.Contacts.GetContactByPhone(ClientPhone);
				if (client == null) {
					client = new Contact {
						ContactType = ContactType.Client,
						Name = clientName,
						Phone = clientPhone,
						Address = clientAddress,
						Wheelchair = Wheelchair,
						Escort = Escort
					};

					AppData.Contacts.Add(client);
				}
				else {
					client.Update(clientName, clientAddress, Wheelchair, Escort);
				}
				return client;
			}
			else {
				Source.Client.Update(newAddress: clientAddress, wheelchair: Wheelchair, escort: Escort);
				return Source.Client;
			}
		}

		protected Contact UpdateOffice(string officeName, string officeAddress)
		{
			if (Source == null) {
				var office = AppData.Contacts.GetContactByName(officeName);
				if (office == null) {
					office = new Contact {
						ContactType = ContactType.Office,
						Name = officeName,
						Address = officeAddress,
					};

					if (!String.IsNullOrEmpty(officeName)) {
						AppData.Contacts.Add(office);
					}
				}
				else {
					office.Update(officeName, officeAddress);
				}
				return office;
			}
			else {
				Source.Office.Update(newAddress: officeAddress);
				return Source.Office;
			}
		}

		//protected override async Task DoAccept()
		//{
		//	if (Source == null) {
		//		var client = AppData.Contacts.GetContactByPhone(ClientPhone);
		//		if (client == null) {
		//			client = new Contact {
		//				ContactType = ContactType.Client,
		//				Name = ClientName,
		//				Phone = clientPhone,
		//				Address = ClientAddress,
		//				Wheelchair = Wheelchair,
		//				Escort = Escort
		//			};

		//			AppData.Contacts.Add(client);
		//		}
		//		else {
		//			client.Update(ClientName, ClientAddress, Wheelchair, Escort);
		//		}

		//		var office = AppData.Contacts.GetContactByName(OfficeName);
		//		if (office == null) {
		//			office = new Contact {
		//				ContactType = ContactType.Office,
		//				Name = OfficeName,
		//				//Phone = OfficePhone,
		//				Address = OfficeAddress,
		//			};

		//			if (!String.IsNullOrEmpty(OfficeName) /*|| !String.IsNullOrEmpty(OfficePhone)*/) {
		//				AppData.Contacts.Add(office);
		//			}
		//		}
		//		else {
		//			office.Update(OfficeName, OfficeAddress);
		//		}

		//		var newPickup = new Pickup(client, office,
		//			new ScheduleTime(PickupTime),
		//			new ScheduleTime(AppoitmentTime));

		//		AppData.Schedule.Add(newPickup);
		//	}
		//	else {
		//		Source.Client.Update(newAddress: ClientAddress, wheelchair: Wheelchair, escort: Escort);
		//		Source.Office.Update(newAddress: OfficeAddress);
		//		Source.PickupTime = new ScheduleTime(PickupTime);
		//		Source.DropoffTime = new ScheduleTime(AppoitmentTime);
		//	}

		//	await AppStorage.SaveAsync();
		//}
	}
}
