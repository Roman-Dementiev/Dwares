using System;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.UWP
{
	public class Package : PackageUnit
	{
		public static readonly Package Instance = new Package();

		Package() : base(typeof(Package)) { }
	}
}
