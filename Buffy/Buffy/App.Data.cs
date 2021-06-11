using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Buffy.Models;
using Xamarin.Forms;


namespace Buffy
{
	public partial class App : Application
	{
		public static List<FuelVendor> Vendors { get; } = new List<FuelVendor>();
		public static ObservableCollection<Fueling> Fuelings { get; } = new ObservableCollection<Fueling>();

		public static async Task InitData()
		{
			await AppStorage.Initialize();
			await AppStorage.Instance.LoadData();
		}

		public static FuelVendor GetVendor(string name, bool create = true)
		{
			var provider = Vendors.FirstOrDefault((provider) => string.Compare(provider.Name, name, ignoreCase: true) == 0);
			if (provider == null && create) {
				provider = new FuelVendor {
					Name = name
				};
			}
			return provider;
		}

		public static Fueling GetFueling(string id)
		{
			return Fuelings.FirstOrDefault((fueling) => fueling.Id == id);
		}

		public static async Task NewFueling(Fueling fueling)
		{
			await AppStorage.Instance.AddFueling(fueling);

			Fuelings.Add(fueling); 
		}

		public static async Task UpdateFueling(Fueling oldFueling, Fueling newFueling)
		{
			await AppStorage.Instance.UpdateFueling(oldFueling, newFueling);

			oldFueling.Date = newFueling.Date;
			oldFueling.Vendor = newFueling.Vendor;
			oldFueling.State = newFueling.State;
			oldFueling.Gallons = newFueling.Gallons;
			oldFueling.Price = newFueling.Price;
			oldFueling.Total = newFueling.Total;
		}

		public static async Task DeleteFueling(Fueling fueling)
		{
			if (fueling == null)
				return;
			
			Fuelings.Remove(fueling);

			if (!fueling.IsNew) {
				await AppStorage.Instance.DeleteFueling(fueling);
			}
		}
	}
}
