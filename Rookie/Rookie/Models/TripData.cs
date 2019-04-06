using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Rookie.Bases;


namespace Dwares.Rookie.Models
{
	public interface ITripDataSource
	{

	}

	public class TripData : IPeriodic
	{
		//static ClassRef @class = new ClassRef(typeof(TripData));

		protected TripData(int year, int month, ITripDataSource source)
		{
			//Debug.EnableTracing(@class);
			Debug.Assert(year > 0);

			Year = year;
			Month = month;
			Source = source;
		}

		public int Year { get; }
		public int Month { get; }
		public int Week => 0;
		public int Day => 0;
		public ITripDataSource Source { get; }

	}

	public class MonthlyTripData : TripData
	{
		public MonthlyTripData(int year, int month, ITripDataSource source = null) :
			base(year, month, source)
		{
			Debug.Assert(month > 0 && month <= 12);
		}

		public string ShortString {
			get => this.ToString(longDefault: false);
		}
		public string LongString {
			get => this.ToString(longDefault: true);
		}
	}

	public class YearlyTripData : TripData
	{
		public YearlyTripData(int year, ITripDataSource source = null) :
			base(year, 0, source)
		{
			MonthlyData =new ObservableCollection<MonthlyTripData>();
			//for (int i = 0; i < 12; i++) {
			//	Monthly.Add(null);
			//}
		}

		//public override string ToString() => Year.ToString();

		public ObservableCollection<MonthlyTripData> MonthlyData { get; }

		public MonthlyTripData AddMonth(int month, ITripDataSource source)
		{
			if (month < 1 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));

			var mothlyData = new MonthlyTripData(Year, month, source);

			for (int i = 0; i < MonthlyData.Count; i++) {
				if (MonthlyData[i].Month == month) {
					MonthlyData[i] = mothlyData;
					return mothlyData;
				}
			}

			MonthlyData.Add(mothlyData);
			return mothlyData;
		}


		public MonthlyTripData GetMonth(int month)
		{
			if (month < 1 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));

			foreach (var data in MonthlyData) {
				if (data.Month == month)
					return data;
			}
			return null;
		}

		public int GetMonthIndex(int month)
		{
			if (month < 1 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));

			for (int i = 1; i <= 12; i++) {
				if (MonthlyData[i].Month == month)
					return i;
			}
			return -1;
		}
	}
}

