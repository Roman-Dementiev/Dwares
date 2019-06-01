using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ScheduleItem : ListViewItem
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleItemViewModel));

		public ScheduleItem(Ride ride)
		{
			//Debug.EnableTracing(@class);

			Ride = ride;

			PropertiesChangedOnSelected = new string[] {
				nameof(IsSelected),
				nameof(CornerRadius),
				nameof(BorderColor),
				nameof(BackgroundColor),
				nameof(ShowDetails),
				nameof(ShowPickupAddress),
				nameof(ShowDropoffAddress)
			};
		}

		public Ride Ride{ get; }

		//public Client Client => Trip.Client;
		public string ClientName => Ride.Client.FullName;

		public bool HasClientPhone => !string.IsNullOrEmpty(Ride.Client.PhoneNumber);
		public string ClientPhone => Ride.Client.PhoneNumber;

		public bool HasPickupStop => Ride.PickupStop != null;
		public string PickupTime => HasPickupStop ? Ride.PickupStop.Time.ToString() : string.Empty;
		public string PickupPlace => HasPickupStop ? Ride.PickupStop.Place.Title : string.Empty;
		public string PickupAddress => HasPickupStop ? Ride.PickupStop.Place.Address : string.Empty;
		public bool ShowPickupAddress => HasPickupStop ? ShowDetails : false;

		public bool HasDropoffStop => Ride.DropoffStop != null;
		public string DropoffTime => HasDropoffStop ? Ride.DropoffStop.Time.ToString() : string.Empty;
		public string DropoffPlace => HasDropoffStop ? Ride.DropoffStop.Place.Title : string.Empty;
		public string DropoffAddress => HasDropoffStop ? Ride.DropoffStop.Place.Address : string.Empty;
		public bool ShowDropoffAddress => HasDropoffStop ? ShowDetails : false;


		public static ObservableCollection<ScheduleItem> CreateCollection()
		{
			return new ShadowCollection<ScheduleItem, Ride>(
				AppScope.Instance.Schedule,
				(ride) => new ScheduleItem(ride)
				);
		}
	}
}
