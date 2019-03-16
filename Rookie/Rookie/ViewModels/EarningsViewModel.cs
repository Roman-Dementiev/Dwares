using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;


namespace Dwares.Rookie.ViewModels
{
	public class EarningsViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(EarningsViewModel));

		decimal total;
		decimal income;
		decimal credit;
		decimal expenses;

		public EarningsViewModel()
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

		public bool IsWorking {
			get => AppScope.Instance.IsWorking;
		}

		void UpdateValues()
		{
			var earnings = AppScope.Instance.Earnings;
			total = earnings.Total;
			income = earnings.Income;
			credit = earnings.Credit;
			expenses = earnings.Expenses;

			PropertiesChanged(nameof(Total), nameof(Income), nameof(Credit), nameof(Expenses),
				nameof(IncomeVisible), nameof(CreditVisible), nameof(ExpencesVisible));
		}

		static string CurrencyString(string sign, decimal value, bool emptyForZero = true)
		{
			if (value == 0 && emptyForZero)
				return string.Empty;

			if (value < 0) {
				sign = "-";
				value = -value;
			}

			return string.Format("{0}{1:C2}", sign ?? string.Empty, value);
		}

		public string Total => CurrencyString(null, total, false);
		public string Income => CurrencyString("+", income);
		public string Credit => CurrencyString("...", credit);
		public string Expenses => CurrencyString(" -", expenses);
	
		public bool IncomeVisible => income > 0;
		public bool CreditVisible => credit > 0;
		public bool ExpencesVisible => expenses > 0;

		public Color TotalColor => total >= 0 ? Color.Blue : Color.Red;
		public Color IncomeColor => Color.Green;
		public Color CreditColor => Color.Black;
		public Color ExpensesColor => Color.Red;

		public double SmallFontSize => 14;
		public double LargeFontSize => 32;
	}
}
