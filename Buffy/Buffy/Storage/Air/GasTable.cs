using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;


namespace Buffy.Storage.Air
{
	public class GasTable : AirTable<GasRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(GasTable));

		public GasTable(AirBase dataBase) :
			base(dataBase, "Gas")
		{
			//Debug.EnableTracing(@class);
		}
	}


	public class GasRecord : AirRecord
	{
		public const string DATE = "Date";
		public const string VENDOR = "Vendor";
		public const string STATE = "State";
		public const string GALLONS = "Gallons";
		public const string PRICE = "Price";
		public const string TOTAL = "Total";

		public DateOnly Date {
			get => GetField<DateOnly>(DATE);
			set => SetField(DATE, value);
		}

		public string Vendor {
			get => GetField<string>(VENDOR);
			set => SetField(VENDOR, value);
		}

		public string State {
			get => GetField<string>(STATE);
			set => SetField(STATE, value);
		}

		public decimal Gallons {
			get => GetField<decimal>(GALLONS);
			set => SetField(GALLONS, value);
		}

		public decimal Price {
			get => GetField<decimal>(PRICE);
			set => SetField(PRICE, value);
		}

		public decimal Total {
			get => GetField<decimal>(TOTAL);
			set => SetField(TOTAL, value);
		}

	}
}
