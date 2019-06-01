using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


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

		public async Task<RideRecord[]> ListRides()
		{
			var list = await ListRecords();
			return list.Records;
		}
	}

	public class RideRecord : AirRecord
	{
		const string SEQ = "#";
		const string RIDE_TYPE = "Ride type";
		const string CLIENT = "Client";
		const string PICKUP_TIME = "Pickup time";
		const string PICKUP_PLACE = "Pickup place";
		const string DROPOFF_TIME = "Dropoff time";
		const string DROPOFF_PLACE = "Dropoff place";

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

		public string PickupPlaceId {
			get => GetLinkId(PICKUP_PLACE);
		}

		public DateTime DropoffTime {
			get => GetField<DateTime>(DROPOFF_TIME);
			set => SetField(DROPOFF_TIME, value);
		}

		public string DropoffPlaceId {
			get => GetLinkId(DROPOFF_PLACE);
		}
	}
}
