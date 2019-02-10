using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Rookie.Data;


namespace Dwares.Rookie.Models
{
	public class TripData : IPeriodic
	{
		//static ClassRef @class = new ClassRef(typeof(TripData));

		protected TripData(int year, int month, TripBase tripBase)
		{
			//Debug.EnableTracing(@class);
			Debug.Assert(year > 0);

			Year = year;
			Month = month;
			TripBase = tripBase;
		}

		public int Year { get; }
		public int Month { get; }
		public int Week => 0;
		public int Day => 0;
		public TripBase TripBase { get; }

	}

	public class MonthlyTripData : TripData
	{
		public MonthlyTripData(int year, int month, TripBase tripBase = null) :
			base(year, month, tripBase)
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
		public YearlyTripData(int year, TripBase tripBase = null) :
			base(year, 0, tripBase)
		{
			MonthlyData =new ObservableCollection<MonthlyTripData>();
			//for (int i = 0; i < 12; i++) {
			//	Monthly.Add(null);
			//}
		}

		//public override string ToString() => Year.ToString();

		public ObservableCollection<MonthlyTripData> MonthlyData { get; }

		public MonthlyTripData AddMonth(int month, TripBase tripBase)
		{
			if (month < 1 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));

			var mothlyData = new MonthlyTripData(Year, month, tripBase);

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
	}
}

