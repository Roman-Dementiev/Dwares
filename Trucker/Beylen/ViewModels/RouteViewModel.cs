using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;
using System.Collections.ObjectModel;
using Dwares.Dwarf.Collections;
using Xamarin.Forms;

namespace Beylen.ViewModels
{
	public class RouteViewModel : CollectionViewModel<RouteStopCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteModel));

		public static ObservableCollection<RouteStopCardModel> CreateCollection()
		{
			return new ShadowCollection<RouteStopCardModel, RouteStop>(
				AppScope.Instance.Route,
				(stop) => new RouteStopCardModel(stop)
				);
		}

		public RouteViewModel() :
			base(ApplicationScope, CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Route";
			AddCommand = new Command(AddStop);
			RouteMapCommand = new Command(RouteMap);
		}

		public Command AddCommand { get; }
		public Command RouteMapCommand { get; }

		public async void AddStop()
		{
			await Shell.Current.GoToAsync($"routestop");
		}

		public async void RouteMap()
		{
			var exc = await AppScope.Instance.Route.ShowRouteMap();
			if (exc != null) {
				await Alerts.ErrorAlert(exc.Message);
			}

		}
	}
}
