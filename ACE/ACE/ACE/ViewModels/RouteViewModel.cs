using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid.Support;
using ACE.Models;
using ACE.Views;

namespace ACE.ViewModels
{
	public class RouteViewModel : CollectionViewModel<RouteItem>
	{
		public RouteViewModel()
		{
			ParentScope = AppScope;
			Items = new ShadowCollection<RouteItem,RouteStop>(AppData.Route, (routeStop => new RouteItem(routeStop)));
		}

		public void OnShowDirections()
		{
			//Debug.Print("RouteViewModel.OnShowDirections()");

			if (Selected != null) {
				Selected.OnShowDirections();
			}
		}

		public bool CanShowDirections()
		{
			//Debug.Print("RouteViewModel.CanShowDirections()");

			if (Selected != null) {
				return Selected.CanShowDirections();
			}
			else {
				return false;
			}
		}

		public async void OnGoToNextStop()
		{
			//Debug.Print("RouteViewModel.OnGoToNextStop*()");
			await AppData.Route.GoToNextStop();
		}

		public async void OnArriveAtNextStop()
		{
			//Debug.Print("RouteViewModel.OnArriveAtNextStop*()");
			await AppData.Route.ArriveAtNextStop();
		}

		public override void UpdateCommands()
		{
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "ShowDirections");
		}
	}
}
