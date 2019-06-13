using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ScheduleItem : ListViewItem<Ride>
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleItemViewModel));

		public ScheduleItem(Ride ride) :
			base(ride)
		{
			//Debug.EnableTracing(@class);

			UpdateFromSource();
		}

		public Ride Ride => Source;
		public Client Client => Ride.Client;

		string clientName;
		public string ClientName {
			get => clientName;
			set => SetProperty(ref clientName, value);
		}

		string clientPhone;
		public string ClientPhone {
			get => clientPhone;
			set {
				if (SetProperty(ref clientPhone, value)) {
					ShowClientPhone = ShowDetails && !string.IsNullOrEmpty(ClientPhone);
				}
			}
		}

		bool showClientPhone;
		public bool ShowClientPhone {
			get => showClientPhone;
			set => SetProperty(ref showClientPhone, value);
		}

		bool hasPickupStop;
		public bool HasPickupStop {
			get => hasPickupStop;
			set => SetProperty(ref hasPickupStop, value);
		}

		string pickupTime;
		public string PickupTime {
			get => pickupTime;
			set => SetProperty(ref pickupTime, value);
		}

		string pickupPlace;
		public string PickupPlace {
			get => pickupPlace;
			set => SetProperty(ref pickupPlace, value);
		}

		string pickupAddres;
		public string PickupAddress {
			get => pickupAddres;
			set => SetProperty(ref pickupAddres, value);
		}

		bool showPickupAddress;
		public bool ShowPickupAddress {
			get => showPickupAddress;
			set => SetProperty(ref showPickupAddress, value);
		}

		bool hasDropoffStop;
		public bool HasDropoffStop {
			get => hasDropoffStop;
			set => SetProperty(ref hasDropoffStop, value);
		}

		string dropoffTime;
		public string DropoffTime {
			get => dropoffTime;
			set => SetProperty(ref dropoffTime, value);
		}

		string dropoffPlace;
		public string DropoffPlace {
			get => dropoffPlace;
			set => SetProperty(ref dropoffPlace, value);
		}

		string dropoffAddress;
		public string DropoffAddress {
			get => dropoffAddress;
			set => SetProperty(ref dropoffAddress, value);
		}

		bool showDropoffAddress;
		public bool ShowDropoffAddress {
			get => showDropoffAddress;
			set => SetProperty(ref showDropoffAddress, value);
		}

		protected override void UpdateFromSource()
		{
			ClientName = Ride.Client.FullName;
			ClientPhone = Ride.Client.PhoneNumber;
			ShowClientPhone = ShowDetails && !string.IsNullOrEmpty(ClientPhone);

			if (Ride.PickupStop != null) {
				HasPickupStop = true;
				PickupTime = Ride.PickupStop.Time.ToString();
				PickupPlace = Ride.PickupStop.Place.Title;
				PickupAddress = Ride.PickupStop.Place.Address;
				ShowPickupAddress = ShowDetails;
			} else {
				HasPickupStop = ShowPickupAddress = false;
				PickupTime = PickupPlace = PickupAddress = string.Empty;
			}

			if (Ride.DropoffStop != null) {
				HasDropoffStop = true;
				DropoffTime = Ride.DropoffStop.Time.ToString();
				DropoffPlace = Ride.DropoffStop.Place.Title;
				DropoffAddress = Ride.DropoffStop.Place.Address;
				ShowDropoffAddress = ShowDetails;
			} else {
				HasDropoffStop = ShowDropoffAddress = false;
				DropoffTime = DropoffPlace = DropoffAddress = string.Empty;
			}
		}

		protected override void OnShowDetailsChanged()
		{
			ShowPickupAddress = ShowDetails && HasPickupStop;
			ShowDropoffAddress = ShowDetails && HasDropoffStop;
		}


		public static ObservableCollection<ScheduleItem> CreateCollection()
		{
			return new ShadowCollection<ScheduleItem, Ride>(
				AppScope.Instance.Schedule,
				(ride) => new ScheduleItem(ride)
				);
		}
	}
}
