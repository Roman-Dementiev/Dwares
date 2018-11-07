using System;
using System.Threading.Tasks;
using Dwares.Druid.Forms;
using ACE.Models;


namespace ACE.ViewModels
{
	public class NewScheduleViewModel : FormScope
	{
		public NewScheduleViewModel()
		{
			var now = DateTime.Now;
			if (now.Hour < 12) {
				Date = DateTime.Today;
				Time = new TimeSpan(now.Hour+1, 0, 0);
			} else {
				Date = DateTime.Today.AddDays(1);
				Time = new TimeSpan(7, 0, 0);
			}
		}

		DateField date = new DateField();
		public DateTime Date {
			get => date;
			set => SetProperty(date, value);
		}

		readonly TimeField time = new TimeField();
		public TimeSpan Time {
			get => time;
			set => SetProperty(time, value);
		}

		protected override Task DoAccept()
		{
			var route = AppData.Route;
			route.NewRoute(Date.Add(Time));

			return null;
		}
	}
}
