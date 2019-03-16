using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Rookie.Bases;


namespace Dwares.Rookie.ViewModels
{
	public class LeaseViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(LeaseViewModel));

		DateField date;
		PositiveNymbeFrield<int> recieptNo;
		CurrencyField amount;
		TextField notes;

		public LeaseViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Lease";

			date = new DateField("Date") { Value = DateTime.Today };
			recieptNo = new PositiveNymbeFrield<int>("Reciept No");
			amount = new CurrencyField("Amount") { IsRequired = true };
			notes = new TextField("Notes") { Value = AppScope.Instance.GetUnpaidDays() };

			Fields = new FieldList(date, recieptNo, amount, notes);
		}

		public DateOnly Date {
			get => date;
			set => SetProperty(date, value);
		}

		public int ReceptNo {
			get => recieptNo;
			set => SetProperty(recieptNo, value);
		}

		public string RecieptNoText {
			get => recieptNo.Text;
			set => SeTextProperty(recieptNo, value);
		}

		public decimal Amount {
			get => amount;
			set => SetProperty(amount, value);
		}

		public string AmountText {
			get => amount.Text;
			set => SeTextProperty(amount, value);
		}

		public string Notes {
			get => notes;
			set => SetProperty(notes, value);
		}

		protected override Task DoAccept()
		{
			try {
				IsBusy = true;

				var record = new LeaseRecord {
					RecieptNo = this.recieptNo,
					Date = this.date,
					Amount = this.amount,
					Notes = this.notes
				};

				return AppScope.Instance.AddLease(record);
			} finally {
				IsBusy = false;
			}
		}
	}
}
