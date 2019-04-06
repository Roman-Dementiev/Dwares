using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Data;
using Dwares.Dwarf.Toolkit;
using Dwares.Rookie.Airtable;

namespace Dwares.Rookie.Bases
{
	public abstract class Formulas
	{
		public static Formulas Instance { get; set; }

		//static ClassRef @class = new ClassRef(typeof(Formulas));

		public Formulas()
		{
			//Debug.EnableTracing(@class);
		}

		public abstract IQueryFormula PeriodsForDateFormula(string fieldName, DateOnly date);
	}

	public class AirFormulas : Formulas
	{
		public override IQueryFormula PeriodsForDateFormula(string fieldName, DateOnly date)
		{
			string formula = $"AND(YEAR({{{fieldName}}}) = {date.Year}, MONTH({{{fieldName}}}) = {date.Month}, DAY({{{fieldName}}}) = {date.Day}";
			return new AirFormula(formula);
		}
	}
}
