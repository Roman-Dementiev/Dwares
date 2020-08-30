using System;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;


namespace Beylen.Storage.Air
{
	public class ArticlesTable : AirTable<ArticleRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(ArticlesTable));

		public ArticlesTable(AirBase airBase) :
			base(airBase, "Articles")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class ArticleRecord : AirRecord
	{
		public const string SEQ = "#";
		public const string INVOICE_SEQ = "Invoice #";
		public const string PRODUCE = "Produce";
		public const string QUANTITY = "Quantity";
		public const string UNIT = "Unit";
		public const string UNIT_PRICE = "Unit price";
		public const string TOTAL_PRICE = "Total price";
		public const string NOTE = "Note";

		public int Seq {
			get => GetField<int>(SEQ);
			set => SetField(SEQ, value);
		}

		public int InvoiceSeq {
			get => GetField<int>(INVOICE_SEQ);
			set => SetField(INVOICE_SEQ, value);
		}

		public string Produce {
			get => GetField<string>(PRODUCE);
			set => SetField(PRODUCE, value);
		}

		public decimal Quantity {
			get => GetField<decimal>(QUANTITY);
			set => SetField(QUANTITY, value);
		}

		public string Unit {
			get => GetField<string>(UNIT);
			set => SetField(UNIT, value);
		}

		public decimal UnitPrice {
			get => GetField<decimal>(UNIT_PRICE);
			set => SetField(UNIT_PRICE, value);
		}

		public decimal TotalPrice {
			get => GetField<decimal>(TOTAL_PRICE);
			set => SetField(TOTAL_PRICE, value);
		}

		public string Note {
			get => GetField<string>(NOTE);
			set => SetField(NOTE, value);
		}
	}
}
