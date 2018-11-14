using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Xamarin.Forms;
using ACE.Models;
using System.ComponentModel;

namespace ACE.ViewModels
{
	public class ScheduleItem : ShadowIten<ScheduleRun>
	{
		//ClassRef @class = new ClassRef(typeof(ScheduleItem));

		public ScheduleItem(ScheduleRun run) :
			base(run)
		{
			//Debug.EnableTracing(@class);
		}

		public Contact Client => Source.Client;
		//public Contact Office => Source.Office;
		public ScheduleTime PickupTime => Source.PickupTime;
		public ScheduleTime DropoffTime => Source.DropoffTime;

		public string ClientName => Source.Client.Name;
		public string ClientPhone => Source.Client.Phone;
		public string ClientAddress => Source.Client.Address;
		//public string OfficeName => Source.Office.Name;
		//public string OfficeAddress => Source.Office.Address;
		public bool Wheelchair => Source.Client.Wheelchair;
		public bool Escort => Source.Client.Escort;

		public TimeSpan PickupTimeSpan => PickupTime.Time;
		public string PickupTimeString => PickupTime.ToString();
		public string PickupStopName => Source.PickupStop.Name;
		public string PickupAddress => Source.PickupAddress;

		public TimeSpan DropoffTimeSpan => DropoffTime.Time;
		public string DropoffTimeString => DropoffTime.ToString();
		public string DropoffStopName => Source.DropoffStop.Name;
		public string DropoffAddress => Source.DropoffAddress;

		public bool ShowClientName => Client.HasName;

		//public bool ShowPickupStopName => !string.IsNullOrEmpty(PickupStopName);
		public bool ShowPickupAddress => !string.IsNullOrEmpty(PickupAddress);

		public bool ShowDropoffInfo => IsSelected;
		public bool ShowDropoffStopName => !string.IsNullOrEmpty(DropoffStopName);
		public bool ShowDropoffAddress => !string.IsNullOrEmpty(DropoffAddress);

		public Command CallClientCommand => Client.CallCommand;
		//public Command CallOfficeCommand => Office.CallCommand;
		//public Command ClientDirectionsCommand => Client.DirectionsCommand;
		//public Command OfficeDirectionsCommand => Office.DirectionsCommand;

		protected override void OnSelectedChanged()
		{
			PropertiesChanged(
				nameof(IsSelected),
				nameof(ShowDropoffInfo)
				);
		}

		protected override void OnSourcePropertyChanged(string propertyName)
		{
			if (propertyName == nameof(ScheduleRun.ClientName)) {
				FirePropertyChanged(nameof(ClientName));
				FirePropertyChanged(nameof(ShowClientName));
			}
			else if (propertyName == nameof(ScheduleRun.PickupAddress)) {
				FirePropertyChanged(nameof(PickupAddress));
				FirePropertyChanged(nameof(ShowPickupAddress));
				//FirePropertyChanged(nameof(ShowClientDirections));
			}
			else if (propertyName == nameof(ScheduleRun.DropoffStopName)) {
				FirePropertyChanged(nameof(DropoffStopName));
				FirePropertyChanged(nameof(ShowDropoffStopName));
			}
			else if (propertyName == nameof(ScheduleRun.DropoffAddress)) {
				FirePropertyChanged(nameof(DropoffAddress));
				FirePropertyChanged(nameof(ShowDropoffAddress));
				//FirePropertyChanged(nameof(ShowDropoffDirections));
			}
			else {
				base.OnSourcePropertyChanged(propertyName);
			}
		}
	}
}
