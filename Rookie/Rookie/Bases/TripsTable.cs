
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;
using Dwares.Dwarf.Data;


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

		public int TripNumber {
			get => GetField<int>(TRIP_NUMBER);
			set => SetField(TRIP_NUMBER, value);
		}

		public DateTime Started {
			get => GetField<DateTime>(STARTED);
			set => SetField(STARTED, value);
		}

		public DateTime Finished {
			get => GetField<DateTime>(FINISHED);
			set => SetField(FINISHED, value);
		}

		public decimal Distance {
			get => GetField<decimal>(DISTANCE);
			set => SetField(DISTANCE, value);
		}

		public decimal Meter {
			get => GetField<decimal>(METER);
			set => SetField(METER, value);
		}

		public decimal Cash {
			get => GetField<decimal>(CASH);
			set => SetField(CASH, value);
		}

		public decimal Expences {
			get => GetField<decimal>(EXPENCES);
			set => SetField(EXPENCES, value);
		}

		public bool IsCredit {
			get => GetField<bool>(IS_CREDIT);
			set => SetField(IS_CREDIT, value);
		}

		public decimal Credit { 
			get => IsCredit ? Meter * 0.9m : 0m;
		}

	}
}
