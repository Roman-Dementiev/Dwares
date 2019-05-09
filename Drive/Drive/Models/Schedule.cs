using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;


namespace Drive.Models
{
	public class Schedule
	{
		//static ClassRef @class = new ClassRef(typeof(Schedule));

		public Schedule()
		{
			//Debug.EnableTracing(@class);
		}

		public ObservableCollection<ScheduleTrip> Trips { get; } = new ObservableCollection<ScheduleTrip>();
	}
}
