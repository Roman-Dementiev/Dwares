using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


namespace Dwares.Rookie.Data
{
	public class SummaryBase : AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(SummaryBase));

		public SummaryBase(string apiKey, string baseId) :
			base(apiKey, baseId)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
