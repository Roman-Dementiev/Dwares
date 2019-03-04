using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid;
using Dwares.Druid.UI;
using Dwares.Druid.Forms;
using Dwares.Druid.Services;
using Dwares.Rookie.ViewModels;
using Dwares.Rookie.Views;
using Dwares.Rookie;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Dwares.Rookie
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

			Page startPage;
			if (AppScope.IsLoggedIn) {
				startPage = CreateForm<HomeViewModel>();
			} else {
				startPage = CreateForm<LoginViewModel>();
			}
			this.InitMainPageWithNavigation(startPage);
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		public static Page CreateForm<ViewModel>() where ViewModel : ViewModels.FormViewModel
		{
			var page = Forge.CreateContentPage<FramedPage>(typeof(ViewModel));
			return page;
		}
	}
}
