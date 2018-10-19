using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public class Pickup: PropertyNotifier, ISelectable
	{
		public Contact Client { get; set; }
		public Contact Office { get; set; }
		public ScheduleTime PickupTime { get; set; }
		public ScheduleTime AppoitmentTime { get; set; }

		public Pickup()
		{
			Client = new Contact(ContactType.Client);
			Office = new Contact(ContactType.Office);

			Client.PropertyChanged += Client_PropertyChanged;
			Office.PropertyChanged += Office_PropertyChanged;
		}

		public Pickup(Contact client, Contact office, ScheduleTime pickupTime, ScheduleTime appoitmentTime)
		{
			Debug.AssertNotNull(client);
			Debug.AssertNotNull(office);

			Client = client;
			Office = office;
			PickupTime = pickupTime;
			AppoitmentTime = appoitmentTime;

			Client.PropertyChanged += Client_PropertyChanged;
			Office.PropertyChanged += Office_PropertyChanged;
		}

		public override string ToString()
		{
			return Strings.Properties(this,
				new string[] { nameof(Client), nameof(Office), nameof(PickupTime), nameof(AppoitmentTime) },
				skipNull: true);
		}

		bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				if (value != isSelected) {
					isSelected = value;

					PropertiesChanged(
						nameof(IsSelected),
						nameof(ShowOfficeInfo),
						nameof(ShowOfficeName),
						nameof(ShowOfficeAddress)
						);
				}
			}
		}

		private void Client_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Contact.Name)) {
				RaisePropertyChanged(nameof(ClientName));
				RaisePropertyChanged(nameof(ShowClientName));
			}
			else if (e.PropertyName == nameof(Contact.Phone)) {
				RaisePropertyChanged(nameof(ClientPhone));
			}
			else if (e.PropertyName == nameof(Contact.Address)) {
				RaisePropertyChanged(nameof(ClientAddress));
				RaisePropertyChanged(nameof(ShowClientAddress));
				RaisePropertyChanged(nameof(ShowClientDirections));

			}
		}

		private void Office_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Contact.Name)) {
				RaisePropertyChanged(nameof(OfficeName));
				RaisePropertyChanged(nameof(ShowOfficeName));
			} else if (e.PropertyName == nameof(Contact.Phone)) {
				RaisePropertyChanged(nameof(OfficePhone));
			} else if (e.PropertyName == nameof(Contact.Address)) {
				RaisePropertyChanged(nameof(OfficeAddress));
				RaisePropertyChanged(nameof(ShowOfficeAddress));
				RaisePropertyChanged(nameof(ShowOfficeDirections));

			}
		}

		public string ClientName {
			get => Client.Name;
			set {
				if (value != Client.Name) {
					Client.Name = value;
					//RaisePropertyChanged();
					//RaisePropertyChanged(nameof(ShowClientName));
				}
			}
		}
		public string ClientPhone {
			get => Client.Phone;
			set {
				if (value != Client.Phone) {
					Client.Phone = value;
					//RaisePropertyChanged();
				}
			}
		}
		public string ClientAddress {
			get => Client.Address;
			set {
				if (value != Client.Address) {
					Client.Address = value;
					//RaisePropertyChanged();
					//RaisePropertyChanged(nameof(ShowClientAddress));
					//RaisePropertyChanged(nameof(ShowClientDirections));
				}
			}
		}
		public string OfficeName {
			get => Office.Name;
			set {
				if (value != Office.Name) {
					Office.Name = value;
					//RaisePropertyChanged();
					//RaisePropertyChanged(nameof(ShowOfficeName));
				}
			}
		}
		public string OfficePhone {
			get => Office.Phone;
			set {
				if (value != Office.Phone) {
					Office.Phone = value;
					//RaisePropertyChanged();
				}
			}
		}
		public string OfficeAddress {
			get => Office.Address;
			set {
				if (value != Office.Address) {
					Office.Address = value;
					//RaisePropertyChanged();
					//RaisePropertyChanged(nameof(ShowOfficeAddress));
					//RaisePropertyChanged(nameof(ShowOfficeDirections));
				}
			}
		}

		public TimeSpan PickupTimeSpan => PickupTime.TimeSpan;
		public TimeSpan AppoitmenTimeSpan => AppoitmentTime.TimeSpan;
		public string PickupTimeString => PickupTime.ToString();
		public string AppoitmentTimeString => AppoitmentTime.ToString();

		public bool ShowClientName => Client.HasName;
		public bool ShowClientAddress => Client.HasAddress;
		public bool ShowClientDirections => Client.HasAddress;
		public bool ShowOfficeInfo => IsSelected;
		public bool ShowOfficeName => Office.HasName;
		public bool ShowOfficeAddress => Office.HasAddress;
		public bool ShowOfficeDirections => Office.HasAddress;

		public Command CallClientCommand => Client.CallCommand;
		public Command CallOfficeCommand => Office.CallCommand;
		public Command ClientDirectionsCommand => Client.DirectionsCommand;
		public Command OfficeDirectionsCommand => Office.DirectionsCommand;

	}
}
