using System;
using System.Threading.Tasks;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf;


namespace Drive.Storage.Air
{
	public class RidesTable : AirTable<RideRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(RidesTable));

		public RidesTable(AirBase airBase) :
			base(airBase, "Rides")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class RideRecord : AirRecord
	{
		const string SEQ = "#";
		const string RIDE_TYPE = "Ride type";
		const string CLIENT = "Client";
		const string PICKUP_TIME = "Pickup time";
		const string PICKUP_ARRIVAL = "Pickup arrival";
		const string PICKUP_PLACE = "Pickup place";
		const string PICKUP_ADDRESS = "Pickup address";
		const string DROPOFF_TIME = "Dropoff time";
		const string DROPOFF_ARRIVAL = "Dropoff arrival";
		const string DROPOFF_PLACE = "Dropoff place";
		const string DROPOFF_ADDRESS = "Dropoff address";
		const string IS_APPOITMENT = "Appoitment?";

		public RideRecord() { }

		public int Seq {
			get => GetField<int>(SEQ);
		}

		public string RideType {
			get => GetField<string>(RIDE_TYPE);
			set => SetField(RIDE_TYPE, value);
		}

		public string ClientId {
			get => GetLinkId(CLIENT);
		}

		public DateTime PickupTime {
			get => GetField<DateTime>(PICKUP_TIME);
			set => SetField(PICKUP_TIME, value);
		}

		public DateTime PickupArrival {
			get => GetField<DateTime>(PICKUP_ARRIVAL);
			set => SetField(PICKUP_ARRIVAL, value);
		}

		public string PickupPlaceId {
			get => GetLinkId(PICKUP_PLACE);
		}

		public string PickupAddress {
			get => GetField<string>(PICKUP_ADDRESS);
			set => SetField(PICKUP_ADDRESS, value);
		}

		public DateTime DropoffTime {
			get => GetField<DateTime>(DROPOFF_TIME);
			set => SetField(DROPOFF_TIME, value);
		}

		public DateTime DropoffArrival {
			get => GetField<DateTime>(DROPOFF_ARRIVAL);
			set => SetField(DROPOFF_ARRIVAL, value);
		}

		public string DropoffPlaceId {
			get => GetLinkId(DROPOFF_PLACE);
		}

		public string DropoffAddress {
			get => GetField<string>(DROPOFF_ADDRESS);
			set => SetField(DROPOFF_ADDRESS, value);
		}

		public bool IsAppotment {
			get => GetField<bool>(IS_APPOITMENT);
			set => SetField(IS_APPOITMENT, value);
		}
	}

	public static class RideType
	{
		public const string RideFromHome = "From Home";
		public const string RideToHome = "To Home";
		public const string PickupAtHome = "Pickup at Home";
		public const string DropoffAtHome = "Dropoff at Home";
		public const string Pickup = "Pickup";
		public const string Dropoff = "Dropoff";
	}
}
