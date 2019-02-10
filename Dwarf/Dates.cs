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
}
