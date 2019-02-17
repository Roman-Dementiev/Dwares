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
		int TripNumber { get; set; }

		public override void CopyFieldsToProperties()
		{
			TripNumber = GetField<int>("");
		}

		public override void CopyPropertiesToFields()
		{
			Fields["Trip #"] = TripNumber;
		}
	}
}
