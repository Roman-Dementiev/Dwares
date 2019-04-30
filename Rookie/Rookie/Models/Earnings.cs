using System;


namespace Dwares.Rookie.Models
{
	public struct Earnings
	{
		public Earnings(IWorkPeriod period)
		{
			if (period != null) {
				Income = period.Cash;
				Credit = period.Credit;
				Lease = period.Lease;
				Gas = period.Gas;
				Expenses = period.Expenses;
			}
			else {
				Income  = Credit = Lease = Gas = Expenses = 0;
			}
		}

		public decimal Income { get; set; }
		public decimal Credit { get; set; }
		public decimal Lease { get; set; }
		public decimal Gas { get; set; }
		public decimal Expenses { get; set; }
		public decimal Total => Income + Credit - Lease - Gas - Expenses;
	}
}
