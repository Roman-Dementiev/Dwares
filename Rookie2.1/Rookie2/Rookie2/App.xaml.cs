using System;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Druid.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Dwares.Rookie
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			FormViewModel.DefaultWidth = 340;
			FormViewModel.DefaultHeight = 500;

			BindingContext = new AppScope();

			SecureStorage.Initialize("Dwares.Rookie.Secure");
			this.AddDefaultViewLocators();
		}

		protected override async void OnStart()
		{
			MainPage = new MainPage();
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
