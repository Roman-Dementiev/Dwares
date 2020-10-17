using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using BingMapsRESTToolkit;
using Ziply.Models;
using Dwares.Druid;
using Dwares.Dwarf;

namespace Ziply.ViewModels
{
	public class ZipViewModel : BaseViewModel
	{
		public ZipViewModel() :
			base(expireMinutes: 10)
		{
			Clear();
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

		public bool IsUSA {
			get => isUSA;
			set => SetProperty(ref isUSA, value);
		}
		bool isUSA;


		public override async Task Refresh(bool silent)
		{
			Clear();

			IsBusy = true;
			try {
				var address = await SendRequest();
				LastRefreshed = DateTime.Now;

				UpdateFromAddress(address);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			finally {
				IsBusy = false;
			}
		}

		public override void Clear()
		{
			Zip = City = State = Country = string.Empty;
		}

		void UpdateFromAddress(Address address)
		{
			if (address == null) {
				Clear();
			} else {
				Zip = address.PostalCode ?? string.Empty;

				if (!string.IsNullOrEmpty(address.Locality)) {
					City = address.Locality;
					if (string.IsNullOrEmpty(address.Neighborhood)) {
						City = address.Locality;
					} else {
						City = $"{address.Locality} ({address.Neighborhood})";
					}
				} else {
					if (string.IsNullOrEmpty(address.Neighborhood)) {
						City = string.Empty;
					} else {
						City = "Address.Neighborhood";
					}
				}

				City = address.Locality ?? string.Empty;

				if (!string.IsNullOrEmpty(address.AdminDistrict)) {
					State = address.AdminDistrict;
				} else {
					State = address.AdminDistrict2 ?? string.Empty;
				}

				if (!string.IsNullOrEmpty(address.CountryRegion)) {
					Country = address.CountryRegion;
				} else {
					Country = address.CountryRegionIso2 ?? string.Empty;
				}

				if (!string.IsNullOrEmpty(address.CountryRegionIso2)) {
					IsUSA = address.CountryRegionIso2 == "US";
				} else {
					IsUSA = Country == "United States";
				}

				SetButtonText(Zip);
			}
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

		protected override string GetMessageText()
		{
			if (string.IsNullOrEmpty(Zip))
				return null;

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

			return text;
		}
	}
}
