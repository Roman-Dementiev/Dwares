using System;
using System.ComponentModel;
using Xamarin.Forms;


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

		//public Pickup(Pickup src) :
		//	this()
		//{
		//	if (src != null) {
		//		CopyFrom(src);
		//	}
		//}

		//public void CopyFrom(Pickup src)
		//{
		//	Client.CopyFrom(src.Client);
		//	Office.CopyFrom(src.Office);
		//}

		bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				if (value != isSelected) {
					isSelected = value;

					OnPropertiesChanged(
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
			set => Client.Name = value;
		}
		public string ClientPhone {
			get => Client.Phone;
			set => Client.Phone = value;
		}
		public string ClientAddress {
			get => Client.Address;
			set => Client.Address = value;
		}
		public string OfficeName {
			get => Office.Name;
			set => Office.Name = value;
		}
		public string OfficePhone {
			get => Office.Phone;
			set => Office.Phone = value;
		}
		public string OfficeAddress {
			get => Office.Address;
			set => Office.Address = value;
		}

		public TimeSpan PickupTimeSpan => PickupTime.TimeSpan;
		public TimeSpan AppoitmenTimeSpan => AppoitmentTime.TimeSpan;
		public string PickupTimeString => PickupTime.ToString();
		public string AppoitmentTimeString => AppoitmentTime.ToString();

		public bool ShowClientName => Client.HasName;
		public bool ShowClientAddress => Client.HasAddress;
		public bool ShowOfficeInfo => IsSelected && (Office.HasName || Office.HasAddress);
		public bool ShowOfficeName => IsSelected && Office.HasName;
		public bool ShowOfficeAddress => IsSelected && Office.HasAddress;

		public Command CallClientCommand => Client.CallCommand;
		public Command CallOfficeCommand => Office.CallCommand;
	}
}
