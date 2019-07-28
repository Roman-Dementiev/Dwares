using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Drive.Models;
using Drive.Views;


namespace Drive.ViewModels
{
	public class ScheduleViewModel : CollectionViewModel<ScheduleCardViewModel>, IRootContentViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleViewModel));

		public ScheduleViewModel() :
			base(ApplicationScope, ScheduleCardViewModel.CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Schedule";
		}

		public Type ContentViewType()
		{
			return typeof(ScheduleView);
		}

		public Type ControlsViewType(bool landscape)
		{
			return null;
		}
	}
}
