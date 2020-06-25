using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Lost.Models
{
	public enum ShiftState
	{
		None,
		Enroute,
		Working,
		Finishing,
		Ended
	}

	public class Shift : PropertyNotifier
	{
		//public Shift() : this(DateOnly.Today) { }

		public Shift(DateTime date)
		{
			this.date = date;
			state = ShiftState.None;
		}

		public DateOnly Date {
			get => date;
			//private set => SetProperty(ref date, value);
		}
		DateOnly date;

		public ShiftState State {
			get => state;
			private set => SetProperty(ref state, value);
		}
		ShiftState state;

		public DateTime? ShiftStartTime {
			get => shiftStartTime;
			private set => SetProperty(ref shiftStartTime, value);
		}
		DateTime? shiftStartTime;

		public DateTime? ShiftEndTime {
			get => shiftEndTime;
			private set => SetProperty(ref shiftEndTime, value);
		}
		DateTime? shiftEndTime;

		public DateTime? FirstPickupTime {
			get => firstPickupTime;
			private set => SetProperty(ref firstPickupTime, value);
		}
		DateTime? firstPickupTime;

		public DateTime? LastDropoffTime {
			get => lastDropoffTime;
			private set => SetProperty(ref lastDropoffTime, value);
		}
		DateTime? lastDropoffTime;

		public int? StartMileage {
			get => startMileage;
			private set => SetProperty(ref startMileage, value);
		}
		int? startMileage;

		public int? EndMileage {
			get => endMileage;
			private set => SetProperty(ref endMileage, value);
		}
		int? endMileage;

		public void StartShift(DateTime time, int milage)
		{
			Debug.Assert(State == ShiftState.None && time.Date == this.Date);
			
			ShiftStartTime = time;
			StartMileage = milage;
			State = ShiftState.Enroute;
		}

		public void EndShift(DateTime time, int milage)
		{
			ShiftEndTime = time;
			EndMileage = milage;
			State = ShiftState.Ended;
		}

		public void FirtPickup(DateTime time)
		{
			FirstPickupTime = time;
			State = ShiftState.Working;
		}

		public void LastDropoff(DateTime time)
		{
			LastDropoffTime = time;
			State = ShiftState.Finishing;
		}
	}
}
