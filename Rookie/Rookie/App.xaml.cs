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

			BindingContext = new AppScope();

			SecureStorage.Initialize("Dwares.Rookie.Secure");
			this.AddDefaultViewLocators();
		}

		protected override async void OnStart()
		{
			// Handle when your app starts
			await AppScope.Instance.Initialize(reset: false);

			//var testPage = new Dwares.Rookie.Views.TestPage();
			//this.InitMainPageWithNavigation(testPage);

			if (AppScope.IsLoggedIn) {
				this.InitMainPageWithNavigation(typeof(MainPageViewModel));
			} else {
				this.InitMainPageWithNavigation(typeof(LoginViewModel));
			}
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
