using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Models
{
	public struct ScheduleTime
	{
		//public ScheduleTime(bool setToday, TimeSpan? span = null)
		//{
		//	if (setToday) {
		//		dt = DateTime.Today;
		//		if (span != null) {
		//			dt = dt.Add((TimeSpan)span);
		//		}
		//	} else {
		//		dt = DateTime.MinValue;
		//	}
		//}

		public ScheduleTime(DateTime time)
		{
			dt = time;
		}

		public ScheduleTime(DateTime afterThis, TimeSpan span)
		{
			dt = afterThis.Add(span);
		}

		public ScheduleTime(ScheduleTime afterThis, TimeSpan span) :
			this(afterThis.DateTime, span)
		{
		}

		public ScheduleTime(TimeSpan span) :
			this(DateTime.Today, span)
		{
		}

		public ScheduleTime(int hour, int minute, int second = 0) :
			this(DateTime.Today, new TimeSpan(hour, minute, second))
		{
		}

		public static ScheduleTime Now => new ScheduleTime(DateTime.Now);
		public static ScheduleTime Today => new ScheduleTime(DateTime.Today);
		public static ScheduleTime Tomorrow => new ScheduleTime(DateTime.Today, new TimeSpan(24, 0, 0));

		//public bool IsSet => dt != DateTime.MinValue;

		DateTime dt;
		public DateTime DateTime {
			get => dt;
			set => dt = value;
		}

		public DateTime Date {
			get => new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
		}

		public TimeSpan Time {
			get => dt.Subtract(Date);
			set => dt = Date.Add(value);
		}
		public DayOfWeek DayOfWeek => dt.DayOfWeek;
		public int Day => dt.Day;
		public int Month => dt.Month;
		public int Year => dt.Year;
		public int Hour => dt.Hour;
		public int Minute => dt.Minute;
		public int Second => dt.Second;

		//public void Set(DateTime time) => dt = time;
		//public void Set(int hour, int minute, int second = 0) => dt = new DateTime(hour, minute, second);
		//public void Unset() => dt = DateTime.MinValue;

		//public void Add(TimeSpan span)
		//{
		//	if (IsSet) {
		//		dt = dt.Add(span);
		//	}
		//}

		public static implicit operator ScheduleTime(DateTime time) => new ScheduleTime(time);
		public static implicit operator DateTime(ScheduleTime time) => time.dt;

		public override string ToString() => ToString("hh:mm tt");
		public string ToString(string format) => dt.ToString(format);
		//public string ToString(string format)
		//{
		//	if (IsSet) {
		//		return dt.ToString(format);
		//	} else {
		//		return String.Empty;
		//	}
		//}

		public bool IsAfter(DateTime time) => DateTime.Compare(dt, time) > 0;
		public bool IsAfter(ScheduleTime time) => IsAfter(time.dt);
		public bool IsBefore(DateTime time) => DateTime.Compare(dt, time) < 0;
		public bool IsBefore(ScheduleTime time) => IsBefore(time.dt);


		//public static ScheduleTime? Add(ScheduleTime? time, TimeSpan? span)
		//{
		//	if (time == null || span == null)
		//		return time;

		//	var result = (ScheduleTime)time;
		//	result.Add((TimeSpan)span);
		//	return result;
		//}
	}
}
