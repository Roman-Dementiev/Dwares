using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


namespace Dwares.Rookie.Bases
{
	public class TripsTable : AirTable<TripRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(TripsTable));

		public TripsTable(AirBase airBase) :
			base(airBase, "Trips")
		{
			//Debug.EnableTracing(@class);
		}
	}


	public class TripRecord : AirRecord
	{
		const string TRIP_NUMBER = "Trip#";
		const string STARTED = "Started";
		const string FINISHED = "Finished";
		const string DISTANCE = "Distance";
		const string METER = "Meter";
		const string CASH = "Cash";
		const string EXPENCES = "Expences";
		const string IS_CREDIT = "IsCredit?";

		public int TripNumber { get; set; }
		public DateTime Started { get; set; }
		public DateTime Finished { get; set; }
		public decimal Distance { get; set; }
		public decimal Meter { get; set; }
		public decimal Cash { get; set; }
		public decimal Expences { get; set; }
		public bool IsCredit { get; set; }


		public override void CopyFieldsToProperties()
		{
			TripNumber = GetField<int>(TRIP_NUMBER);
			Started = GetField<DateTime>(STARTED);
			Finished = GetField<DateTime>(FINISHED);
			Distance = GetField<decimal>(DISTANCE);
			Meter = GetField<decimal>(METER);
			Cash = GetField<decimal>(CASH);
			Expences = GetField<decimal>(EXPENCES);
			IsCredit = GetField<bool>(IS_CREDIT);
		}

		public override void CopyPropertiesToFields()
		{
			Fields[TRIP_NUMBER] = TripNumber;
			Fields[STARTED] = Started;
			Fields[FINISHED] = Finished;
			Fields[DISTANCE] = Distance;
			Fields[METER] = Meter;
			Fields[CASH] = Cash;
			Fields[EXPENCES] = Expences;
			Fields[IS_CREDIT] = IsCredit;
		}
	}
}
