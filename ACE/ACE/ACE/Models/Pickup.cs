using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drums;


namespace ACE.Models
{
	public class Pickup: Run
	{
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

		public Contact Client { get; }
		public Contact Office { get; }
		public ScheduleTime PickupTime { get; set; }
		public ScheduleTime AppoitmentTime { get; set; }

		public string ClientName => Client.Name;
		public string ClientPhone => Client.Phone;
		public string ClientAddress => Client.Address;
		public string OfficeName => Office.Name;
		public string OfficePhone => Office.Phone;
		public string OfficeAddress => Office.Address;
		public bool Wheelchair => Client.Wheelchair;
		public bool Escort => Client.Escort;

		public override string OriginName => ClientName;
		public override ILocation Origin => Client;
		public override ScheduleTime? OriginTime => PickupTime;
		public RouteStop PickupStop => OriginStop;

		public override string DestinationName => OfficeName;
		public override ILocation Destination => Office;
		public override ScheduleTime? DestinationTime => AppoitmentTime;
		public RouteStop DropoffStop => DestinationStop;

		private void Client_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Contact.Name)) {
				FirePropertyChanged(nameof(ClientName));
				FirePropertyChanged(nameof(OriginName));
			}
			else if (e.PropertyName == nameof(Contact.Phone)) {
				FirePropertyChanged(nameof(ClientPhone));
			}
			else if (e.PropertyName == nameof(Contact.Address)) {
				FirePropertyChanged(nameof(ClientAddress));
				FirePropertyChanged(nameof(Origin));
			}
			else if (e.PropertyName == nameof(Contact.Wheelchair)) {
				FirePropertyChanged(nameof(Wheelchair));
			}
			else if (e.PropertyName == nameof(Contact.Escort)) {
				FirePropertyChanged(nameof(Escort));
			}
		}

		private void Office_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Contact.Name)) {
				FirePropertyChanged(nameof(OfficeName));
				FirePropertyChanged(nameof(DestinationName));
			}
			else if (e.PropertyName == nameof(Contact.Phone)) {
				FirePropertyChanged(nameof(OfficePhone));
			}
			else if (e.PropertyName == nameof(Contact.Address)) {
				FirePropertyChanged(nameof(OfficeAddress));
				FirePropertyChanged(nameof(Destination));
			}
		}

	}
}
