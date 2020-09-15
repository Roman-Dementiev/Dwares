using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using RouteOptimizer.Models;
using RouteOptimizer.Storage;
using RouteOptimizer.Views;


namespace RouteOptimizer
{
	public partial class App : Application
	{
		//public static new App Current => Application.Current as App;
		public static new App Current { get; private set; }
		public static IAppStorage Storage => Current.AppStorage;

		public App()
		{
			Current = this;

			InitializeComponent();

			Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
			Routing.RegisterRoute(nameof(RoutePage), typeof(RoutePage));
			Routing.RegisterRoute(nameof(PlacesPage), typeof(PlacesPage));

			//DependencyService.Register<MockStorage>();
			//AppStorage = new MockStorage();
			AppStorage = new JsonStorage();

			UIThemeManager.Instance = new UIThemes();

			Route = new Route();
			Places = new Places();

			MainPage = new AppShell();
		}

		public IAppStorage AppStorage { get; }

		public Places Places { get; }
		public Route Route { get; }

		protected override async void OnStart()
		{
			await AppStorage.LoadPlaces(Places);
			await AppStorage.LoadRoute(Route);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}

		public async Task ReloadPlaces()
		{
			Places.Clear();
			await AppStorage.LoadPlaces(Places);
		}

		public async Task AddPlace(Place place)
		{
			Places.Add(place);
			await AppStorage.AddPlace(place);
		}

		public async Task UpdatePlace(Place place)
		{
			await AppStorage.UpdatePlace(place);
		}

		public async Task DeletePlace(Place place)
		{
			if (Places.Remove(place)) {
				await AppStorage.DeletePlace(place);
			}
		}
	}
}
