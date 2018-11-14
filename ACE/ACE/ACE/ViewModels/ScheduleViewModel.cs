using System;
using Dwares.Druid;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid.Satchel;
using ACE.Models;
using ACE.Views;


namespace ACE.ViewModels
{
	public class ScheduleViewModel : CollectionViewModel<ScheduleItem>
	{
		//ClassRef @class = new ClassRef(typeof(ScheduleViewModel));

		public ScheduleViewModel() :
			base(AppScope, new ScheduleItems())
		{
			//Debug.EnableTracing(@class);
		}

		//public ObservableCollection<ScheduleItem> Pickups => Items;

		public async void OnNewAppoitment()
		{
			//var page = new PickupDetailPage(null);
			var page = new RunDetailPage();
			page.Scope = new AppoitmentViewModel(null);

			await Navigator.PushModal(page);

		}

		public bool CanEdit() => HasSelected();

		public async void OnEdit()
		{
			if (Selected != null) {
				//var page = new PickupDetailPage(Selected.Source);
				var page = new RunDetailPage();

				if (Selected.Source.SheduleRunType == SheduleRunType.Appoitment) {
					page.Scope = new AppoitmentViewModel(Selected.Source);
				} else {
					// TODO
					Debug.Fail();
					return;
				}

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

	internal class ScheduleItems : ShadowCollection<ScheduleItem, ScheduleRun>
	{
		public ScheduleItems() :
			base(AppData.Schedule, scheduleRun => new ScheduleItem(scheduleRun))
		{ }
	}


}
