using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Druid.Painting;
using Dwares.Druid.UI;
using Dwares.Druid.Resources;
using Drive.ViewModels;


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

			BindingContext = AppScope.Instance;
			this.AddDefaultViewLocators();

			//UITheme.Current = UITheme.ByName("Dark");
			UITheme.Current = UITheme.ByName("Light");
			//UITheme.Current = UITheme.ByName("Oceanic");
		}

		protected override async void OnStart()
		{
			//AppStorage.Instance = new Storage.MockStorage();
			AppStorage.Instance = new Storage.Air.AirStorage();

			var rootPage = AppScope.CreatePage(typeof(RootViewModel));
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
