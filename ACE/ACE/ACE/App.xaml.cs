using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Views;
using ACE.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ACE
{
	public partial class App : Application
	{
		MainPage mainPage;

		public App()
		{
			InitializeComponent();


			MainPage = mainPage = new MainPage();
		}

		public static new App Current => Application.Current as App;
		public static Page CurrentPage => Current.mainPage.CurrentPage;

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

		public static async Task Alert(string title, string message, string dismiss)
		{
			await Current.MainPage.DisplayAlert(title, message, dismiss);
		}

		public static async Task<bool> Alert(string title, string message, string accept, string cancel)
		{
			return await Current.MainPage.DisplayAlert(title, message, accept, cancel);
		}

		public static async Task ErrorAlert(string message, string dismiss = "OK")
		{
			await Alert("Error", message, dismiss);
		}
	}
}
