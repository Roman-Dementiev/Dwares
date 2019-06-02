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
	}
}
