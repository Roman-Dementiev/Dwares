using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Views;
using ACE.Models;
using Dwares.Dwarf;
using Dwares.Druid;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ACE
{
	public partial class App : Application
	{
		//MainPage mainPage;

		public App()
		{
			
			BindingContext = new AppScope();
			InitializeComponent();

			//var device = DeviceEx.Instance;
			//Debug.Print("App.App(): Platform={0} Idiom={1} ScreenSize={2} ScaledSie={3} Orientation={4}", 
			//	device.Platform, device.Idiom, device.PixelScreenSize, device.ScaledScreenSize, device.Orientation);

			//var mock = new MockDevice("Lumia 735", Device.UWP, TargetIdiom.Phone, new Size(360, 640));
			//DeviceEx.AddMock(mock);

			MainPage = new MainPage();

			Navigator.Initialize();

			//Drum.DefaultMapAppication = new BingMaps();
			//Drum.DefaultMapService = new BingMapsRest();
		}

		//public static new App Current => Application.Current as App;
		//public static Page CurrentPage => Current.mainPage.CurrentPage;

		protected override async void OnStart()
		{
			await AppStorage.LoadAsync();
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
