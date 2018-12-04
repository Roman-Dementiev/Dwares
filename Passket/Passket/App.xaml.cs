using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf.Runtime;
using Dwares.Druid;
using Passket.ViewModels;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Passket
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			BindingContext = new AppScope();

			this.AddDefaultViewLocators();
			this.InitMainPageWithNavigation(typeof(MainPageViewModel));
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
