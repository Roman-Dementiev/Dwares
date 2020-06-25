using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using Lost.Models;

namespace Lost.Storage.Air
{
	public class DailyTable : AirTable<ShiftRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(DailyTable));

		public DailyTable(AirBase airBase) :
			base(airBase, "Daily")
		{
			//Debug.EnableTracing(@class);
		}

	}

	public class ShiftRecord : AirRecord
	{
		public const string DATE = "Date";
		public const string SHIFT_STATE = "Shift State";
		public const string SHIFT_START = "Shift Start";
		public const string FIRST_PICKUP = "First Pickup";
		public const string LAST_DROPOFF = "Last Dropoff";
		public const string SHIFT_END = "Shift End";
		public const string START_MILEAGE = "Start Mileage";
		public const string END_MILEAGE = "End Mileage";

		public DateTime Date {
			get => GetField<DateTime>(DATE);
			set => SetField(DATE, value);
		}

		public ShiftState ShiftState {
			get {
				var s = GetField<string>(SHIFT_STATE);
				switch (s)
				{
				default:
					return ShiftState.None;
				case "Enroute":
					return ShiftState.Enroute;
				case "Working":
					return ShiftState.Working;
				case "Finishing":
					return ShiftState.Finishing;
				case "Ended":
					return ShiftState.Ended;
				}
			}
			set {
				string s;
				if (value == ShiftState.None) {
					s = "";
				} else {
					s = value.ToString();
				}
				SetField(SHIFT_STATE, s);
			}
		}

		public DateTime ShiftStart {
			get => GetField<DateTime>(SHIFT_START);
			set => SetField(SHIFT_START, value);
		}

		public DateTime FirstPickup {
			get => GetField<DateTime>(FIRST_PICKUP);
			set => SetField(FIRST_PICKUP, value);
		}

		public DateTime LastDropoff {
			get => GetField<DateTime>(LAST_DROPOFF);
			set => SetField(LAST_DROPOFF, value);
		}

		public DateTime ShiftEnd {
			get => GetField<DateTime>(SHIFT_END);
			set => SetField(SHIFT_END, value);
		}

		public int StartMileage {
			get => GetField<int>(START_MILEAGE);
			set => SetField(START_MILEAGE, value);
		}

		public int EndMileage {
			get => GetField<int>(END_MILEAGE);
			set => SetField(END_MILEAGE, value);
		}
	}
}
