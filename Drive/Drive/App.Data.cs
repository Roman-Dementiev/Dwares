using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Xamarin.Forms;
using Drive.Storage;
using Drive.Storage.Air;
using Drive.Models;


namespace Drive
{
	public partial class App : Application
	{
		public static IAppStorage Storage {
			get => storage ??= new AirStorage();
			set => storage = value;
		}
		static IAppStorage storage;


		public static bool StorageIsAvailable
		{
			get => storageIsAvailable;
			set {
				if (value != storageIsAvailable) {
					storageIsAvailable = value;
					(Current as App).OnPropertyChanged();
				}
			}
		}
		static bool storageIsAvailable;

		public static async Task InitData(bool silent = false)
		{
			var apiKey = Settings.ApiKey;
			var baseId = Settings.BaseId;
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(baseId)) {
				if (!silent) {
					await Alerts.DisplayAlert("", "Go to Settings and enter Database API key and Base Id");
				}
				return;
			}

			try {
				await Storage.Initialize();
				StorageIsAvailable = true;

				await Storage.LoadData();
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				await Alerts.ExceptionAlert(exc);
			}
		}

		public static async Task ReloadData()
		{
			StorageIsAvailable = false;

			if (storage != null) {
				storage.Dispose();
				storage = null;
			}

			await InitData();
		}
	}
}
