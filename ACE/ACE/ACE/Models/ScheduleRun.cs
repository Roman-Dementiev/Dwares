using System;
using System.ComponentModel;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public enum SheduleRunType
	{
		Appoitment,
		ReturnHpme,
		MidddleRun
	}

	public class ScheduleRun : PropertyNotifier
	{
		protected ScheduleRun(SheduleRunType runType)
		{
			SheduleRunType = runType;
		}

		public SheduleRunType SheduleRunType { get; }
		public Contact Client { get; protected set; }
		public Contact Office { get; protected set; }

		public RouteStop PickupStop { get; set; }
		public ScheduleTime PickupTime { get; set; }
		public RouteStop DropoffStop { get; set; }
		public ScheduleTime DropoffTime { get; set; }

		public string ClientName => Client.Name;
		public string ClientPhone => Client.Phone;
		//public string ClientAddress => Client.Address;
		public bool Wheelchair => Client.Wheelchair;
		public bool Escort => Client.Escort;

		public string PickupStopName => PickupStop?.Name;
		public string PickupAddress => PickupStop?.Address;

		public string DropoffStopName => DropoffStop?.Name;
		public string DropoffAddress => DropoffStop?.Address;

		protected virtual void OnClientContactChanged(string propertyName)
		{
			if (propertyName == nameof(Contact.Name)) {
				FirePropertyChanged(nameof(ClientName));
			}
			else if (propertyName == nameof(Contact.Phone)) {
				FirePropertyChanged(nameof(ClientPhone));
			}
			else if (propertyName == nameof(Contact.Wheelchair)) {
				FirePropertyChanged(nameof(Wheelchair));
			}
			else if (propertyName == nameof(Contact.Escort)) {
				FirePropertyChanged(nameof(Escort));
			}
		}

		protected virtual void OnPickupContactChanged(Contact contact, string propertyName)
		{
			if (propertyName == nameof(Contact.Name)) {
				PickupStop.Name = contact.Name;
				FirePropertyChanged(nameof(PickupStopName));
			}
			else if (propertyName == nameof(Contact.Address)) {
				PickupStop.Address = contact.Address;
				FirePropertyChanged(nameof(PickupAddress));
			}
		}

		protected virtual void OnDropoffContactChanged(Contact contact, string propertyName)
		{
			if (propertyName == nameof(Contact.Name)) {
				DropoffStop.Name = contact.Name;
				FirePropertyChanged(nameof(DropoffStopName));
			}
			else if (propertyName == nameof(Contact.Address)) {
				DropoffStop.Address = contact.Address;
				FirePropertyChanged(nameof(DropoffAddress));
			}
		}

		protected void Client_PropertyChanged(object sender, PropertyChangedEventArgs e)
			=> OnClientContactChanged(e.PropertyName);

		protected void Pickup_PropertyChanged(object sender, PropertyChangedEventArgs e)
			=> OnPickupContactChanged(sender as Contact, e.PropertyName);

		protected void Dropoff_PropertyChanged(object sender, PropertyChangedEventArgs e)
			=> OnDropoffContactChanged(sender as Contact, e.PropertyName);
	}
}
