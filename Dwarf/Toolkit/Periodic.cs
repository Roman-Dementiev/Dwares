using System;

namespace Dwares.Dwarf.Toolkit
{
	public enum PeriodicFrequency
	{
		Unknown,
		Yearly,
		Monthly,
		Weekly,
		Daily
	}

	public interface IPeriodic
	{
		int Year { get; }
		int Month { get; }
		int Week { get; }
		int Day { get; }
	}

	public class Periodic : IPeriodic
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public int Week { get; set; }
		public int Day { get; set; }
	}

	public static partial class Extensions
	{
		public static PeriodicFrequency Frequency(this IPeriodic periodic)
		{
			if (periodic == null ||  periodic.Year <= 0)
				return PeriodicFrequency.Unknown;

			if (periodic.Month > 0)
				return PeriodicFrequency.Monthly;

			if (periodic.Week > 0)
				return PeriodicFrequency.Weekly;

			if (periodic.Day > 0)
				return PeriodicFrequency.Daily;

			return PeriodicFrequency.Yearly;
		}

		public static bool IsAnnual(this IPeriodic periodic) => Frequency(periodic) == PeriodicFrequency.Yearly;
		public static bool IsMonthly(this IPeriodic periodic) => Frequency(periodic) == PeriodicFrequency.Monthly;
		public static bool IsWeekly(this IPeriodic periodic) => Frequency(periodic) == PeriodicFrequency.Weekly;
		public static bool IsDaily(this IPeriodic periodic) => Frequency(periodic) == PeriodicFrequency.Daily;

		public static DateTime StartDate(this IPeriodic periodic)
		{
			switch (Frequency(periodic))
			{
			case PeriodicFrequency.Yearly:
				return Dates.YearFirstDay(periodic.Year);

			case PeriodicFrequency.Monthly:
				return Dates.MonthFirstDay(periodic.Year, periodic.Month);

			case PeriodicFrequency.Weekly:
				return Dates.WeekFirstDay(periodic.Year, periodic.Week);

			case PeriodicFrequency.Daily:
				return new DateTime(periodic.Year, periodic.Month, periodic.Day);

			default:
				return new DateTime();
			}
		}

		public static DateTime EndDate(this IPeriodic periodic)
		{
			switch (Frequency(periodic))
			{
			case PeriodicFrequency.Yearly:
				return Dates.YearLastDay(periodic.Year);

			case PeriodicFrequency.Monthly:
				return Dates.MonthLastDay(periodic.Year, periodic.Month);


			case PeriodicFrequency.Weekly:
				return Dates.WeekLastDay(periodic.Year, periodic.Week);

			case PeriodicFrequency.Daily:
				return new DateTime(periodic.Year, periodic.Month, periodic.Day);

			default:
				return new DateTime();
			}
		}

		public static string ToString(this IPeriodic periodic, string format = null, bool numericFormat = false, bool longDefault = false)
		{
			switch (Frequency(periodic))
			{
			case PeriodicFrequency.Yearly:
				if (string.IsNullOrEmpty(format)) { 
					return periodic.Year.ToString();
				}
				break;

			case PeriodicFrequency.Monthly:
				if (string.IsNullOrEmpty(format)) {
					format = longDefault ? "MMMM" : "MMM";
					numericFormat = false;
				}
				break;

			case PeriodicFrequency.Weekly:
				if (string.IsNullOrEmpty(format)) {
					format = longDefault ? "Week {2}" : "{2}";
					numericFormat = true;
				}
				break;

			case PeriodicFrequency.Daily:
				if (string.IsNullOrEmpty(format)) {
					return  periodic.Day.ToString();
				}
				break;

			default:
				return string.Empty;
			}

			if (numericFormat) {
				return string.Format(format, periodic.Year, periodic.Month, periodic.Week, periodic.Day);
			} else {
				return StartDate(periodic).ToString(format);
			}
		}
	}
}
