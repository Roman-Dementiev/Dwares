using System;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid
{
	public class Package: PackageUnit
	{
		//static PackageUnit instance;
		//public static PackageUnit Instance = LazyInitializer.EnsureInitialized(ref instance);
		public static readonly Package Instance = new Package();

		Package() : base(typeof(Package)) { }
	}

}
