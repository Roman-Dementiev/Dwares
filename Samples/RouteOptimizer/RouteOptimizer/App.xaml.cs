﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using RouteOptimizer.Models;
using RouteOptimizer.Storage;
using RouteOptimizer.Views;
using RouteOptimizer.ViewModels;
using Xamarin.Essentials;
using System.IO;

namespace RouteOptimizer
{
	public partial class App : Application
	{
		public const string HospitalsSample = "Hospitals.txt";


		//public static new App Current => Application.Current as App;
		public static new App Current { get; private set; }
		public static IAppStorage Storage => Current.AppStorage;

		public App()
		{
			Current = this;

			InitializeComponent();

			Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
			Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
			Routing.RegisterRoute(nameof(RoutePage), typeof(RoutePage));
			Routing.RegisterRoute(nameof(PlacesPage), typeof(PlacesPage));
			Routing.RegisterRoute(nameof(PlaceEditPage), typeof(PlaceEditPage));

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

		public bool UseInPlaceEditor {
			get => useInPlaceEditor;
			set {
				if (value != useInPlaceEditor) {
					useInPlaceEditor = value;
					OnPropertyChanged();
				}
			}
		}
		bool useInPlaceEditor;

		protected override async void OnStart()
		{
			await AppStorage.LoadPlacesAsync(Places);
			await AppStorage.LoadRouteAsync(Route);
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
			await AppStorage.LoadPlacesAsync(Places);
		}

		public async Task ClearPlaces()
		{
			Places.Clear();
			await AppStorage.SavePlacesAsync(Places);
		}

		public async Task SavePlaces()
		{
			await AppStorage.SavePlacesAsync(Places);
		}

		public async Task AddPlace(Place place)
		{
			Places.Add(place);
			await AppStorage.AddPlaceAsync(place);
		}

		public async Task UpdatePlace(Place place)
		{
			await AppStorage.UpdatePlaceAsync(place);
		}

		public async Task DeletePlace(Place place)
		{
			if (Places.Remove(place)) {
				await AppStorage.DeletePlaceAsync(place);
			}
		}

		public async Task LoadSample(string sampleFile, bool skipDuplicates)
		{
			using (var stream = await FileSystem.OpenAppPackageFileAsync(sampleFile))
			using (var reader = new StreamReader(stream)) {
				var text = await reader.ReadToEndAsync();
				var json = JsonStorage.DeserializeJson<PlacesJson>(text);

				int i = 0;
				foreach (var rec in json.Places) {
					var found = Places.GetByName(rec.Name);
					if (found != null) {
						if (skipDuplicates)
							continue;
						Places.Remove(found);
					}
					Places.Add(new Place {
						Name = rec.Name,
						Tags = rec.Tags,
						Address = rec.Address
					});
					i++;
				}
			}

			await SavePlaces();
		}
	}
}
