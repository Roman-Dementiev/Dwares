using System;


namespace Dwares.Dwarf
{
	public static class Dates
	{
		public static bool IsValidYear(int year)
		{
			return year >= DateTime.MinValue.Year || year <= DateTime.MinValue.Year;
		}

		public static DateTime YearFirstDay(int year)
		{
			return IsValidYear(year) ? new DateTime(year, 1, 1) : throw new ArgumentOutOfRangeException(nameof(year));
		}

		public static DateTime YearLastDay(int year)
		{
			return IsValidYear(year) ? new DateTime(year, 12, 31) : throw new ArgumentOutOfRangeException(nameof(year));
		}

		public static DateTime MonthFirstDay(int year, int month)
		{
			if (!IsValidYear(year))
				throw new ArgumentOutOfRangeException(nameof(year));
			if (month <= 0 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));

			return new DateTime(year, month, 1);
		}

		public static DateTime MonthLastDay(int year, int month)
		{
			//if (!IsValidYear(year))
			//	throw new ArgumentOutOfRangeException(nameof(year));

			//switch (month)
			//{
			//case 1: // Jan
			//	case 3: // Marh
			//case 5: // May
			//case 7: // Jul
			//case 8: // Aug
			//case 10: // Oct
			//case 12: // Dec
			//	return new DateTime(year, month, 31);

			//case 4: // Aor
			//case 6: // Jun
			//case 9: // Sep
			//case 11: // Nov
			//	return new DateTime(year, month, 31);

			//case 2: // Feb
			//		return new DateTime(year, month, DateTime.IsLeapYear(year) ? 29 : 28);

			//default:
			//	throw new ArgumentOutOfRangeException(nameof(month));
			//}

			if (!IsValidYear(year))
				throw new ArgumentOutOfRangeException(nameof(year));
			if (month <= 0 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));

			return new DateTime(year, month, DateTime.DaysInMonth(year, month));
		}

		public static DateTime WeekFirstDay(int year, int week)
		{
			//TODO: Use System.Globalization.Calendar.
			return new DateTime();
		}

		public static DateTime WeekLastDay(int year, int week)
		{
			//TODO: Use System.Globalization.Calendar.
			return new DateTime();
		}

	}


	public struct DateOnly : IComparable, IComparable<DateOnly>, IEquatable<DateOnly> /*, IFormattable*/
	{
		public DateOnly(int year, int month, int day)
		{
			DateTime = new DateTime(year, month, day);
		}

		public DateOnly(DateTime datetime) : this(datetime.Year, datetime.Month, datetime.Day) { }

		public DateTime DateTime { get; }
		public int Year => DateTime.Year;
		public int Month => DateTime.Month;
		public int Day => DateTime.Day;
		public DayOfWeek DayOfWeek => DateTime.DayOfWeek;

		public override string ToString() => ToShortDateString();
		//public string ToString(string format, IFormatProvider formatProvider) => DateTime.ToString(format, formatProvider);
		public string ToLongDateString() => DateTime.ToLongDateString();
		public string ToShortDateString() => DateTime.ToShortDateString();

		public static bool TryParse(string s, out DateOnly result)
		{
			if (DateTime.TryParse(s, out var datetime)) {
				result = new DateOnly(datetime);
				return true;
			} else {
				result = default(DateOnly);
				return false;
			}
		}

		public static DateOnly Parse(string s)
		{
			var datetime = DateTime.Parse(s);
			return new DateOnly(datetime);
		}

		public int CompareTo(DateOnly other) => DateTime.CompareTo(other.DateTime);
		public int CompareTo(object other) => DateTime.CompareTo(other);
		public bool Equals(DateOnly other) => DateTime.Equals(other);

		public static implicit operator DateOnly(DateTime datetime) => new DateOnly(datetime);
		public static implicit operator DateTime(DateOnly dateonly) => dateonly.DateTime;

		public static DateOnly ToDateOnly(object source)
		{
			if (source is DateOnly dateonly) {
				return dateonly;
			}
			else if (source is DateTime datetime) {
				return new DateOnly(datetime);
			}
			else if (source is string str) {
				return Parse(str);
			}
			else {
				throw new ArgumentException("Invalid argument in DateOnly constructor", nameof(source));
			}
		}

		public static DateOnly NextDay(int year, int month, int day)
		{
			if (!Dates.IsValidYear(year))
				throw new ArgumentOutOfRangeException(nameof(year));
			if (month <= 0 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));

			int daysInMonth = DateTime.DaysInMonth(year, month);
			if (day <= 0 || day > daysInMonth)
				throw new ArgumentOutOfRangeException(nameof(day));

			if (day < daysInMonth)
				return new DateOnly(year, month, day+1);

			if (month < 12)
				return new DateOnly(year, month+1, 1);

			return new DateOnly(year+1, 1, 1);
		}

		public static DateOnly PreviousDay(int year, int month, int day)
		{
			if (!Dates.IsValidYear(year))
				throw new ArgumentOutOfRangeException(nameof(year));
			if (month <= 0 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));
			if (day <= 0 || day > DateTime.DaysInMonth(year, month))
				throw new ArgumentOutOfRangeException(nameof(day));

			if (day > 1)
				return new DateOnly(year, month, day-1);

			if (month > 1)
				return new DateOnly(year, month-1, DateTime.DaysInMonth(year, month-1));

			return new DateOnly(year-1, 12, DateTime.DaysInMonth(year-1, 12));
		}

		public DateOnly NextDay() => NextDay(Year, Month, Day);
		public DateOnly PreviousDay() => PreviousDay(Year, Month, Day);

		public static DateOnly Today => new DateOnly(DateTime.Today);
		public static DateOnly Yesterday => Today.PreviousDay();
		public static DateOnly Tomorror => Today.NextDay();

	}
}
