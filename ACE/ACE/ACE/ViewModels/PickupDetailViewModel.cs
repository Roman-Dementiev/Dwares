using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf.Validation;
using Dwares.Druid.Forms;
using Dwares.Druid.Support;
using ACE.Models;
using System.Collections;

namespace ACE.ViewModels
{
	public class PickupDetailViewModel : FormScope
	{
		public PickupDetailViewModel(Pickup source)
		{
			Source = source;

			clientName = new PersonNameField();
			officeName = new GeneralNameField();
			clientAddress = new Field<string>();
			officeAddress = new Field<string>();
			clientPhone = new PhoneField("Client phone number is invalid", "Client phone number is required");
			//officePhone = new PhoneField("Office phone number is invalid");
			pickupTime = new TimeField();
			appoitmentTime = new TimeField();
			wheelchair = new Field<bool>();
			escort = new Field<bool>();

			if (source != null) {
				clientName.Value = source.ClientName;
				officeName.Value = source.OfficeName;
				clientPhone.Value = source.ClientPhone;
				//officePhone.Value = source.OfficePhone;
				clientAddress.Value = source.ClientAddress;
				officeAddress.Value = source.OfficeAddress;
				pickupTime.Value = source.PickupTime.TimeSpan;
				appoitmentTime.Value = source.AppoitmentTime.TimeSpan ;
				wheelchair.Value = source.Wheelchair;
				escort.Value = source.Escort;
			} else {
				AppData.Schedule.EstimateNextPickup(out var pickup, out var appoitment);
				pickupTime.Value = pickup.TimeSpan;
				appoitmentTime.Value = appoitment.TimeSpan;
			}

			validatables = new Validatables(clientPhone, clientAddress, /*officePhone,*/ officeAddress);
		}

		public Pickup Source { get;}
		public bool IsNew => Source == null;
		public bool IsEditing => Source != null;

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

		Field<string> clientAddress;
		public string ClientAddress {
			get => clientAddress;
			set => SetProperty(clientAddress, value);
		}

		GeneralNameField officeName;
		public string OfficeName {
			get => officeName;
			set => SetProperty(officeName, value);
		}

		//PhoneField officePhone;
		//public string OfficePhone {
		//	get => officePhone;
		//	set => SetProperty(officePhone, value);
		//}

		Field<string> officeAddress;
		public string OfficeAddress {
			get => officeAddress;
			set => SetProperty(officeAddress, value);
		}

		TimeField pickupTime;
		public TimeSpan PickupTime {
			get => pickupTime;
			set => SetProperty(pickupTime, value);
		}

		TimeField appoitmentTime;
		public TimeSpan AppoitmentTime {
			get => appoitmentTime;
			set => SetProperty(appoitmentTime, value);
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

		AutoSuggestions officeNameSuggestions;
		public AutoSuggestions OfficeNameSuggestions {
			get => officeNameSuggestions;
			set => SetProperty(ref officeNameSuggestions, value);
		}

		AutoSuggestions officePhoneSuggestions;
		public AutoSuggestions OfficePhoneSuggestions {
			get => officePhoneSuggestions;
			set => SetProperty(ref officePhoneSuggestions, value);
		}

		private static AutoSuggestions GetNameSuggestions(List<Contact> contacts)
		{
			return new AutoSuggestions() {
				SuggestionSource = contacts,
				DisplayProperty = nameof(Contact.NameSuggestion)
			};

		}

		private static AutoSuggestions GetPhoneSuggestions(List<Contact> contacts)
		{
			return new AutoSuggestions() {
				SuggestionSource = contacts,
				DisplayProperty = nameof(Contact.PhoneSuggestion)
			};
		}

		public void UpdateAutoSuggestions()
		{
			var clients = AppData.Contacts.GetClients();
			var offices = AppData.Contacts.GetOffices();
			ClientNameSuggestions = GetNameSuggestions(clients);
			OfficeNameSuggestions = GetNameSuggestions(offices);
			ClientPhoneSuggestions = GetPhoneSuggestions(clients);
			OfficePhoneSuggestions = GetPhoneSuggestions(offices);
		}

		public void OnClientSelected(Contact client)
		{
			if (Source != null || client == null || client.ContactType != ContactType.Client)
				return;

			ClientName = client.Name;
			ClientPhone = client.Phone;
			ClientAddress = client.Address;
			Wheelchair = client.Wheelchair;
			Escort = client.Escort;
		}

		public void OnOfficeSelected(Contact office)
		{
			if (Source != null || office == null || office.ContactType != ContactType.Office)
				return;

			OfficeName = office.Name;
			//OfficePhone = office.Phone;
			OfficeAddress = office.Address;
		}

		protected override async Task<bool> Validate()
		{
			bool valid = await base.Validate();

			if (valid  && Source == null) {
				var client = AppData.Contacts.GetContactByPhone(ClientPhone);
				if (client != null && client.NeedUpdate(ClientName, ClientAddress)) {
					bool update = await Alerts.ConfirmAlert("Client name or address is different from the record.\nDo you want to update client information?");
					if (!update)
						return false;
				}

				var office = AppData.Contacts.GetContactByName(OfficeName);
				if (office != null) {
					if (office.NeedUpdate(OfficeName, OfficeAddress)) {
						bool update = await Alerts.ConfirmAlert("Office address is different from the record.\nDo you want update office information?");
						if (!update)
							return false;
					}
				}
			}

			return valid;
		}

		protected override async Task DoAccept()
		{
			if (Source == null) {
				var client = AppData.Contacts.GetContactByPhone(ClientPhone);
				if (client == null) {
					client = new Contact {
						ContactType = ContactType.Client,
						Name = ClientName,
						Phone = clientPhone,
						Address = ClientAddress,
						Wheelchair = Wheelchair,
						Escort = Escort
					};

					AppData.Contacts.Add(client);
				} else {
					client.Update(ClientName, ClientAddress, Wheelchair, Escort);
				}

				var office = AppData.Contacts.GetContactByName(OfficeName);
				if (office == null) {
					office = new Contact {
						ContactType = ContactType.Office,
						Name = OfficeName,
						//Phone = OfficePhone,
						Address = OfficeAddress,
					};

					if (!String.IsNullOrEmpty(OfficeName) /*|| !String.IsNullOrEmpty(OfficePhone)*/) {
						AppData.Contacts.Add(office);
					}
				} else {
					office.Update(OfficeName, OfficeAddress);
				}

				var newPickup = new Pickup(client, office, 
					(ScheduleTime)PickupTime,
					(ScheduleTime)AppoitmentTime);

				AppData.Schedule.Add(newPickup);
			} else {
				Source.Client.Update(newAddress: ClientAddress, wheelchair: Wheelchair, escort: Escort);
				Source.Office.Update(newAddress: OfficeAddress);
				Source.PickupTime = (ScheduleTime)PickupTime;
				Source.AppoitmentTime = (ScheduleTime)AppoitmentTime;
			}

			await AppStorage.SaveAsync();
		}
	}
}
