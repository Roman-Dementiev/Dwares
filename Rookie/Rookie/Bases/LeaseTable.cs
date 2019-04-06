using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf;


namespace Dwares.Rookie.Bases
{
	public class LeaseTable : AirTable<LeaseRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(LeaseTable));

		public LeaseTable(AirBase airBase) :
			base(airBase, "Lease")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class LeaseRecord : AirRecord
	{
		const string RECIEPT_NO = "Reciept No";
		const string DATE = "Date";
		const string AMOUNT = "Amount";
		const string NOTES = "Notes";

		public int RecieptNo {
			get => GetField<int>(RECIEPT_NO);
			set => SetField(RECIEPT_NO, value);
		}

		public DateOnly Date {
			get => GetField<DateOnly>(DATE);
			set => SetField(DATE, value);
		}

		public decimal Amount {
			get => GetField<decimal>(AMOUNT);
			set => SetField(AMOUNT, value);
		}

		public string Notes {
			get => GetField<string>(NOTES);
			set => SetField(NOTES, value);
		}
	}
}
