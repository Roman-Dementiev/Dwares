using System;


namespace Drive.Models
{
	public struct ScheduleTime
	{
		DateTime dt;

		public ScheduleTime(DateTime datetime)
		{
			dt = datetime;
		}

		public ScheduleTime(DateTime day, TimeSpan time)
		{
			dt = day.Add(time);
		}

		public ScheduleTime(TimeSpan time) : this(DateTime.Today, time) { }

		public bool IsSet {
			get => dt.Ticks > 0;
		}

		public void Unset()
		{
			dt = new DateTime(0);
		}

		public DateTime DateTime {
			get => dt;
			set => dt = value;
		}

		public string ToString(string format)
		{
			if (IsSet) {
				return dt.ToString(format);
			} else {
				return string.Empty;
			}
		}

		public override string ToString()
			=> ToString("hh:mm tt");


		public DateTime Date => dt.Date;
		public TimeSpan Time => dt.TimeOfDay;
		public DayOfWeek DayOfWeek => dt.DayOfWeek;
		public int Day => dt.Day;
		public int Month => dt.Month;
		public int Year => dt.Year;
		public int Hour => dt.Hour;
		public int Minute => dt.Minute;
		public int Second => dt.Second;

		public bool IsAfter(DateTime time) => DateTime.Compare(dt, time) > 0;
		//public bool IsAfter(ScheduleTime time) => IsAfter(time.dt);
		public bool IsBefore(DateTime time) => DateTime.Compare(dt, time) < 0;
		//public bool IsBefore(ScheduleTime time) => IsBefore(time.dt);

		public static implicit operator ScheduleTime(DateTime dt) => new ScheduleTime(dt);
		public static implicit operator DateTime(ScheduleTime st) => st.DateTime;

		public static ScheduleTime Now => new ScheduleTime(DateTime.Now);
		public static ScheduleTime Today => new ScheduleTime(DateTime.Today);
		public static ScheduleTime Tomorrow => new ScheduleTime(DateTime.Today, new TimeSpan(24, 0, 0));
	}
}
