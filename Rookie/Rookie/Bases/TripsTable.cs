using System;
using System.Collections.Generic;
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
		public decimal Credit { 
			get => IsCredit ? Meter * 0.9m : 0m;
		}


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

		public override void CopyPropertiesToFields(IEnumerable<string> fieldNames)
		{
			SetField(TRIP_NUMBER, TripNumber, fieldNames);
			SetField(STARTED, Started, fieldNames);
			SetField(FINISHED, Finished, fieldNames);
			SetField(DISTANCE, Distance, fieldNames);
			SetField(METER, Meter, fieldNames);
			SetField(CASH, Cash, fieldNames);
			SetField(EXPENCES, Expences, fieldNames);
			SetField(IS_CREDIT, IsCredit, fieldNames);
		}
	}
}
