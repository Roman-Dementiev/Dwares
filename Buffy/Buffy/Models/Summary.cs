﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Buffy.Models
{
	public abstract class SummaryRecord : Record
	{
		//static ClassRef @class = new ClassRef(typeof(Summary));

		protected SummaryRecord()
		{
			//Debug.EnableTracing(@class);
		}

		public abstract string Title { get; }
		public DateOnly StartDate { get; private set; }
		public DateOnly EndDate { get; private set; }

		protected void SetPeriod(DateOnly start, DateOnly end)
		{
			StartDate = start;
			EndDate = end;
			FirePropertiesChanged(nameof(StartDate), nameof(EndDate), nameof(Title));
		}

		public int Count {
			get => count;
			set => SetProperty(ref count, value);
		}
		int count;

		public bool IsEstimated {
			get => isEstimated;
			set => SetProperty(ref isEstimated, value);
		}
		bool isEstimated;

		public string GallonsStr() => GallonsStr(IsEstimated);

		public int Year => StartDate.Year;

		public virtual bool DateInRange(DateOnly date)
		{
			return date >= StartDate && date <= EndDate;
		}

		public virtual void Add(Fueling fueling, decimal estimatedGallons = 0)
		{
			if (!DateInRange(fueling.Date))
				return;

			Count++;
			Total += fueling.Total;

			if (fueling.Gallons > 0) {
				Gallons += fueling.Gallons;
			} else {
				Gallons += estimatedGallons;
				IsEstimated = true;
			}
		}
	}

	public class YearSummary : SummaryRecord
	{
		public YearSummary(int year)
		{
			SetPeriod(new DateOnly(year, 1, 1), new DateOnly(year, 12, 31));
		}

		public override bool DateInRange(DateOnly date) => date.Year == Year;

		public override string Title {
			get => Year.ToString();
		}
	}

	public class MonthSummary : SummaryRecord
	{
		public MonthSummary(int year, int month)
		{
			SetPeriod(new DateOnly(year, month, 1), new DateOnly(year, month, DateTime.DaysInMonth(year, month)));
		}

		public int Month => StartDate.Month;

		public override bool DateInRange(DateOnly date) => date.Year == Year && date.Month == Month;

		public override string Title {
			get => StartDate.DateTime.ToString("Y");
		}

	}

	public class WeekSummary : SummaryRecord
	{
		public WeekSummary(DateOnly date)
		{
			var start = date;
			while (start.DayOfWeek != DayOfWeek.Sunday)
				start = start.PreviousDay();
			
			var end = start.DateTime.AddDays(6);

			if (start.Year < date.Year)
				start = new DateTime(date.Year, 1, 1);

			if (end.Year > date.Year)
				end = new DateTime(date.Year, 12, 31);

			SetPeriod(start, end);
		}

		public override string Title {
			//get  => $"{StartDate.Month}/{StartDate.Day} - {EndDate.Month}/{EndDate.Day}";
			get => $"Week of {EndDate.ToShortDateString()}";
		}
	}


	public class SummaryAddedEventArgs : EventArgs
	{
		public SummaryAddedEventArgs(SummaryRecord summary, bool atEnd)
		{
			Summary = summary;
			AtEnd = atEnd;
		}

		public SummaryRecord Summary { get; }
		public bool AtEnd { get; }
	}

	public class Summary : SummaryRecord
	{
		public Summary()
		{
			var today = DateOnly.Today;
			SetPeriod(today, today);
		}

		public event Action<SummaryAddedEventArgs> SummaryAdded;
		public event Action<EventArgs> SummaryCleared;

		public List<YearSummary> YearSummaries = new List<YearSummary>();
		public List<MonthSummary> MonthSummaries = new List<MonthSummary>();
		public List<WeekSummary> WeekSummaries = new List<WeekSummary>();

		public override string Title {
			get => $"{StartDate.ToShortDateString()}- {EndDate.ToShortDateString()}";
		}
		//public override bool DateInRange(DateOnly date) => true;

		public override void Add(Fueling fueling, decimal estimatedGallons = 0) 
		{
			if (fueling.Date < StartDate)
				SetPeriod(fueling.Date, EndDate);
			if (fueling.Date > EndDate)
				SetPeriod(StartDate, fueling.Date);

			var date = fueling.Date;
			GetYearSummary(date.Year, true).Add(fueling, estimatedGallons);
			GetMonthSummary(date.Year, date.Month, true).Add(fueling, estimatedGallons);
			GetWeekSummary(date, true).Add(fueling, estimatedGallons);
			base.Add(fueling, estimatedGallons);
		}

		void FireSummaryAdded(SummaryRecord summary, bool atEnd)
		{
			if (SummaryAdded != null) {
				SummaryAdded.Invoke(new SummaryAddedEventArgs(summary, atEnd));
			}
		}

		public YearSummary GetYearSummary(int year, bool create)
		{
			for (int i = 0; i < YearSummaries.Count; i++) {
				var sum = YearSummaries[i];
				if (sum.Year == year)
					return sum;

				if (sum.Year > year) {
					if (create) {
						sum = new YearSummary(year);
						YearSummaries.Insert(i, sum);
						FireSummaryAdded(sum, false);
						return sum;
					} else {
						return null;
					}
				}
			}

			if (create) {
				var sum = new YearSummary(year);
				YearSummaries.Add(sum);
				FireSummaryAdded(sum, true);
				return sum;
			} else {
				return null;
			}
		}

		public MonthSummary GetMonthSummary(int year, int month, bool create)
		{
			for (int i = 0; i < MonthSummaries.Count; i++) {
				var sum = MonthSummaries[i];
				if (sum.Year == year && sum.Month == month)
					return sum;

				if (sum.Year < year)
					continue;

				if (sum.Year > year || sum.Month > month) {
					if (create) {
						sum = new MonthSummary(year, month);
						MonthSummaries.Insert(i, sum);
						FireSummaryAdded(sum, false);
						return sum;
					} else {
						return null;
					}
				}
			}

			if (create) {
				var sum = new MonthSummary(year, month);
				MonthSummaries.Add(sum);
				FireSummaryAdded(sum, true);
				return sum;
			} else {
				return null;
			}
		}

		public WeekSummary GetWeekSummary(DateOnly date, bool create)
		{
			var newSum = new WeekSummary(date);

			for (int i = 0; i < WeekSummaries.Count; i++) {
				var sum = WeekSummaries[i];
				if (sum.StartDate == newSum.StartDate)
					return sum;

				if (sum.StartDate > newSum.StartDate) {
					if (create) {
						WeekSummaries.Insert(i, newSum);
						FireSummaryAdded(newSum, false);
						return newSum;
					} else {
						return null;
					}
				}
			}

			if (create) {
				WeekSummaries.Add(newSum);
				FireSummaryAdded(newSum, true);
				return newSum;
			} else {
				return null;
			}
		}

		public void Clear()
		{
			YearSummaries.Clear();
			MonthSummaries.Clear();
			WeekSummaries.Clear();
			SummaryCleared?.Invoke(new EventArgs());
		}
	}
}

