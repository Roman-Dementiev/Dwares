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
			InitializeComponent();

			BindingContext = AppScope.Instance;
			this.AddDefaultViewLocators();

			new MaterualDarkColorScheme();

			new UITheme(new Themes.BaseTheme());
			//UITheme.Current = new UITheme(new Themes.Default());
			UITheme.Current = new Themes.DarkTheme();
		}

		protected override async void OnStart()
		{
			//AppStorage.Instance = new Storage.MockStorage();
			AppStorage.Instance = new Storage.Air.AirStorage();

			//var page = AppScope.CreatePage(typeof(ScheduleViewModel));
			//this.InitMainPageWithNavigation(page);

			//var homePage = Forge.CreatePage(typeof(HomeViewModel));
			//this.InitMainPageWithNavigation(homePage);

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
