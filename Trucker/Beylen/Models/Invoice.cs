using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	public class Invoice : Model
	{
		//static ClassRef @class = new ClassRef(typeof(Invoice));

		public Invoice()
		{
			//Debug.EnableTracing(@class);

			Articles = new ObservableCollection<Article>();
		}

		public int Ordinal {
			get => ordinal;
			set => SetProperty(ref ordinal, value);
		}
		int ordinal;

		public int Seq {
			get => seq;
			set => SetProperty(ref seq, value);
		}
		int seq;

		public DateOnly Date {
			get => date;
			set => SetProperty(ref date, value);
		}
		DateOnly date;

		public string CarId {
			get => carId;
			set => SetProperty(ref carId, value);
		}
		string carId;

		public string Number {
			get => number;
			set => SetProperty(ref number, value);
		}
		string number;

		public Customer Customer {
			get => customer;
			set => SetProperty(ref customer, value);
		}
		Customer customer;

		public string Notes {
			get => notes;
			set => SetProperty(ref notes, value);
		}
		string notes;

		public ObservableCollection<Article> Articles { get; }

	}
}
