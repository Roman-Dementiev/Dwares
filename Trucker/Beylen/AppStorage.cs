using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Dwarf;
using Beylen.Storage;
using Dwares.Dwarf.Toolkit;
using System.Threading.Tasks;

namespace Beylen
{
	public static class AppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AppStorage));

		static IAppStorage instance;
		public static IAppStorage Instance {
			get => LazyInitializer.EnsureInitialized(ref instance, () => new MockStorage());
			set => instance = value;
		}

		public static DateOnly? GetDateProperty(string name)
		{
			var value = Instance.GetProperty(name);
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

		public static int GetIntProperty(string name)
		{
			var value = Instance.GetProperty(name);
			if (!string.IsNullOrEmpty(value)) {
				int i;
				if (int.TryParse(value, out i) && i > 0)
					return i;
			}
			return 0;
		}

		public static async Task SetProperty(string name, object value)
		{
			await Instance.SetProperty(name, value.ToString());
		}

		public static DateOnly? GetClosedDate() => GetDateProperty("ClosedDate");
		public static async Task SetClosedDate(DateOnly date) => await SetProperty("ClosedDate", date);

		public static DateOnly? GetOrderingDate() => GetDateProperty("OrderingDate");
		public static async Task SetOrderingDate(DateOnly date) => await SetProperty("OrderingDate", date);

		public static int GetOrderingLast() => GetIntProperty("OrderingLast");
		public static async Task SetOrderingLast(int last) => await SetProperty("OrderingLast", last);
	}
}
