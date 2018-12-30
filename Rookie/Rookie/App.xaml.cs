using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid;
using Rookie.ViewModels;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Rookie
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			BindingContext = new AppScope();
			//AppData.Instance.Initialize();

			this.AddDefaultViewLocators();
			this.InitMainPageWithNavigation(typeof(TripsSheetViewModel));
		}

		protected override void OnStart()
		{
			// Handle when your app starts
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
