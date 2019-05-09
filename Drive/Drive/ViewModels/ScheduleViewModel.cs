using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ScheduleViewModel : CollectionViewModel<ScheduleTrip>
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleViewModel));

		public ScheduleViewModel() : this(ApplicationScope, AppScope.Instance.Schedule.Trips) { }

		private ScheduleViewModel(BindingScope parentScope, ObservableCollection<ScheduleTrip> trips) :
			base(parentScope, AppScope.Instance.Schedule.Trips)
		{
			//Debug.EnableTracing(@class);

			Title = "Schedule";
		}

		public async void OnNewAppoitment()
		{
			var page = Forge.CreateContentPage(typeof(AppoitmentViewModel));
			await Navigator.PushPage(page);
		}

		//public void OnGoToContacts()
		//{
		//	Debug.Print("ScheduleViewModel.OnGoToContacts");
		//}


		public Color ActiveIconTextColor {
			get => AppScope.ActiveIconTextColor;
		}
	}
}
