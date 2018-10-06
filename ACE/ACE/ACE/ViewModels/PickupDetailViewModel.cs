using System;
using Xamarin.Forms;
using ACE.Models;
using Dwares.Dwarf.Validation;
using Dwares.Druid.Support;
using System.Collections.Generic;

namespace ACE.ViewModels
{
	public class PickupDetailViewModel : BaseViewModel
	{
		PhoneField clientPhone;
		PhoneField officePhone;
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
		public string ClientName { get; set; }
		public string ClientPhone {
			get => clientPhone.Value;
			set => clientPhone.Value = value;
		}
		public string ClientAddress { get; set; }
		public string OfficeName { get; set; }
		public string OfficePhone {
			get => officePhone.Value;
			set => officePhone.Value = value;
		}
		public string OfficeAddress { get; set; }
		public TimeSpan PickupTime { get; set; }
		public TimeSpan AppoitmentTime { get; set; }

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

		public async void OnAccept()
		{
			//if (String.IsNullOrEmpty(ClientPhone)) {
			//	await App.ErrorAlert("Client pnone number is required");
			//	return;
			//}

			if (!validatables.Validate()) {
				await App.ErrorAlert(validatables.FirstError);
				return;
			}

			var newPickup = new Pickup() {
				ClientName = this.ClientName,
				ClientPhone = this.ClientPhone,
				ClientAddress = this.ClientAddress,
				OfficeName = this.OfficeName,
				OfficePhone = this.OfficePhone,
				OfficeAddress = this.OfficeAddress,
				PickupTime = (ScheduleTime)this.PickupTime,
				AppoitmentTime = (ScheduleTime)this.AppoitmentTime
			};
			await AppData.ReplacePickup(newPickup, Source);
			
			await Navigation.PopModalAsync();
		}

		public async void OnCancel()
		{
			await Navigation.PopModalAsync();
		}
	}
}
