using System;
using System.Reflection;
using Dwares.Dwarf.Runtime;
using Xamarin.Forms;

namespace Dwares.Druid
{
	public class Package: PackageUnit
	{
		//static PackageUnit instance;
		//public static PackageUnit Instance = LazyInitializer.EnsureInitialized(ref instance);
		public static readonly Package Instance = new Package();

		Package() : base(typeof(Package)) { }

		public static void Init()
		{
			Instance.Initialize();
		}

	}
}
