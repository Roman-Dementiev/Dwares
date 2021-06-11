using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Buffy.Models;


namespace Buffy.Storage
{
	public class MockStorage : IAppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(MockStorage));

		public MockStorage()
		{
			//Debug.EnableTracing(@class);
		}

		public Task Initialize()
		{
			return Task.CompletedTask;
		}

		static Task Delay()
		{
			//return Task.Delay(1000);
			return Task.CompletedTask;
		}

		public async Task LoadData()
		{
			var fuelings = App.Fuelings;


			fuelings.Add(new Fueling {
				Id = "1",
				Date = new DateOnly(2021, 1, 7),
				Vendor = App.GetVendor("Cumberland Farms"),
				State = "NY",
				Total = 40.50M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "2",
				Date = new DateOnly(2021, 4, 1),
				Vendor = App.GetVendor("TA"),
				State = "MD",
				Gallons = 12.926M,
				Price = 3.009M,
				Total = 41.90M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "3",
				Date = new DateOnly(2021, 4, 6),
				Vendor = App.GetVendor("Chevron"),
				State = "FL",
				Total = 49.44M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "4",
				Date = new DateOnly(2021, 4, 14),
				Vendor = App.GetVendor("QT"),
				State = "GA",
				Gallons = 11.677M,
				Price = 2.799M,
				Total = 32.68M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "5",
				Date = new DateOnly(2021, 4, 15),
				Vendor = App.GetVendor("BP"),
				State = "MD",
				Total = 47.72M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "6",
				Date = new DateOnly(2021, 4, 16),
				Vendor = App.GetVendor("Royal Farms"),
				State = "VA",
				Gallons = 13.313M,
				Price = 2.999M,
				Total = 39.93M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "7",
				Date = new DateOnly(2021, 4, 29),
				Vendor = App.GetVendor("Wawa"),
				State = "PA",
				Gallons = 13.594M,
				Price = 3.417M,
				Total = 47.29M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "8",
				Date = new DateOnly(2021, 5, 22),
				Vendor = App.GetVendor("Petro"),
				State = "VA",
				Gallons = 19.1547M,
				Price = 3.270M,
				Total = 62.61M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "9",
				Date = new DateOnly(2021, 4, 23),
				Vendor = App.GetVendor("Marathon"),
				State = "MD",
				Total = 38.80M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "10",
				Date = new DateOnly(2021, 5, 25),
				Vendor = App.GetVendor("Loves"),
				State = "MO",
				Gallons = 16.077M,
				Price = 3.189M,
				Total = 51.27M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "11",
				Date = new DateOnly(2021, 5, 25),
				Vendor = App.GetVendor("Loves"),
				State = "MO",
				Gallons = 16.077M,
				Price = 3.189M,
				Total = 51.27M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "12",
				Date = new DateOnly(2021, 5, 26),
				Vendor = App.GetVendor("Pilot"),
				State = "OH",
				Gallons = 16.130M,
				Price = 3.470M,
				Total = 55.95M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "13",
				Date = new DateOnly(2021, 5, 28),
				Vendor = App.GetVendor("Shell"),
				State = "CT",
				Gallons = 12.566M,
				Price = 2.999M,
				Total = 537.69M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "14",
				Date = new DateOnly(2021, 5, 30),
				Vendor = App.GetVendor("Pilot"),
				State = "NY",
				Gallons = 11.354M,
				Price = 3.470M,
				Total = 39.39M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "15",
				Date = new DateOnly(2021, 6, 3),
				Vendor = App.GetVendor("Flying J"),
				State = "PA",
				Gallons = 14.776M,
				Price = 3.870M,
				Total = 51.17M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "16",
				Date = new DateOnly(2021, 6, 4),
				Vendor = App.GetVendor("Speedway"),
				State = "KY",
				Gallons = 10.020M,
				Price = 3.099M,
				Total = 33.56M
			});
			await Delay();

			fuelings.Add(new Fueling {
				Id = "17",
				Date = new DateOnly(2021, 6, 5),
				Vendor = App.GetVendor("Sunoco"),
				State = "PA",
				Gallons = 15.326M,
				Price = 3.519M,
				Total = 53.93M
			});
			await Delay();
		}

		public Task AddFueling(Fueling fueling) => throw new NotImplementedException();
		public Task UpdateFueling(Fueling oldFueling, Fueling newFueling) => throw new NotImplementedException();
		public Task DeleteFueling(Fueling fueling) => throw new NotImplementedException();
	}

}
