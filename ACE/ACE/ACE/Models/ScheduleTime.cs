using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Models
{
	public struct ScheduleTime
	{
		public ScheduleTime(DateTime time)
		{
			dt = time;
		}

		public ScheduleTime(TimeSpan span)
		{
			dt = DateTime.Today.Add(span);
		}

		DateTime dt;
		public DateTime DateTime {
			get => dt;
			set => this.dt = value;
		}
		public TimeSpan TimeSpan {
			get => dt.Subtract(DateTime.Today);
			set => dt = DateTime.Today.Add(value);
		}
		public int Hour => dt.Hour;
		public int Minute => dt.Minute;
		public int Second => dt.Second;

		public ScheduleTime(int hours, int minutes, int seconds = 0)
		{
			var today = DateTime.Today;
			var span = new TimeSpan(hours, minutes, seconds);
			dt = today.Add(span);
		}

		public static implicit operator ScheduleTime(DateTime time) => new ScheduleTime(time);
		public static implicit operator DateTime(ScheduleTime time) => time.dt;

		public static explicit operator ScheduleTime(TimeSpan time) => new ScheduleTime(time);
		public static explicit operator TimeSpan(ScheduleTime time) => time.TimeSpan;

		public override string ToString() => dt.ToString("hh:mm tt");
		public string ToString(string format) => dt.ToString(format);

		public bool IsAfter(DateTime time) => DateTime.Compare(dt, time) > 0;
	}
}
