using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
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

		public int RecieptNo { get; set; }
		public DateOnly Date { get; set; }
		public decimal Amount { get; set; }
		public string Notes { get; set; }

		public override void CopyFieldsToProperties()
		{
			RecieptNo = GetField<int>(RECIEPT_NO);
			Date = GetField<DateOnly>(DATE);
			Amount = GetField<decimal>(AMOUNT);
			Notes = GetField<string>(NOTES);
		}

		public override void CopyPropertiesToFields(IEnumerable<string> fieldNames)
		{
			SetField(RECIEPT_NO, RecieptNo, fieldNames);
			SetField(DATE, Date, fieldNames);
			SetField(AMOUNT, Amount, fieldNames);
			SetField(NOTES, Notes, fieldNames);
		}
	}
}
