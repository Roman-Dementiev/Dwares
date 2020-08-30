using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Dwarf;
using Beylen.Storage;
using Dwares.Dwarf.Toolkit;
using System.Threading.Tasks;
using Beylen.Models;


namespace Beylen
{
	public static class AppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AppStorage));

		static IAppStorage instance;
		public static IAppStorage Instance {
			//get => LazyInitializer.EnsureInitialized(ref instance, () => new MockStorage());
			get => LazyInitializer.EnsureInitialized(ref instance, () => new Storage.Air.AirStorage());
			set => instance = value;
		}

		public static async Task<DateOnly?> GetDateProperty(string name, Car car)
		{
			var value = await Instance.GetProperty(name, car);
			if (!string.IsNullOrEmpty(value)) {
				DateOnly date;
				if (DateOnly.TryParse(value, out date))
					return date;
			}
			return null;
		}

		//public static async Task SetDateProperty(string name, DateOnly date)
		//{
		//	await Instance.SetProperty(name, date.ToString());
		//}

		public static async Task<int> GetIntProperty(string name, Car car)
		{
			var value = await Instance.GetProperty(name, car);
			if (!string.IsNullOrEmpty(value)) {
				int i;
				if (int.TryParse(value, out i) && i > 0)
					return i;
			}
			return 0;
		}

		public static async Task SetProperty(string name, Car car, object value)
		{
			await Instance.SetProperty(name, car, value.ToString());
		}

		public static async Task<Stage?> GetStage(Car car)
		{
			var value = await Instance.GetProperty("Stage", car);
			if (value == nameof(Stage.Preparing))
				return Stage.Delivering;
			if (value == nameof(Stage.Delivering))
				return Stage.Delivering;
			if (value == nameof(Stage.ClosingUp))
				return Stage.ClosingUp;
			return null;
		}

		public static async Task SetStage(Car car, Stage stage)
		{
			await Instance.SetProperty("Stage", car, stage.ToString());
		}


		public static async Task<DateOnly?> GetStageDate(Car car) => await GetDateProperty("StageDate", car);
		public static async Task SetStageDate(Car car, DateOnly date) => await SetProperty("StageDate", car, date);

		public static async Task<DateOnly?> GetOrderingDate() => await GetDateProperty("OrderingDate", null);
		public static async Task SetOrderingDate(DateOnly date) => await SetProperty("OrderingDate", null, date);

		//public static async Task<int> GetOrderingLast() => await GetIntProperty("OrderingLast", null);
		//public static async Task SetOrderingLast(int last) => await SetProperty("OrderingLast", null, last);
	}
}
