using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid.UI;
using RouteOptimizer.Models;
using RouteOptimizer.Storage;
using RouteOptimizer.Views;
using RouteOptimizer.ViewModels;


namespace RouteOptimizer
{
	public partial class App : Application
	{
		public const string HospitalsSample = "Hospitals.txt";
		public const string Phila_Ru_Sample = "Phila-ru.txt";


		public static new App Current { get; private set; }

		public App()
		{
			Current = this;

			InitializeComponent();
			Device.SetFlags(new string[] { "RadioButton_Experimental" });

			Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
			Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
			Routing.RegisterRoute(nameof(RoutePage), typeof(RoutePage));
			Routing.RegisterRoute(nameof(PlacesPage), typeof(PlacesPage));
			Routing.RegisterRoute(nameof(PlaceEditPage), typeof(PlaceEditPage));


			UIThemeManager.Instance = new UIThemes();

			//PlaceOrder = Place.CompareByCategory;

			MainPage = new AppShell();
		}

		private static Preloaded Preloaded { get; set; }

		public IAppStorage Storage => AppStorage.Instance;
		public Places Places { get; } = new Places();
		public Route Route { get; } = new Route();

		//public Comparison<Place> PlaceOrder { get; }

		public bool UseInPlaceEditor {
			get => useInPlaceEditor;
			set {
				if (value != useInPlaceEditor) {
					useInPlaceEditor = value;
					OnPropertyChanged();
				}
			}
		}
		bool useInPlaceEditor = false;

		public static async Task PreloadData()
		{
			Preloaded = new Preloaded();
			await Preloaded.Load();
		}

		protected override async void OnStart()
		{
			Place[] places;
			RouteStop[] route;

			if (Preloaded?.IsLoaded == true) {
				places = Preloaded.Places;
				route = Preloaded.Route;
			} else {
				places = await Storage.LoadPlacesAsync();
				route = await Storage.LoadRouteAsync();
			}

			Preloaded = null;
			AddPlaces(places);
			AddStops(route);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}


		public async Task ClearPlaces()
		{
			Places.Clear();
			await Storage.SavePlacesAsync(Places.List);
		}

		public async Task LoadPlaces()
		{
			Places.Clear();
			
			var places = await Storage.LoadPlacesAsync();
			AddPlaces(places);
		}

		void AddPlaces(Place[] places)
		{
			//if (PlaceOrder != null) {
			//	Array.Sort(places, PlaceOrder);
			//}

			using (var batch = new BatchCollectionChange(Places.List)) {
				foreach (var place in places) {
					Places.Add(place);
				}
			}
		}


		public async Task SavePlaces()
		{
			await Storage.SavePlacesAsync(Places.List);
		}

		public Task LoadRoute()
		{
			return Task.CompletedTask;
		}

		void AddStops(RouteStop[] stops)
		{

		}

		public async Task<string> AddPlace(Place place)
		{
			string id = await Storage.AddPlaceAsync(place);
			Debug.Assert(place.Id == id);

			Places.Add(place);
			return id;
		}

		public async Task<string> UpdatePlace(Place place)
		{
			string oldId = place.Id; 
			string newId = await Storage.UpdatePlaceAsync(oldId, place);
			if (newId != oldId) {
				Places.Replace(oldId, place);
			}
			return newId;
		}

		public async Task DeletePlace(Place place)
		{
			if (Places.Remove(place)) {
				await Storage.DeletePlaceAsync(place.Id);
			}
		}

		public async Task LoadSample(string sampleFile, bool skipDuplicates)
		{
			using (var batch = new BatchCollectionChange(Places.List))
			using (var stream = await FileSystem.OpenAppPackageFileAsync(sampleFile))
			using (var reader = new StreamReader(stream)) {
				var text = await reader.ReadToEndAsync();
				var json = JsonStorage.DeserializeJson<PlacesJson>(text);

				//int i = 0;
				foreach (var rec in json.Places) {
					var found = Places.GetByName(rec.Name);
					if (found != null) {
						if (skipDuplicates)
							continue;
						Places.Remove(found);
					}
					var place = JsonStorage.JsonToPlace(rec);
					Places.Add(place);
					//i++;
				}
			}

			await SavePlaces();
		}
	}


	internal class Preloaded
	{
		public Place[] Places { get; private set; }
		public RouteStop[] Route { get; private set; }

		public bool IsLoaded { get; set; }

		public async Task Load()
		{
			if (IsLoaded)
				return;

			var storage = AppStorage.Instance;
			try {
				Places = await storage.LoadPlacesAsync();
				Route = await storage.LoadRouteAsync();
				if (Places != null && Route != null) {
					IsLoaded = true;
				}
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			Places = null;
			Route = null;
		}
	}
}
