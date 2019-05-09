using System;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;


namespace Drive.Models
{
	public class AppoitmentTrip : ScheduleTrip
	{
		//static ClassRef @class = new ClassRef(typeof(AppoitmentTrip));

		public AppoitmentTrip()
		{
			//Debug.EnableTracing(@class);
		}

		ScheduleTime appoitmentTime;
		public ScheduleTime AppoitmentTime {
			get => appoitmentTime;
			set => SetProperty(ref appoitmentTime, value);
		}

		Facility appoitmentPlace;
		public Facility AppoitmentPlace {
			get => appoitmentPlace;
			set => SetProperty(ref appoitmentPlace, value);
		}
	} 
}
