using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Support;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ACE.Models;
using ACE.Views;


namespace ACE.ViewModels
{
	public class ScheduleViewModel : CollectionViewModel<ScheduleItem>
	{
		//ClassRef @class = new ClassRef(typeof(ScheduleViewModel));

		public ScheduleViewModel() :
			//base(AppScope, AppData.Schedule)
			base(AppScope, new ScheduleItems())
		{
			//Debug.EnableTracing(@class);
		}

		public ObservableCollection<ScheduleItem> Pickups => Items;

		public async void OnAddPickup()
		{
			var page = new PickupDetailPage(null);
			await Navigator.PushModal(page);

		}

		public bool CanEditPickup() => HasSelected();

		public async void OnEditPickup()
		{
			if (Selected != null) {
				var page = new PickupDetailPage(Selected.Source);
				await Navigator.PushModal(page);
			}
		}


		public bool CanDeletePickup() => HasSelected();

		public async void OnDeletePickup()
		{
			if (AppData.Schedule.Remove(Selected?.Source)) {
				await AppStorage.SaveAsync();
			}
		}

		public override void UpdateCommands()
		{
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "EditPickup");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "DeletePickup");
		}
	}

	internal class ScheduleItems : ShadowCollection<ScheduleItem, Pickup>
	{
		public ScheduleItems() :
			base(AppData.Schedule, pickup => new ScheduleItem(pickup))
		{ }
	}


}
