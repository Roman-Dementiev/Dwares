using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using BingMapsRESTToolkit;
using System.Diagnostics;

namespace Ziply
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public const string UnknownZip = "?????";

		const string BMK = "At3kj4rBGQ5lVXSMcxAoYc7AQ2tLFhbyfikyPfaEbXuw03XiRTGCWAdYeiUzqFNa";

		public ViewModel()
		{
			ExpirePeriod = TimeSpan.FromMinutes(10);
			RefreshCommand = new Command(async () => await Refresh());
			ShareCommand = new Command(async () => await Share());
		}

		public TimeSpan ExpirePeriod { get; set; }
		public DateTime LastRefreshed { get; private set; }

		public Command RefreshCommand { get; set; }
		public Command ShareCommand { get; set; }

		public bool ShowCheckboxes {
			get => false;
		}

		public string Zip {
			get => zip;
			set => SetProperty(ref zip, value);
		}
		string zip;

		public string City {
			get => city;
			set => SetProperty(ref city, value);
		}
		string city;

		public string State {
			get => state;
			set => SetProperty(ref state, value);
		}
		string state;

		public string Country {
			get => country;
			set => SetProperty(ref country, value);
		}
		string country;

		public Address Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		Address address;

		public bool IsUSA {
			get => isUSA;
			set => SetProperty(ref isUSA, value);
		}
		bool isUSA;

		//public bool IsExpired {
		//public bool IsExpired {
		//	get => CheckIfExpired();
		//}

		public bool IsBusy {
			get => isBusy;
			set => SetPropertyEx(ref isBusy, value, nameof(IsBusy), nameof(IsReady));
		}
		bool isBusy;

		public bool IsReady {
			get => !IsBusy && !string.IsNullOrEmpty(Zip);
		}
		
		public bool CheckIfExpired()
		{
			var now = DateTime.Now;
			if (now > LastRefreshed) {
				return (now - LastRefreshed) >= ExpirePeriod;
			} else {
				return false;
			}
		}

		public async Task Refresh()
		{
			Zip = UnknownZip;
			City = State = Country = string.Empty;
			IsBusy = true;

			try {
				Address = await SendRequest();
				UpdateFromAddress();
				LastRefreshed = DateTime.Now;
			}
			catch (Exception exc) {
				Debug.WriteLine($"## ExceptionCaught: {exc.Message}");
				Address = null;
			}
			finally {
				IsBusy = false;
			}
		}

		public async Task  RefreshIfExpired()
		{
			if (CheckIfExpired()) {
				await Refresh();
			}
		}

		void UpdateFromAddress()
		{
			Zip = !string.IsNullOrEmpty(Address.PostalCode) ? Address.PostalCode : UnknownZip;

			if (!string.IsNullOrEmpty(Address.Locality)) {
				City = Address.Locality;
				if (string.IsNullOrEmpty(Address.Neighborhood)) {
					City = Address.Locality;
				} else {
					City = $"{Address.Locality} ({Address.Neighborhood})";
				}
			} else {
				if (string.IsNullOrEmpty(Address.Neighborhood)) {
					City = string.Empty;
				} else {
					City = "Address.Neighborhood";
				}
			}

			City = Address.Locality ?? string.Empty;

			if (!string.IsNullOrEmpty(Address.AdminDistrict)) {
				State = Address.AdminDistrict;
			} else {
				State = Address.AdminDistrict2 ?? string.Empty;
			}

			if (!string.IsNullOrEmpty(Address.CountryRegion)) {
				Country = Address.CountryRegion;
			} else {
				Country = Address.CountryRegionIso2 ?? string.Empty;
			}

			if (!string.IsNullOrEmpty(Address.CountryRegionIso2)) {
				IsUSA = Address.CountryRegionIso2 == "US";
			} else {
				IsUSA = Country == "United States";
			}
		}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
		{
			return SetPropertyEx(ref storage, value, propertyName);
		}

		protected bool SetPropertyEx<T>(ref T storage, T value, params string[] changedProperties)
		{
			if (Object.Equals(storage, value))
				return false;
			if (EqualityComparer<T>.Default.Equals(storage, value))
				return false;

			storage = value;

			if (PropertyChanged != null) {
				foreach (var propertyName in changedProperties) {
					if (propertyName != null) {
						PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
					}
				}
			}
			return true;
		}

		async Task<Address> SendRequest()
		{
			return await SendRequest(null, true, true);
		}

		async Task<Address> SendRequest(bool includeIso2, bool includeNeighborhood, params EntityType[] entityTypes)
		{
			return await SendRequest(entityTypes, includeIso2, includeNeighborhood);
		}

		async Task<Address> SendRequest(IEnumerable<EntityType> entityTypes, bool includeIso2, bool includeNeighborhood)
		{
			var geoLocation = await Geolocation.GetLocationAsync();

			var request = new ReverseGeocodeRequest {
				Point = new Coordinate(geoLocation.Latitude, geoLocation.Longitude),
				IncludeIso2 = includeIso2,
				IncludeNeighborhood = includeNeighborhood,
				BingMapsKey = BMK
			};

			if (entityTypes != null) {
				request.IncludeEntityTypes = new List<EntityType>(entityTypes);
			}

			var response = await request.Execute();

			foreach (var rs in response.ResourceSets) {
				int i = 0;
				foreach (var resource in rs.Resources) {
					var location = resource as BingMapsRESTToolkit.Location;
					if (location != null) {
						return location.Address;
					}
				}
			}

			return null;
		}

		async Task Share()
		{
			if (string.IsNullOrEmpty(Zip))
				return;

			string text = $"{Zip}";

			string info;
			if (!string.IsNullOrEmpty(City)) {
				if (string.IsNullOrEmpty(State)) {
					info = $"{City}";
				} else {
					info = $"{City}, {State}";
				}
			} else if (!string.IsNullOrEmpty(State)) {
				info = $"{State}";
			} else {
				info = string.Empty;
			}


			if (info.Length > 0) {
				if (!IsUSA) {
					info += $", {Country}";
				}
				text += $"\n{info}";
			}


			await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest {
				Subject = "Current location",
				Text = text
			});
		}
	}
}
