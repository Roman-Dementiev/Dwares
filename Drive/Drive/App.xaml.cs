using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid;
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

			UITheme.Current = new UITheme(new Themes.Default());
		}

		protected override async void OnStart()
		{
			var homePage = Forge.CreatePage(typeof(HomeViewModel));
			this.InitMainPageWithNavigation(homePage);

			var page = AppScope.CreatePage(typeof(ScheduleViewModel));
			await Navigator.PushPage(page);

			AppStorage.Instance = new Storage.MockStorage();
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
