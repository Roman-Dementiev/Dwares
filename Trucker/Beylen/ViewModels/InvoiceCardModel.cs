using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Beylen.Models;
using System.Collections.Generic;

namespace Beylen.ViewModels
{
	public class InvoiceCardModel : CardViewModel<Invoice>
	{
		//static ClassRef @class = new ClassRef(typeof(InvoiceCardModel));

		public InvoiceCardModel(Invoice source) :
			base(source)
		{
			//Debug.EnableTracing(@class);

			UpdateFromSource();
		}

		//public int Seq {
		//	get => seq;
		//	set => SetProperty(ref seq, value);
		//}
		//int seq;

		public int Ordinal {
			get => ordinal;
			set => SetPropertyEx(ref ordinal, value, nameof(Ordinal), nameof(OrdString));
		}
		int ordinal;

		public string OrdString {
			get => $"{Ordinal}.";
		}

		public DateOnly Date {
			get => date;
			set => SetProperty(ref date, value);
		}
		DateOnly date;

		public string Number {
			get => number;
			set => SetProperty(ref number, value);
		}
		string number;

		public Customer Customer {
			get => customer;
			set => SetPropertyEx(ref customer, value, nameof(Customer), nameof(CustomerName));
		}
		Customer customer;

		public string CustomerName {
			get => Customer.DisplayName;
		}

		public string Notes {
			get => notes;
			set => SetProperty(ref notes, value);
		}
		string notes;

		protected override void UpdateFromSource()
		{
			//Seq = Source.Seq;
			Ordinal = Source.Ordinal;
			Date = Source.Date;
			Number = Source.Number;
			Customer = Source.Customer;
			Notes = Source.Notes;
		}


	}
}
