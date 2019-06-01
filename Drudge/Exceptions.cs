using System;
using Dwares.Dwarf;


namespace Dwares.Drudge
{
	public class InconsitantData : DwarfException
	{
		public InconsitantData(string message) : base(message) {}

		public InconsitantData(string error, string details) : base(error, details) { }
 
	}
}
