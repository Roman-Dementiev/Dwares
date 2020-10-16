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
			Address = null;
			UpdateFromAddress();
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


		public override async Task Refresh(bool silent)
		{
			Address = null;
			UpdateFromAddress();

			IsBusy = true;
			try {
				Address = await SendRequest();
				LastRefreshed = DateTime.Now;
				UpdateFromAddress();
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			finally {
				IsBusy = false;
			}
		}


		void UpdateFromAddress()
		{
			if (Address == null) {
				Zip = City = State = Country = string.Empty;
			} else {
				Zip = Address.PostalCode ?? string.Empty;

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
