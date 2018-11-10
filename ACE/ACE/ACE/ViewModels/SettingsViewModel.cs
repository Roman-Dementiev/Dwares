using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Druid.Forms;
using Dwares.Druid.Services;
using Dwares.Drums;
using Dwares.Dwarf;


namespace ACE.ViewModels
{
	public class SettingsViewModel: FormScope
	{
		public SettingsViewModel()
		{
			Title = "Settings";

			AddDrivingTimeList = MinutesItem.List(0, 30, 5);
			AddDrivingTimeWithTraficList = MinutesItem.List(0, 30, 5);
			DefaultStopTimeList = MinutesItem.List(5, 30, 5);
			WheelchairStopTimeList = MinutesItem.List(5, 30, 5);

			MapApplicationSelected = Settings.MapApplication;
			MapServiceSelected = Settings.MapService;
			AddDrivingTimeSelected = MinutesItem.Choose(AddDrivingTimeList, Settings.AddDrivingTime);
			AddDrivingTimeWithTraficSelected = MinutesItem.Choose(AddDrivingTimeWithTraficList, Settings.AddDrivingTimeWithTrafic);
			DefaultStopTimeSelected = MinutesItem.Choose(DefaultStopTimeList, Settings.DefaultStopTime);
			WheelchairStopTimeSelected = MinutesItem.Choose(WheelchairStopTimeList, Settings.WheelchairStopTime);
		}

		public IList MapApplicationsList => Maps.Applications;
		public IList MapServicesList => Maps.Services;
		public List<MinutesItem> AddDrivingTimeList { get; }
		public List<MinutesItem> AddDrivingTimeWithTraficList { get; }
		public List<MinutesItem> DefaultStopTimeList { get; }
		public List<MinutesItem> WheelchairStopTimeList { get; }

		public IMapApplication MapApplicationSelected { get; set; }
		public IMapService MapServiceSelected { get; set; }
		public MinutesItem AddDrivingTimeSelected { get; set; }
		public MinutesItem AddDrivingTimeWithTraficSelected { get; set; }
		public MinutesItem DefaultStopTimeSelected { get; set; }
		public MinutesItem WheelchairStopTimeSelected { get; set; }

		protected override Task DoAccept()
		{
			Settings.MapApplication = MapApplicationSelected;
			Settings.MapService = MapServiceSelected;

			Settings.AddDrivingTime = AddDrivingTimeSelected.Minutes;
			Settings.AddDrivingTimeWithTrafic = AddDrivingTimeWithTraficSelected.Minutes;
			Settings.DefaultStopTime = DefaultStopTimeSelected.Minutes;
			Settings.WheelchairStopTime = WheelchairStopTimeSelected.Minutes;
			AppData.Route.UpdateEstimations();

			return null;
		}

	}

	public class MinutesItem
	{
		public MinutesItem(int minutes)
		{
			Minutes = minutes;
		}

		public int Minutes { get; }

		public override string ToString()
		{
			if (Minutes > 0) {
				return String.Format("{0} min", Minutes);
			}
			else {
				return "None";
			}
		}

		public static List<MinutesItem> List(int min, int max, int step)
		{
			var list = new List<MinutesItem>();
			for (int minutes = min; minutes <= max; minutes += step) {
				list.Add(new MinutesItem(minutes));
			}
			return list;
		}

		public static MinutesItem Choose(List<MinutesItem> list, int mins)
		{
			if (list == null || list.Count == 0)
				return default(MinutesItem);

			foreach (var item in list) {
				if (item.Minutes == mins)
					return item;
			}
			
			return list[0];
		}
	}
}
