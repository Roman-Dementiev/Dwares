using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid;
using Beylen.Models;
using Xamarin.Forms;


namespace Beylen.ViewModels
{
	public class RouteViewModel : CollectionViewModel<RouteStopCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteModel));

		//public static ObservableCollection<RouteStopCardModel> CreateCollection()
		//{
		//	var collection =  new OrdarableShadowCollection<RouteStopCardModel, RouteStop>(
		//		AppScope.Instance.Route.Stops,
		//		(stop) => new RouteStopCardModel(stop)
		//		);
		//	collection.SameOrder = true;
		//	return collection;
		//}

		public RouteViewModel() :
			base(ApplicationScope, new RouteStopCardCollection())
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
			try {
				await AppScope.Instance.Route.ShowRouteMap();
			}
			catch (Exception exc) {
				await Alerts.ExceptionAlert(exc);
			}

		}

		protected override Task ReloadItems(CollectionViewReloadMode mode)
		{
			return Task.CompletedTask;
		}
	}

	internal class RouteStopCardCollection : OrdarableShadowCollection<RouteStopCardModel, RouteStop>
	{
		public RouteStopCardCollection() :
			base(AppScope.Instance.Route.Stops, (stop) => new RouteStopCardModel(stop))
		{
			//SameOrder = false;
		}

		public override async void ChangeOrdinal(int oldIndex, int newIndex)
		{
			var stop = this[oldIndex].Source;
			if (stop.Kind == RouteStopKind.StartPoint || stop.Kind == RouteStopKind.EndPoint || stop.Status > RoutеStopStatus.Pending)
				return;

			if (newIndex > oldIndex) {
				var prev = this[newIndex].Source;
				if (prev.Kind == RouteStopKind.EndPoint)
					newIndex--;
			}

			while (newIndex < oldIndex) {
				var next= this[newIndex].Source;
				if (next.Status == RoutеStopStatus.Pending)
					break;
				newIndex++;
			}

			if (oldIndex != newIndex) {
				int startIndex= Math.Min(oldIndex, newIndex);
				int startOrdinal  = this[startIndex].Source.Ordinal;

				base.ChangeOrdinal(oldIndex, newIndex);
				await AppScope.Instance.Route.ChangeOrdinals(startIndex, startOrdinal);
			}
		}
	}
}
