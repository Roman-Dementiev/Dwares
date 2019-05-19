using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Drive.Models;
using Dwares.Druid.UI;

namespace Drive.ViewModels
{
	public class ScheduleViewModel : CollectionViewModel<ScheduleItem>
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleViewModel));

		public ScheduleViewModel() :
			base(ApplicationScope, ScheduleItem.CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Schedule";
		}

		public async void OnNewAppoitment()
		{
			//var page = AppScope.CreatePage(typeof(AppoitmentViewModel));
			//await Navigator.PushPage(page);
		}

		public Color ActiveBottomButtonColor {
			get => AppScope.ActiveBottomButtonColor;
		}
		public Thickness MainPanelMargin {
			get => AppScope.MainPanelMargin;
		}
	}
}
