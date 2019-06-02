using System;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;


namespace Drive.Models
{
	public class Ride : TitleHolder
	{
		//static ClassRef @class = new ClassRef(typeof(ScheduleTrip));

		protected Ride(Client client)
		{
			//Debug.EnableTracing(@class);

			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public Client Client { get; }
		public string Id { get; set; }

		int seq;
		public int Seq {
			get => seq;
			set {
				seq = value;
				
				if (PickupStop != null) {
					PickupStop.Seq = 2 * value;
				}
				if (DropoffStop != null) {
					DropoffStop.Seq = 2 * value + 1;
				}
			}
		}


		public RouteStop PickupStop { get; protected set; }
		public RouteStop DropoffStop { get; protected set; }


		public static int Compare(Ride ride1, Ride ride2)
		{
			var stop1 = ride1.PickupStop ?? ride1.DropoffStop;
			var stop2 = ride2.PickupStop ?? ride2.DropoffStop;
			return RouteStop.Compare(stop1, stop2);
		}
	}


	public class PickupRide : Ride
	{
		public PickupRide(Client client, IPlace pickupPlace, ScheduleTime time) :
			base(client)
		{
			PickupStop = new RouteStop(pickupPlace, time);
		}

		public static PickupRide AtHome(Client client, ScheduleTime time)
			=> new PickupRide(client, client.Home, time);
	}


	public class DropoffRide : Ride
	{
		public DropoffRide(Client client, IPlace dropoffPlace, ScheduleTime time) :
			base(client)
		{
			DropoffStop = new RouteStop(dropoffPlace, time);
		}

		public static DropoffRide ToHome(Client client, ScheduleTime time)
			=> new DropoffRide(client, client.Home, time);
	}


	public class CompleteRide : Ride
	{
		public CompleteRide(Client client, IPlace pickupPlace, ScheduleTime pickupTime, IPlace dropoffPlace, ScheduleTime dropoffTime) :
			base(client)
		{
			PickupStop = new RouteStop(pickupPlace, pickupTime);
			DropoffStop = new RouteStop(dropoffPlace, dropoffTime);
		}

		public static CompleteRide FromHome(Client client, ScheduleTime pickupTime, IPlace dropoffPlace, ScheduleTime dropoffTime)
			=> new CompleteRide(client, client.Home, pickupTime, dropoffPlace, dropoffTime);

		public static CompleteRide ToHome(Client client, IPlace pickupPlace, ScheduleTime pickupTime, ScheduleTime dropoffTime)
			=> new CompleteRide(client, pickupPlace, pickupTime, client.Home, dropoffTime);

		//public static CompleteRide FromHome(Client client, IPlace dropoffPlace)
		//	=> FromHome(client, null, dropoffPlace, null);

		//public static CompleteRide ToHome(Client client, IPlace pickupPlace)
		//	=> ToHome(client, null, new ScheduleTime(), null);

	}
}
