using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ACE.Models;
using Dwares.Dwarf.Validation;
using Dwares.Druid.Support;

namespace ACE.ViewModels
{
	public class PickupDetailViewModel : BaseViewModel
	{
		Validatables validatables;

		public PickupDetailViewModel(INavigation navigation, Pickup source) :
			base(navigation)
		{
			Source = source;
			if (source != null) {
				ClientName = source.ClientName;
				//ClientPhone = source.ClientPhone;
				ClientAddress = source.ClientAddress;
				OfficeName = source.OfficeName;
				//OfficePhone = source.Office.Phone;
				OfficeAddress = source.OfficeAddress;
				PickupTime = (TimeSpan)source.PickupTime;
				AppoitmentTime = (TimeSpan)source.AppoitmentTime;
			} else {
				PickupTime = AppoitmentTime = (TimeSpan) AppData.ApproximateNextPickup();
			}

			clientPhone = new PhoneField("Client phone number is invalid", "Client phone number is required") { Value = source?.ClientPhone };
			officePhone = new PhoneField("Office phone number is invalid") { Value = source?.OfficePhone };

			validatables = new Validatables(clientPhone, officePhone);

			AcceptCommand = new Command(OnAccept);
			CancelCommand = new Command(OnCancel);

			//var clients = AppData.GetClients();
			//var offices = AppData.GetOffices();
			//clientPhoneSuggestions = GetPhoneSuggestions(clients);
			//officePhoneSuggestions = GetPhoneSuggestions(offices);
		}

		public Pickup Source { get;}
		public bool IsNiew => Source == null;
		public bool IsEditing => Source != null;

		string clientName;
		public string ClientName {
			get => clientName;
			set => SetProperty(ref clientName, value);
		}

		PhoneField clientPhone;
		public string ClientPhone {
			get => clientPhone;
			set => SetProperty(clientPhone, value);
		}

		string clientAddress;
		public string ClientAddress {
			get => clientAddress;
			set => SetProperty(ref clientAddress, value);
		}

		string officeName;
		public string OfficeName {
			get => officeName;
			set => SetProperty(ref officeName, value);
		}

		PhoneField officePhone;
		public string OfficePhone {
			get => officePhone;
			set => SetProperty(officePhone, value);
		}

		string officeAddress;
		public string OfficeAddress {
			get => officeAddress;
			set => SetProperty(ref officeAddress, value);
		}

		TimeSpan pickupTime;
		public TimeSpan PickupTime {
			get => pickupTime;
			set => SetProperty(ref pickupTime, value);
		}

		TimeSpan appoitmentTime;
		public TimeSpan AppoitmentTime {
			get => appoitmentTime;
			set => SetProperty(ref appoitmentTime, value);
		}

		public Command AcceptCommand { get; }
		public Command CancelCommand { get; }

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
				DisplayProperty = nameof(Contact.Name)
			};
		}

		private static AutoSuggestions GetPhoneSuggestions(List<Contact> contacts)
		{
			return new AutoSuggestions() {
				SuggestionSource = contacts,
				DisplayProperty = nameof(Contact.Phone)
			};
		}

		public void UpdateAutoSuggestions()
		{
			var clients = AppData.GetClients();
			var offices = AppData.GetOffices();
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
		}

		public void OnOfficeSelected(Contact office)
		{
			if (Source != null || office == null || office.ContactType != ContactType.Office)
				return;

			OfficeName = office.Name;
			OfficePhone = office.Phone;
			OfficeAddress = office.Address;
		}

		public async void OnAccept()
		{
			if (!validatables.Validate()) {
				await App.ErrorAlert(validatables.FirstError);
				return;
			}

			if (Source == null) {
				var client = AppData.GetContactByPhone(ClientPhone);
				if (client != null) {
					if (client.NeedUpdate(ClientName, ClientAddress)) {
						bool update = await App.ConfirmAlert("Client name or address is different from the record.\nDo you want to update client information?");
						if (!update)
							return;
					}
				}

				var office = AppData.GetContactByPhone(OfficePhone);
				if (office != null) {
					if (office.NeedUpdate(OfficeName, OfficeAddress)) {
						bool update = await App.ConfirmAlert("Office name or address is different from the record.\nDo you want update office information?");
						if (!update)
							return;
					}
				}

				if (client == null) {
					client = new Contact(ContactType.Client) {
						Name = ClientName,
						Phone = clientPhone,
						Address = ClientAddress,
					};

					await AppData.NewContact(client);
				} else {
					client.Update(newName: ClientName, newAddress: ClientAddress);
				}

				if (office == null) {
					office = new Contact(ContactType.Office) {
						Name = OfficeName,
						Phone = OfficePhone,
						Address = OfficeAddress,
					};

					await AppData.NewContact(office);
				} else {
					office.Update(newName: OfficeName, newAddress: OfficeAddress);
				}

				var newPickup = new Pickup(client, office);
				await AppData.NewPickup(newPickup);
			}
			else {
				//Source.ClientName = this.ClientName;
				//Source.ClientPhone = this.ClientPhone;
				Source.ClientAddress = this.ClientAddress;
				//Source.OfficeName = this.OfficeName;
				//Source.OfficePhone = this.OfficePhone;
				Source.OfficeAddress = this.OfficeAddress;
				Source.PickupTime = (ScheduleTime)this.PickupTime;
				Source.AppoitmentTime = (ScheduleTime)this.AppoitmentTime;
			}

			//await Navigation.PopModalAsync();
			await Navigator.PopPageAsync();
		}

		public async void OnCancel()
		{
			//await Navigation.PopModalAsync();
			await Navigator.PopPageAsync();
		}
	}
}
