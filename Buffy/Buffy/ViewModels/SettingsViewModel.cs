using System;
using Dwares.Druid.ViewModels;
using Dwares.Dwarf;
using Buffy.Models;


namespace Buffy.ViewModels
{
	public class SettingsViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(SettingsViewModel));

		public SettingsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Settings";

			ApiKey = Settings.ApiKey;
			BaseId = Settings.BaseId;
		}

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

		public async void OnDisappearing()
		{
			if (ApiKey != Settings.ApiKey || BaseId != Settings.BaseId) {
				Settings.ApiKey = ApiKey;
				Settings.BaseId = BaseId;

				await App.ReloadData();
			}
		}
	}
}
