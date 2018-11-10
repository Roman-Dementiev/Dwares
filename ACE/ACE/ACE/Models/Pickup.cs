using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drums;


namespace ACE.Models
{
	public class Pickup: Run, ISelectable
	{
		public Contact Client { get; }
		public Contact Office { get; }
		public ScheduleTime PickupTime { get; set; }
		public ScheduleTime AppoitmentTime { get; set; }

		public Pickup() :
			this(null, null, new ScheduleTime(), new ScheduleTime())
		{ }

		public Pickup(Contact client, Contact office, ScheduleTime pickupTime, ScheduleTime appoitmentTime)
		{
			Client = client ?? new Contact { ContactType = ContactType.Client };
			Office = office ?? new Contact { ContactType = ContactType.Office };

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

		public override string OriginName => ClientName;
		public override ILocation Origin => Client;
		public override ScheduleTime? OriginTime => PickupTime;
		public RouteStop PickupStop => OriginStop;

		public override string DestinationName => OfficeName;
		public override ILocation Destination => Office;
		public override ScheduleTime? DestinationTime => AppoitmentTime;
		public RouteStop DropoffStop => DestinationStop;

		protected override void OnSelectedChanged()
		{
			PropertiesChanged(
				nameof(IsSelected),
				nameof(ShowOfficeInfo),
				nameof(ShowOfficeName),
				nameof(ShowOfficeAddress)
				);
		}

		private void Client_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Contact.Name)) {
				FirePropertyChanged(nameof(ClientName));
				FirePropertyChanged(nameof(ShowClientName));
			}
			else if (e.PropertyName == nameof(Contact.Phone)) {
				FirePropertyChanged(nameof(ClientPhone));
			}
			else if (e.PropertyName == nameof(Contact.Address)) {
				FirePropertyChanged(nameof(ClientAddress));
				FirePropertyChanged(nameof(ShowClientAddress));
				FirePropertyChanged(nameof(ShowClientDirections));
			} else if (e.PropertyName == nameof(Contact.Wheelchair)) {
				FirePropertyChanged(nameof(Wheelchair));
			} else if (e.PropertyName == nameof(Contact.Escort)) {
				FirePropertyChanged(nameof(Escort));
			}
		}

		private void Office_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Contact.Name)) {
				FirePropertyChanged(nameof(OfficeName));
				FirePropertyChanged(nameof(ShowOfficeName));
			//} else if (e.PropertyName == nameof(Contact.Phone)) {
			//	RaisePropertyChanged(nameof(OfficePhone));
			} else if (e.PropertyName == nameof(Contact.Address)) {
				FirePropertyChanged(nameof(OfficeAddress));
				FirePropertyChanged(nameof(ShowOfficeAddress));
				FirePropertyChanged(nameof(ShowOfficeDirections));

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

		public bool Wheelchair {
			get => Client.Wheelchair;
			set {
				if (value != Client.Wheelchair) {
					Client.Wheelchair = value;
					//RaisePropertyChanged();
				}
			}
		}

		public bool Escort {
			get => Client.Escort;
			set {
				if (value != Client.Escort) {
					Client.Escort = value;
					//RaisePropertyChanged();
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
		//public string OfficePhone {
		//	get => Office.Phone;
		//	set {
		//		if (value != Office.Phone) {
		//			Office.Phone = value;
		//			//RaisePropertyChanged();
		//		}
		//	}
		//}

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
