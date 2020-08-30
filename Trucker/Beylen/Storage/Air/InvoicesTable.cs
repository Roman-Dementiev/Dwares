using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
{
	public class InvoicesTable : AirTable<InvoiceRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(InvoicesTable));

		public InvoicesTable(AirBase airBase) :
			base(airBase, "Invoices")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class InvoiceRecord : AirRecord
	{
		public const string SEQ = "#";
		public const string DATE = "Date";
		public const string NUMBER = "Number";
		public const string CAR = "Car";
		public const string CUSTOMER = "Customer";
		public const string NOTES = "Notes";

		public InvoiceRecord() { }

		public int Seq {
			get => GetField<int>(SEQ);
			set => SetField(SEQ, value);
		}

		public DateOnly Date {
			get => GetField<DateOnly>(DATE);
			set => SetField(DATE, value);
		}

		public string Number {
			get => GetField<string>(NUMBER);
			set => SetField(NUMBER, value);
		}

		public string Car {
			get => GetField<string>(CAR);
			set => SetField(CAR, value);
		}
		public string Customer {
			get => GetField<string>(CUSTOMER);
			set => SetField(CUSTOMER, value);
		}

		public string Notes {
			get => GetField<string>(NOTES);
			set => SetField(NOTES, value);
		}
	}
}
