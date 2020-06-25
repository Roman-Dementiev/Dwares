using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Druid.Painting;
using Dwares.Druid.Services;
using Dwares.Druid.Resources;
using Dwares.Druid.UI;
using Drive.ViewModels;
using Drive.Views;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Drive
{
	public partial class App : Application
	{
		public App()
		{
			Dwares.Dwarf.Package.Init();
			Dwares.Druid.Package.Init();

			InitializeComponent();

			Preferences.DefaultShare = "ZenRide";

			BindingContext = AppScope.Instance;
			this.AddDefaultViewLocators();

			UIThemeManager.Instance = new UIThemes();
		}

		protected override async void OnStart()
		{
			//AppStorage.Instance = new Storage.MockStorage();
			AppStorage.Instance = new Storage.Air.AirStorage();
			
			var rootPage = Forge.CreatePage(typeof(MainPageViewModel));
			rootPage.StartSendingPageSizeMessage();

			//this.InitMainPageWithNavigation(rootPage);
			MainPage = new NavigationPageEx(rootPage, "NavigationPage");
			Navigator.Initialize();

			WritCommand.ExecuteWrit("GoToSchedule");

			await AppScope.Instance.Initialize();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
