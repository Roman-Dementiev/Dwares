using System;
using Xamarin.Forms;
using ACE.Models;


namespace ACE.ViewModels
{
	public class PickupDetailViewModel : BaseViewModel
	{
		public PickupDetailViewModel(INavigation navigation, Pickup source) :
			base(navigation)
		{
			Source = source;
			
			if (source != null) {
				ClientName = source.ClientName;
				ClientPhone = source.ClientPhone; ;
				ClientAddress = source.ClientAddress;
				OfficeName = source.OfficeName;
				OfficePhone = source.Office.Phone;
				OfficeAddress = source.OfficeAddress;
				PickupTime = (TimeSpan)source.PickupTime;
				AppoitmentTime = (TimeSpan)source.AppoitmentTime;
			} else {
				PickupTime = AppoitmentTime = (TimeSpan) AppData.ApproximateNextPickup();
			}

			AcceptCommand = new Command(OnAccept);
			CancelCommand = new Command(OnCancel);
		}

		public Pickup Source { get;}
		public string ClientName { get; set; }
		public string ClientPhone { get; set; }
		public string ClientAddress { get; set; }
		public string OfficeName { get; set; }
		public string OfficePhone { get; set; }
		public string OfficeAddress { get; set; }
		public TimeSpan PickupTime { get; set; }
		public TimeSpan AppoitmentTime { get; set; }

		public Command AcceptCommand { get; }
		public Command CancelCommand { get; }


		public async void OnAccept()
		{
			if (String.IsNullOrEmpty(ClientPhone)) {
				await App.ErrorAlert("Client pnone number is required");
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
