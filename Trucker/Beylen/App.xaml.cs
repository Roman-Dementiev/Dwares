using System;
using Dwares.Druid.UI;
using Dwares.Drums;
using Dwares.Drums.Google;
using Dwares.Drums.Bing;
using Xamarin.Forms;
using Xamarin.Essentials;
using Beylen.Models;

namespace Beylen
{
	public partial class App : Application
	{
		//public AppShell AppShell { get; }

		public App()
		{
			Dwares.Dwarf.Package.Init();
			Dwares.Druid.Package.Init();

			InitializeComponent();

			//Preferences.Clear(Settings.cShare);

			BindingContext = AppScope.Instance;
			//this.AddDefaultViewLocators();

			Device.SetFlags(new string[] { "RadioButton_Experimental" });

			UIThemeManager.Instance = new UIThemes();

			Maps.MapApplication = new GoogleMaps();
			Maps.MapService = new BingMapsREST();
			Preferences.Set("BingMaps.Key", "At3kj4rBGQ5lVXSMcxAoYc7AQ2tLFhbyfikyPfaEbXuw03XiRTGCWAdYeiUzqFNa", Settings.cShare);
		}

		protected override async void OnStart()
		{
			AppStorage.Instance = new Storage.Air.AirStorage();

			var appScope = AppScope.Instance;
			appScope.Configure();

			await appScope.Initialize();
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
