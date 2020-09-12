using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf.Collections;
using DraggableListView.Models;
using DraggableListView.Services;
using DraggableListView.Views;

namespace DraggableListView
{
	public partial class App : Application
	{

		public static new App Current => Application.Current as App;

		public App()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(MarinesPage), typeof(MarinesPage));
			Routing.RegisterRoute(nameof(SquadPage), typeof(SquadPage));
			Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));

			DependencyService.Register<MockDataStore>();

			Squad = new Squad();
			Squad.Teams.AutoOrdinals = true;

			Marines = new OrderableCollection<Marine>();
			Marines.AutoOrdinals = true;

			MainPage = new AppShell();
		}

		public Squad Squad { get; } = new Squad();
		public OrderableCollection<Marine> Marines { get; }

		protected override async void OnStart()
		{
			await DataStore.Instance.LoadSquad(Squad);
			await DataStore.Instance.LoadMarines(Marines);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
