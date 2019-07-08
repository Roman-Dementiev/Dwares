using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Druid.UI;
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


			var test1 = new MaterialColorPalette();
			Debug.Print($"test1={test1}");

			var test2 = new Themes.Colors.MaterialGray();
			var primary = test2.PrimaryColor;
			var secondary = test2.SecondaryColor;
			Debug.Print($"test2={test2}");

			//new UITheme(new Themes.BaseTheme());
			UITheme.Current = new UITheme(new Themes.Default());

			//var baseTheme = new UITheme(new Themes.BaseTheme());
			////UITheme.Current = new UITheme(new Themes.Cold(), baseTheme);
			///

			//new ColorPalette(new Themes.Material.MaterialPalette());
			//new Dwares.Druid.Satchel.ColorScheme(new Themes.Material.Teal());

			new UITheme(new Themes.BaseTheme());
			UITheme.Current = new UITheme(new Themes.Light());
		}

		protected override async void OnStart()
		{
			//AppStorage.Instance = new Storage.MockStorage();
			AppStorage.Instance = new Storage.Air.AirStorage();

			//var page = AppScope.CreatePage(typeof(ScheduleViewModel));
			//this.InitMainPageWithNavigation(page);

			//var homePage = Forge.CreatePage(typeof(HomeViewModel));
			//this.InitMainPageWithNavigation(homePage);

			var page = AppScope.CreatePage(typeof(RootViewModel));
			page.StartSendingPageSizeMessage();
			this.InitMainPageWithNavigation(page);
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
