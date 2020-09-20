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
		public static IAppStorage Storage => Current.AppStorage;

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

			if (Preloaded != null) {
				Data = Preloaded;
				Preloaded = null;
			} else {
				Data = new Data();
			}

			MainPage = new AppShell();
		}

		private static Data Preloaded { get; set; }
		private Data Data { get; set; }

		public IAppStorage AppStorage => Data.AppStorage;
		public Places Places => Data.Places;
		public Route Route => Data.Route;

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
			Preloaded = new Data();
			await Preloaded.Load();
		}

		protected override async void OnStart()
		{
			await Data.Load();
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
			await AppStorage.SavePlacesAsync(Places);
		}

		public async Task LoadPlaces()
		{
			Places.Clear();
			await Data.LoadPlaces();
		}

		public async Task SavePlaces()
		{
			await AppStorage.SavePlacesAsync(Places);
		}


		public async Task AddPlace(Place place)
		{
			string id = await AppStorage.AddPlaceAsync(place);
			Debug.Assert(place.Id == id);

			Places.Add(place);
		}

		public async Task UpdatePlace(Place place)
		{
			string oldId = place.Id; 
			string newId = await AppStorage.UpdatePlaceAsync(oldId, place);
			if (newId != oldId) {
				Places.Replace(oldId, place);
			}
		}

		public async Task DeletePlace(Place place)
		{
			if (Places.Remove(place)) {
				await AppStorage.DeletePlaceAsync(place.Id);
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


	internal class Data
	{
		public readonly IAppStorage AppStorage = new JsonStorage();
		public readonly Places Places = new Places();
		public readonly Route Route = new Route();

		public bool IsLoaded { get; set; }

		public async Task Load()
		{
			if (IsLoaded)
				return;

			try {
				await LoadPlaces();
				await LoadRoute();
			} catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			IsLoaded = true;
		}

		public async Task LoadPlaces()
		{
			var places = new List<Place>();
			await AppStorage.LoadPlacesAsync(places);

			using (var batch = new BatchCollectionChange(Places.List)) {
				foreach (var place in places) {
					Places.Add(place);
				}
			}
		}

		public async Task LoadRoute()
		{
			await AppStorage.LoadRouteAsync(Route);
		}
	}
}
