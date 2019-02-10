using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid;
using Dwares.Druid.Services;
using Dwares.Rookie.ViewModels;
using Dwares.Rookie;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Rookie
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			SecureStorage.Initialize("Dwares.Rookie.Secure");

			this.AddDefaultViewLocators();
			this.InitMainPageWithNavigation(typeof(LoginViewModel));

			//MainPage = new NavigationPage(new LoginPage());;
			//Navigator.Initialize();
		}

		protected override async void OnStart()
		{
			// Handle when your app starts
			await AppData.Instance.Initialize(reset: false);
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
