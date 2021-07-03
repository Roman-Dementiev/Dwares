using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Buffy.Models
{
	public class Fueling : Record
	{
		//static ClassRef @class = new ClassRef(typeof(Fueling));

		public Fueling()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id {
			get => id;
			set => SetProperty(ref id, value);
		}
		string id;

		public bool IsNew {
			get => string.IsNullOrEmpty(Id);
		}

		public DateOnly Date {
			get => date;
			set => SetProperty(ref date, value);
		}
		DateOnly date;

		public FuelVendor Vendor {
			get => vendor;
			set => SetPropertyEx(ref vendor, value, 
				nameof(Vendor), nameof(VendorName), nameof(VendorIcon));
		}
		FuelVendor vendor;

		public string State {
			get => state;
			set => SetPropertyEx(ref state, value,
				nameof(State), nameof(VendorName));
		}
		string state;

		public string VendorIcon {
			get => Vendor.Icon ?? "ic_gas_station.png";
		}

		public string VendorName {
			get {
				var name = string.IsNullOrEmpty(Vendor?.Name) ? "??" : Vendor?.Name;
				if (!string.IsNullOrEmpty(State)) {
					name = $"{name}, {State}";
				}
				return name;
			}
		}

		//public decimal EstimatedGallons {
		//	get => estimatedGallons;
		//	set => SetProperty(ref estimatedGallons, value);
		//}
		//decimal estimatedGallons;
	}
}
