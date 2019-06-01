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

		public string Id { get; set; }
		public int Seq { get; set; }


		public Client Client { get; }
		public Stop PickupStop { get; set; }
		public Stop DropoffStop { get; set; }


		public class Stop
		{
			public Stop(IPlace place, ScheduleTime? time = null)
			{
				Place = place ?? throw new ArgumentNullException(nameof(place));

				if (time != null) {
					Time = (ScheduleTime)time;
				}
			}

			public IPlace Place { get; set; }
			public ScheduleTime Time { get; set; }
		}

		public static int Compare(Ride ride1, Ride ride2)
		{
			var t1 = (ride1.PickupStop ?? ride1.DropoffStop).Time.Ticks;
			var t2 = (ride2.PickupStop ?? ride2.DropoffStop).Time.Ticks;
			if (t1 == t2) {
				t1 = ride1.Seq;
				t2 = ride2.Seq;
			}

			if (t1 < t2)
				return -1;
			if (t1 > t2)
				return 1;
			return 0;
		}
	}


	public class PickupRide : Ride
	{
		public PickupRide(Client client, IPlace pickupPlace, ScheduleTime? time = null) :
			base(client)
		{
			PickupStop = new Stop(pickupPlace, time);
		}

		public static PickupRide AtHome(Client client, ScheduleTime? time = null)
			=> new PickupRide(client, client.Home, time);

	}


	public class DropoffRide : Ride
	{
		public DropoffRide(Client client, IPlace dropoffPlace, ScheduleTime? time = null) :
			base(client)
		{
			DropoffStop = new Stop(dropoffPlace, time);
		}

		public static DropoffRide ToHome(Client client, ScheduleTime? time = null)
			=> new DropoffRide(client, client.Home, time);
	}


	public class CompleteRide : Ride
	{
		public CompleteRide(Client client, IPlace pickupPlace, ScheduleTime? pickupTime, IPlace dropoffPlace, ScheduleTime? dropoffTime) :
			base(client)
		{
			PickupStop = new Stop(pickupPlace, pickupTime);
			DropoffStop = new Stop(dropoffPlace, dropoffTime);
		}

		public static CompleteRide FromHome(Client client, ScheduleTime? pickupTime, IPlace dropoffPlace, ScheduleTime? dropoffTime)
			=> new CompleteRide(client, client.Home, pickupTime, dropoffPlace, dropoffTime);

		public static CompleteRide ToHome(Client client, IPlace pickupPlace, ScheduleTime? pickupTime, ScheduleTime? dropoffTime)
			=> new CompleteRide(client, pickupPlace, pickupTime, client.Home, dropoffTime);

		public static CompleteRide FromHome(Client client, IPlace dropoffPlace)
			=> FromHome(client, null, dropoffPlace, null);

		public static CompleteRide ToHome(Client client, IPlace pickupPlace)
			=> ToHome(client, null, new ScheduleTime(), null);

	}
}
