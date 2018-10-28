using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Views;
using ACE.Models;
using Dwares.Dwarf;
using Dwares.Druid.Support;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ACE
{
	public partial class App : Application
	{
		MainPage mainPage;

		public App()
		{
			BindingContext = new AppScope();
			InitializeComponent();

			mainPage = new MainPage();
			MainPage = mainPage;

			Navigator.Initialize();
		}

		//public static new App Current => Application.Current as App;
		//public static Page CurrentPage => Current.mainPage.CurrentPage;

		protected override async void OnStart()
		{
			await AppData.LoadAsync();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		//public static INavigation Navigation => CurrentPage.Navigation;

	}
}
