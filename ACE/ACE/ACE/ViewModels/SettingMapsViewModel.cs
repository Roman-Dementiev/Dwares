using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Druid.Forms;
using Dwares.Drums;


namespace ACE.ViewModels
{
	public class SettingMapsViewModel : FormScope
	{
		public SettingMapsViewModel()
		{
			AddDrivingTimeList = MinutesItem.List(0, 20, 5);
			AddDrivingTimeWithTraficList = MinutesItem.List(5, 30, 5);

			ApplicationSelected = Maps.MapApplication;
			ServiceSelected = Maps.MapService;
			AddDrivingTimeListSelected = AddDrivingTimeList[0] as MinutesItem;
			AddDrivingTimeWithTraficSelected = AddDrivingTimeWithTraficList[0] as MinutesItem;
		}


		public IList ApplicationsList => Maps.Applications;
		public IList ServicesList => Maps.Services;
		public IList AddDrivingTimeList { get; }
		public IList AddDrivingTimeWithTraficList { get; }

		public IMapApplication ApplicationSelected { get; set; }
		public IMapService ServiceSelected { get; set; }
		public MinutesItem AddDrivingTimeListSelected { get; set; }
		public MinutesItem AddDrivingTimeWithTraficSelected { get; set; }

		public override async Task OnCancel()
		{
			await base.OnCancel();
		}
	}
}
