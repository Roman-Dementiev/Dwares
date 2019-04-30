using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Rookie.Models;


namespace Dwares.Rookie.ViewModels
{
	public class EarningsViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(EarningsViewModel));

		protected EarningsViewModel()
		{
			//Debug.EnableTracing(@class);

			//Title = "Earnings";
			UpdateValues();
			AppScope.Instance.PropertyChanged += (sender, e) => {
				if (e.PropertyName == nameof(AppScope.IsWorking)) {
					PropertiesChanged(nameof(IsWorking));
				} else  if (e.PropertyName == nameof(AppScope.Earnings)) {
					UpdateValues();
				}
			};
		}

		public Earnings Earnings { get; private set; }

		public bool IsWorking {
			get => AppScope.Instance.IsWorking;
		}

		protected virtual void UpdateValues()
		{
			Earnings = AppScope.Instance.Earnings;
		}

		protected static string CurrencyString(string sign, decimal value, bool emptyForZero = false)
		{
			if (value == 0 && emptyForZero)
				return string.Empty;

			if (value < 0) {
				sign = "-";
				value = -value;
			}

			return string.Format("{0}{1:C2}", sign ?? string.Empty, value);
		}

		public double SmallFontSize => 20;
		public double LargeFontSize => 36;
	}

	public class EarningsQuickViewModel : EarningsViewModel
	{
		public string Income => CurrencyString(null, Earnings.Income);
		public string Credit => CurrencyString("+", Earnings.Credit, emptyForZero: true);

		public bool IncomeVisible => true; //Earnings.Income > 0;
		public bool CreditVisible => Earnings.Credit > 0;

		public Color IncomeColor => Color.Green;
		public Color CreditColor => Color.Black;


		protected override void UpdateValues()
		{
			base.UpdateValues();
			PropertiesChanged(nameof(Income), nameof(Credit), nameof(IncomeVisible), nameof(CreditVisible));
		}

	}

	public class EarninsDetailViewModel : EarningsViewModel
	{
		public string Income => CurrencyString(null, Earnings.Income);
		public string Credit => CurrencyString("+", Earnings.Credit, emptyForZero: true);
		public string Lease => CurrencyString("-", Earnings.Lease, emptyForZero: true);
		public string Gas => CurrencyString("-", Earnings.Gas, emptyForZero: true);
		public string Expenses => CurrencyString("-", Earnings.Expenses, emptyForZero: true);
		public string Total => CurrencyString(null, Earnings.Total, emptyForZero: false);


		public bool IncomeVisible => true; //Earnings.Income > 0;
		public bool CreditVisible => Earnings.Credit > 0;
		public bool LeaseVisible => Earnings.Lease > 0;
		public bool GasVisible => Earnings.Gas > 0;
		public bool ExpensesVisible => Earnings.Expenses > 0;


		protected override void UpdateValues()
		{
			base.UpdateValues();
			PropertiesChanged(nameof(Income), nameof(Credit));
		}

	}
}
