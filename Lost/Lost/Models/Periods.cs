using System;
using Dwares.Dwarf;


namespace Lost.Models
{
	public interface IPeriod
	{
		DateTime StartDate { get; }
		DateTime EndDate { get; }
		string DatesToString();
	}

	public class ShiftPeriod : IPeriod
	{
		DateTime date;

		public ShiftPeriod(DateTime date)
		{
			this.date = date;
		}

		public DateTime StartDate => date;
		public DateTime EndDate => date;
		public string DatesToString() => date.ToShortDateString();
	}
}
