using System;
using System.IO;
using System.Reflection;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf
{
	public struct ResourceId
	{
		public Assembly Assembly { get; set; }
		public string Name { get; set; }

		public ResourceId(Assembly assembly, string name)
		{
			Debug.AssertNotNull(assembly);
			Debug.AssertNotNull(name);

			Assembly = assembly;
			Name = name;
		}

		public ResourceId(Type type, string name)
		{
			Debug.AssertNotNull(type);
			Debug.AssertNotNull(name);

			Assembly = type.GetTypeInfo().Assembly;
			Name = name;
		}

		public ResourceId(PackageUnit package, string name)
		{
			Debug.AssertNotNull(package);
			Debug.AssertNotNull(name);

			Assembly = package.Assembly;
			Name = name;
		}

		public Stream GetStream() => Assembly.GetManifestResourceStream(Name);
	}
}
