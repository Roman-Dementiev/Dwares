using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class CurrencyField : PositiveDecimalField
	{
		//static ClassRef @class = new ClassRef(typeof(CurrencyField));

		public CurrencyField(string name) :
			base(name, 2)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
