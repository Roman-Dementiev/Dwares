using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;


namespace Drive.Models
{
	public class ScheduleTrip : TitleHolder
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleTrip));

		public ScheduleTrip()
		{
			//Debug.EnableTracing(@class);
		}

		Client client;
		public Client Client {
			get => client;
			set => SetProperty(ref client, value);
		}

		ScheduleTime pickupTime;
		public ScheduleTime PickupTime {
			get => pickupTime;
			set => SetProperty(ref pickupTime, value);
		}
	}
}
