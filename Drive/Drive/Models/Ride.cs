using System;
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

		protected Ride(Client client, IPlace place, ScheduleTime time) :
			this(client)
		{
			Stops = new Stop[] { new Stop {
				Place = place,
				Time = time
			} };
		}

		//protected Ride(Client client, IPlace place) :
		//	this(client, place, new ScheduleTime())
		//{
		//}


		public Client Client { get; }
		public Stop[] Stops { get; set; }

		public Stop FirstStop {
			get => Stops[0];
		}

		public Stop? SecondStop {
			get {
				if (Stops.Length > 1) {
					return Stops[1];
				} else {
					return null;
				}
			}
		}

		public struct Stop
		{
			public IPlace Place { get; set; }
			public ScheduleTime Time { get; set; }
		}
	}

	public class PickupRide : Ride
	{
		public PickupRide(Client client, IPlace pickupPlace) :
			base(client, pickupPlace, new ScheduleTime())
		{
		}

		public PickupRide(Client client, IPlace pickupPlace, ScheduleTime time) :
			base(client, pickupPlace, time)
		{
		}

		public static PickupRide AtHome(Client client, ScheduleTime time)
			=> new PickupRide(client, client.Home, time);

		public static PickupRide AtHome(Client client)
			=> new PickupRide(client, client.Home, new ScheduleTime());
	}

	public class DropoffRide : Ride
	{
		public DropoffRide(Client client, IPlace pickupPlace) :
			base(client, pickupPlace, new ScheduleTime())
		{
		}

		public DropoffRide(Client client, IPlace pickupPlace, ScheduleTime time) :
			base(client, pickupPlace, time)
		{
		}


		public static DropoffRide ToHome(Client client, ScheduleTime time)
			=> new DropoffRide(client, client.Home, time);

		public static DropoffRide ToHome(Client client)
			=> new DropoffRide(client, client.Home, new ScheduleTime());
	}

	public class CompleteRide : Ride
	{
		public CompleteRide(Client client, IPlace pickupPlace, ScheduleTime pickupTime, IPlace dropoffPlace, ScheduleTime dropoffTime) :
			base(client)
		{
			Stops = new Stop[] { 
				new Stop {
					Place = pickupPlace,
					Time = pickupTime
				},
				new Stop {
					Place = dropoffPlace,
					Time = dropoffTime
				}
			};
		}

		public static CompleteRide FromHome(Client client, ScheduleTime pickupTime, IPlace dropoffPlace, ScheduleTime dropoffTime)
			=> new CompleteRide(client, client.Home, pickupTime, dropoffPlace, dropoffTime);

		public static CompleteRide ToHome(Client client, IPlace pickupPlace, ScheduleTime pickupTime, ScheduleTime dropoffTime)
			=> new CompleteRide(client, pickupPlace, pickupTime, client.Home, dropoffTime);

		public static CompleteRide FromHome(Client client, IPlace dropoffPlace)
			=> FromHome(client, new ScheduleTime(), dropoffPlace, new ScheduleTime());

		public static CompleteRide ToHome(Client client, IPlace pickupPlace)
			=> ToHome(client, pickupPlace, new ScheduleTime(), new ScheduleTime());

	}
}
