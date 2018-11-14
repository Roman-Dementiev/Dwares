using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drums;


namespace ACE.Models
{
	public class Appoitment: ScheduleRun
	{
		public Appoitment() :
			this(null, null, new ScheduleTime(), new ScheduleTime())
		{ }

		public Appoitment(Contact client, Contact office, ScheduleTime pickupTime, ScheduleTime appoitmentTime) :
			base(SheduleRunType.Appoitment)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
			Office = office ?? throw new ArgumentNullException(nameof(office));
			PickupTime = pickupTime;
			DropoffTime = appoitmentTime;

			PickupStop = new RouteStop(RouteStopType.HomePickup, Client.Name, Client.Location, pickupTime);
			DropoffStop = new RouteStop(RouteStopType.OfficeDropoff, Office.Name, Office.Location, appoitmentTime);
			//DropoffStop.Origin = PickupStop.Location;

			Client.PropertyChanged += Client_PropertyChanged;
			Office.PropertyChanged += Dropoff_PropertyChanged;
		}

		public override string ToString()
		{
			return Strings.Properties(this,
				new string[] { nameof(Client), nameof(Office), nameof(PickupTime), nameof(DropoffTime) },
				skipNull: true);
		}

		//public string OfficeName => Office.Name;
		//public string OfficePhone => Office.Phone;
		//public string OfficeAddress => Office.Address;

		//public override string OriginName => ClientName;
		//public override ILocation Origin => Client;
		//public override ScheduleTime? OriginTime => PickupTime;
		//public RouteStop PickupStop => OriginStop;

		//public override string DestinationName => OfficeName;
		//public override ILocation Destination => Office;
		//public override ScheduleTime? DestinationTime => AppoitmentTime;
		//public RouteStop DropoffStop => DestinationStop;

		protected override void OnClientContactChanged(string propertyName)
		{
			base.OnClientContactChanged(propertyName);
			OnPickupContactChanged(Client, propertyName);
		}

		//private void Office_PropertyChanged(object sender, PropertyChangedEventArgs e)
		//{
		//	if (e.PropertyName == nameof(Contact.Name)) {
		//		FirePropertyChanged(nameof(DropoffStopName));
		//		DropoffStop.Name = DropoffStopName;
		//	}
		//	else if (e.PropertyName == nameof(Contact.Address)) {
		//		FirePropertyChanged(nameof(DropoffAddress));
		//	}
		//}

	}
}
