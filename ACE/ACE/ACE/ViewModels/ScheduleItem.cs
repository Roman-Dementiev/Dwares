using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Xamarin.Forms;
using ACE.Models;
using System.ComponentModel;

namespace ACE.ViewModels
{
	public class ScheduleItem : ShadowIten<Pickup>
	{
		//ClassRef @class = new ClassRef(typeof(ScheduleItem));

		public ScheduleItem(Pickup pickup) :
			base(pickup)
		{
			//Debug.EnableTracing(@class);
		}

		public Contact Client => Source.Client;
		public Contact Office => Source.Office;
		public ScheduleTime PickupTime => Source.PickupTime;
		public ScheduleTime AppoitmentTime => Source.AppoitmentTime;

		public string ClientName => Source.Client.Name;
		public string ClientPhone => Source.Client.Phone;
		public string ClientAddress => Source.Client.Address;
		public string OfficeName => Source.Office.Name;
		public string OfficeAddress => Source.Office.Address;
		public bool Wheelchair => Source.Client.Wheelchair;
		public bool Escort => Source.Client.Escort;

		public TimeSpan PickupTimeSpan => PickupTime.Time;
		public TimeSpan AppoitmenTimeSpan => AppoitmentTime.Time;
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

		protected override void OnSelectedChanged()
		{
			PropertiesChanged(
				nameof(IsSelected),
				nameof(ShowOfficeInfo),
				nameof(ShowOfficeName),
				nameof(ShowOfficeAddress)
				);
		}

		protected override void OnSourcePropertyChanged(string propertyName)
		{
			if (propertyName == nameof(Pickup.ClientName)) {
				FirePropertyChanged(nameof(ClientName));
				FirePropertyChanged(nameof(ShowClientName));
			}
			else if (propertyName == nameof(Pickup.ClientAddress)) {
				FirePropertyChanged(nameof(ClientAddress));
				FirePropertyChanged(nameof(ShowClientAddress));
				FirePropertyChanged(nameof(ShowClientDirections));
			}
			else if (propertyName == nameof(Pickup.OfficeName)) {
				FirePropertyChanged(nameof(OfficeName));
				FirePropertyChanged(nameof(ShowClientName));
			}
			else if (propertyName == nameof(Pickup.OfficeAddress)) {
				FirePropertyChanged(nameof(OfficeAddress));
				FirePropertyChanged(nameof(ShowOfficeAddress));
				FirePropertyChanged(nameof(ShowOfficeDirections));
			}
			else {
				base.OnSourcePropertyChanged(propertyName);
			}
		}
	}
}
