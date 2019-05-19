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

		public DateTime? Date { get; set; }

		public ObservableCollection<Ride> Rides { get; } = new ObservableCollection<Ride>();
	}	
}
