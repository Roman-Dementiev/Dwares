using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid;
using Dwares.Druid.UI;
using Dwares.Druid.Services;
using Dwares.Drums;
using Dwares.Drums.Google;
using Dwares.Drums.Bing;
using Beylen.Models;
using Beylen.Views;


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

			Preferences.DefaultShare = "Beylen";
			//Preferences.Clear();

			//DependencyService.Register<MockDataStore>();

			BindingContext = AppScope.Instance;
			//this.AddDefaultViewLocators();

			UIThemeManager.Instance = new UIThemes();

			Maps.MapApplication = new GoogleMaps();
			Maps.MapService = new BingMapsREST();
			Preferences.Set("BingMaps.Key", "At3kj4rBGQ5lVXSMcxAoYc7AQ2tLFhbyfikyPfaEbXuw03XiRTGCWAdYeiUzqFNa");
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
