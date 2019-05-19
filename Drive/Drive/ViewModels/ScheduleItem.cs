using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ScheduleItem : PropertyNotifier, ISelectable
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleItemViewModel));

		public ScheduleItem(Ride ride)
		{
			//Debug.EnableTracing(@class);

			Ride = ride;
		}

		public Ride Ride{ get; }

		//public Client Client => Trip.Client;
		public string ClientName => Ride.Client.FullName;
		public string ClientPhone => Ride.Client.PhoneNumber;

		public string FirstStopTime => Ride.FirstStop.Time.ToString();
		public string FirstStopTitle => Ride.FirstStop.Place.Title;
		public string FirstStopAddress => Ride.FirstStop.Place.Address;

		public bool HasSecondStop => Ride.SecondStop != null;
		public string SecondStopTime => HasSecondStop ? ((Ride.Stop)Ride.SecondStop).Time.ToString() : string.Empty;
		public string SecondStopTitle => HasSecondStop ? ((Ride.Stop)Ride.SecondStop).Place.Title : string.Empty;
		public string SecondStopAddress => HasSecondStop ? ((Ride.Stop)Ride.SecondStop).Place.Address : string.Empty;

		bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set => SetPropertyEx(ref isSelected, value, nameof(IsSelected),
				nameof(ShowDetails), nameof(CornerRadius), nameof(BorderColor), nameof(BackgroundColor));
		}

		public bool ShowDetails {
			// TODO: why UneventRows doesn't work on UWP??
			//get => IsSelected;

			get {
				return Device.RuntimePlatform == Device.UWP || isSelected;
			}
		}

		static float cornerRadius = 10;
		public float CornerRadius {
			get => IsSelected ? 0 : cornerRadius;
		}

		//static Color backgroundColor = new Color(232.0/255.0, 233.0/255.0, 236.0/255.0);
		static Color backgroundColor = Color.LightCyan;
		static Color backgroundSelected = Color.LightSkyBlue;
		public Color BackgroundColor {
			get => IsSelected ? backgroundSelected : backgroundColor;
		}

		static Color borderColor = Color.Accent;
		public Color BorderColor {
			get => IsSelected ? borderColor : Color.Transparent;
		}

		public double TextSize {
			get => 20;
		}
		public double SmallTextSize {
			get => 18;
		}

		public static ObservableCollection<ScheduleItem> CreateCollection()
		{
			return new ShadowCollection<ScheduleItem, Ride>(
				AppScope.Instance.Schedule.Rides,
				(ride) => new ScheduleItem(ride)
				);
		}
	}
}
