using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid.ViewModels;
using Drive.Models;
using Dwares.Druid;
using Xamarin.Forms;


namespace Drive.ViewModels
{
	public class SettingsViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(SettingsViewModel));

		public SettingsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Settings";

			SaveCommand = new Command(OnSave);

			ApiKey = Settings.ApiKey;
			BaseId = Settings.BaseId;
			IsModified = false;
		}

		public Command SaveCommand { get; }

		public string ApiKey {
			get => apiKey;
			set => SetProperty(ref apiKey, value);
		}
		string apiKey;

		public string BaseId {
			get => baseId;
			set => SetProperty(ref baseId, value);
		}
		string baseId;

		//public async void OnDisappearing()
		//{
		//	}
		//}

		public async Task<bool> CanGoBack()
		{
			Debug.Print("SettingsModel.CanGoBack()");

			bool proceed = true;
			if (IsModified) {
				proceed = await Alerts.ConfirmAlert("Settings are not saved.\nDo you want to leave without saving?");
			}

			return proceed;
		}

		async void OnSave()
		{
			if (ApiKey != Settings.ApiKey || BaseId != Settings.BaseId) {
				Settings.ApiKey = ApiKey;
				Settings.BaseId = BaseId;

				await App.ReloadData();
			}

			IsModified = false;
		}
	}
}
