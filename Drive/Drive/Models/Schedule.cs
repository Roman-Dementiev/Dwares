using System;
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
			Route = new Route();
		}

		public Route Route { get; }

		public DateTime? Date { get; set; }

		public new void Add(Ride ride)
		{
			base.Add(ride);

			if (ride.PickupStop != null) {
				Route.Add(ride.PickupStop);
			}
			if (ride.DropoffStop != null) {
				Route.Add(ride.DropoffStop);
			}
		}

		//void RebuildRoute()
		//{
		//	Route.Clear();

		//	foreach (var ride in this) {
		//		if (ride.PickupStop != null) {
		//			Route.Add(ride.PickupStop);
		//		}
		//		if (ride.DropoffStop != null) {
		//			Route.Add(ride.DropoffStop);
		//		}
		//	}
		//}

		//public ObservableCollection<Ride> Rides { get; } = new ObservableCollection<Ride>();

	}
}
