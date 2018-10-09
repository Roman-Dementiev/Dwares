using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public class Pickup: NotifyPropertyChanged, ISelectable
	{
		public Contact Client { get; set; }
		public Contact Office { get; set; }
		public ScheduleTime PickupTime { get; set; }
		public ScheduleTime AppoitmentTime { get; set; }

		public Pickup()
		{
			Client = new Contact(ContactType.Client);
			Office = new Contact(ContactType.Office);
		}

		public Pickup(Contact client, Contact office, ScheduleTime pickupTime, ScheduleTime appoitmentTime)
		{
			Client = client;
			Office = office;
			PickupTime = pickupTime;
			AppoitmentTime = appoitmentTime;
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

		public string ClientName {
			get => Client.Name;
			set {
				Client.Name = value;
				RaisePropertyChanged();
			}
		}
		public string ClientPhone {
			get => Client.Phone;
			set {
				Client.Phone = value;
				RaisePropertyChanged();
			}
		}
		public string ClientAddress {
			get => Client.Address;
			set {
				Client.Address = value;
				RaisePropertyChanged();
			}
		}
		public string OfficeName {
			get => Office.Name;
			set {
				Office.Name = value;
				RaisePropertyChanged();
			}
		}
		public string OfficePhone {
			get => Office.Phone;
			set {
				Office.Phone = value;
				RaisePropertyChanged();
			}
		}
		public string OfficeAddress {
			get => Office.Address;
			set {
				Office.Address = value;
				RaisePropertyChanged();
			}
		}

		public TimeSpan PickupTimeSpan => PickupTime.TimeSpan;
		public TimeSpan AppoitmenTimeSpan => AppoitmentTime.TimeSpan;
		public string PickupTimeString => PickupTime.ToString();
		public string AppoitmentTimeString => AppoitmentTime.ToString();

		public bool ShowClientName => Client.HasName;
		public bool ShowClientAddress => Client.HasAddress;
		public bool ShowOfficeInfo => IsSelected;
		public bool ShowOfficeName => Office.HasName;
		public bool ShowOfficeAddress => Office.HasAddress;

		public Command CallClientCommand => Client.CallCommand;
		public Command CallOfficeCommand => Office.CallCommand;
	}
}
