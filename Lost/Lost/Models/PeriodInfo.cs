using System;
using Dwares.Dwarf;


namespace Lost.Models
{
	public struct PeriodInfo
	{
		public static bool UseFullTime { get; set; } = true;

		public IPeriod Period { get; set; }
		public int WorkTime { get; set; }
		public int FullTime { get; set; }
		public int Mileage { get; set; }

		public string DatesString {
			get => Period.DatesToString();
		}

		public string TimeString {
			get {
				int time = UseFullTime ? FullTime : WorkTime;
				if (time > 0) {
					int hours = time / 60;
					int minutes = time % 60;
					return string.Format("{0} h {1} min", hours, minutes);
				} else {
					return string.Empty;
				}
			}
		}

		public string MilageString {
			get {
				if (Mileage > 0) {
					return string.Format("{0} mi", Mileage);
				} else {
					return string.Empty;
				}
			}
		}
	}
}
