using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Druid.Support;
using ACE.Models;
using ACE.Views;


namespace ACE.ViewModels
{
	public class PickupsListViewModel: CollectionViewModel<Pickup>
	{
		public PickupsListViewModel() :
			base(AppScope, AppData.Schedule)
		{
		}

		public ObservableCollection<Pickup> Pickups => Items;

		public async void OnAddPickup() => await AddOrEdit(null);

		public async void OnEditPickup() => await AddOrEdit(Selected);

		private Task AddOrEdit(Pickup pickup)
		{
			var page = new PickupDetailPage(pickup);
			return Navigator.PushModal(page);
		}

		public bool CanEditPickup() => HasSelected();

		public async void OnDeletePickup()
		{
			if (AppData.Schedule.Remove(Selected)) {
				await AppStorage.SaveAsync();
			}
		}

		public bool CanDeletePickup() => HasSelected();

		public override void UpdateCommands()
		{
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "EditPickup");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "DeletePickup");
		}
	}
}
