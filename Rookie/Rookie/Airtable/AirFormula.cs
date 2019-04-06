using System;
using System.Collections.Generic;
using Dwares.Dwarf.Data;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Rookie.Airtable
{
	public class AirFormula : IValueHolder<string>, IQueryFormula
	{
		public AirFormula(string formula)
		{
			Value = formula;
		}

		public string Value { get; set; }
		public override string ToString() => Value;
		
		public object MakeQueryToken() => new QyeryBuilder { FilterByFormula = Value };
	}
}
