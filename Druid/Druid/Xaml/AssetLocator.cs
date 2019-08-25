using System;
using System.Reflection;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;
using Xamarin.Forms;


namespace Dwares.Druid.Xaml
{
	public static class AssetLocator
	{
		//static ClassRef @class = new ClassRef(typeof(AssetLocator));

		static Assembly _defaultAssembly;
		public static Assembly DefaultAssembly {
			set => _defaultAssembly = value;
			get => LazyInitializer.EnsureInitialized(ref _defaultAssembly, () => Application.Current.GetType().Assembly);
		}

		public static bool ResolveName(ref string name, out Assembly assembly)
		{
			assembly = null;
			//@namespace = null;
			if (string.IsNullOrEmpty(name))
				return false;


			var sep = name.IndexOf(':');
			if (sep > 0) {
				var packageName = name.Substring(0, sep);
				var package = CompilationUnit.GetPackage(packageName);
				if (package == null) {
					Debug.Print($"PackageUnit.ResolveName(): unknown package {packageName}");
					return false;
				}

				name = package.Namespace + '.' + name.Substring(sep + 1);
				assembly = package.Assembly;
				return true;
			} else {
				assembly = DefaultAssembly;
				return assembly != null;
			}
		}

		public static ResourceId GetResourceId(string name)
		{
			Assembly assembly;
			if (!ResolveName(ref name, out assembly)) {
				return new ResourceId(assembly, name);
			} else if (DefaultAssembly != null) {
				return new ResourceId(DefaultAssembly, name);
			} else {
				return default;
			}
		}

		public static Type GetTypeByName(string name)
		{
			Assembly assembly;
			if (ResolveName(ref name, out assembly)) {
				var type = assembly.GetTypeByName(name);
				//if (type == null) {
				//	type = assembly.GetTypeByName(name);
				//}
				return type;
			} else {
				return DefaultAssembly?.GetTypeByName(name);
			}
		}

		public static object CreateInstance(string typeName)
		{
			var type = GetTypeByName(typeName);
			if (type != null) {
				try {
					var instance = Activator.CreateInstance(type);
					return instance;
				}
				catch (Exception ex) {
					Debug.ExceptionCaught(ex);
				}
			}
			return null;
		}

	}
}
