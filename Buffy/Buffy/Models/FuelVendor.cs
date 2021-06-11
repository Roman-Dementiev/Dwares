using Dwares.Dwarf;
using System;


namespace Buffy.Models
{
	public class FuelVendor
	{
		//static ClassRef @class = new ClassRef(typeof(FuelProvider));

		public FuelVendor()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name { get; set; }
		public string Icon { get; set; }
	}
}
