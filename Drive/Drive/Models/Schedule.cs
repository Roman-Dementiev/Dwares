using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Drive.Models
{
	public class Schedule : OrderedCollection<Ride>
	{
		//static ClassRef @class = new ClassRef(typeof(Schedule));

		public Schedule() :
			base(Ride.Compare)
		{
			//Debug.EnableTracing(@class);
		}

		public DateTime? Date { get; set; }

		//public ObservableCollection<Ride> Rides { get; } = new ObservableCollection<Ride>();

	}	
}
