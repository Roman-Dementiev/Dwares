using System;
using Dwares.Dwarf;
using Dwares.Druid.Support;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ACE.Models;
using ACE.Views;


namespace ACE.ViewModels
{
	public class ScheduleViewModel : CollectionViewModel<Pickup>
	{
		//ClassRef @class = new ClassRef(typeof(ScheduleViewModel));

		public ScheduleViewModel() :
			base(AppScope, AppData.Schedule)
		{
			//Debug.EnableTracing(@class);
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
