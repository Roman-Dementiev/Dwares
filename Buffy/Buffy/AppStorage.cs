using System;
using System.Threading.Tasks;
using Buffy.Models;
using Buffy.Storage;


namespace Buffy
{
	public static class AppStorage
	{
		static IAppStorage instance;
		public static IAppStorage Instance {
			//get => instance ??= new Storage.MockStorage();
			get => instance ??= new Storage.Air.AirStorage();
			set => instance = value;
		}

		public static Task Initialize()
		{
			var providers = App.Vendors;
			providers.Add(new FuelVendor { Name = "Pilot", Icon = "ic_gp_pilot.png" });
			providers.Add(new FuelVendor { Name = "Flying J", Icon = "ic_gp_flying_j.png" });
			providers.Add(new FuelVendor { Name = "Loves", Icon = "ic_gp_loves.png" });
			providers.Add(new FuelVendor { Name = "TA", Icon = "ic_gp_ta.png" });
			providers.Add(new FuelVendor { Name = "Petro", Icon = "ic_gp_petro.png" });
			providers.Add(new FuelVendor { Name = "BP", Icon = "ic_gp_bp.png" });
			providers.Add(new FuelVendor { Name = "Speedway", Icon = "ic_gp_speedway.png" });
			providers.Add(new FuelVendor { Name = "Sunoco", Icon = "ic_gp_sunoco.png" });
			providers.Add(new FuelVendor { Name = "Shell", Icon = "ic_gp_shell.png" });
			providers.Add(new FuelVendor { Name = "Marathon", Icon = "ic_gp_marathon.png" });
			providers.Add(new FuelVendor { Name = "Chevron", Icon = "ic_gp_chevron.png" });
			providers.Add(new FuelVendor { Name = "QT", Icon = "ic_gp_qt.png" });
			providers.Add(new FuelVendor { Name = "Wawa", Icon = "ic_gp_wawa.png" });
			providers.Add(new FuelVendor { Name = "Royal Farms", Icon = "ic_gp_royal_farms.png" });
			providers.Add(new FuelVendor { Name = "Cumberland Farms", Icon = "ic_gp_cumberland_farms.png" });

			return Instance.Initialize();
		}
	}
}
