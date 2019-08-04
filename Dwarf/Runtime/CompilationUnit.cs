using System;
using System.Collections.Generic;
using System.Reflection;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf.Runtime
{
	public abstract class CompilationUnit
	{
		public CompilationUnit Parent { get; protected set; }
		public abstract string Name { get; }
		public abstract string FullName { get; }

		public override string ToString() => FullName;

		public bool? Tracing { get; set; }
		public bool IsTracing {
			get {
				if (Tracing != null) {
					return (bool)Tracing;
				} else if (Parent != null) {
					return Parent.IsTracing;
				} else {
					return false;
				}
			}
			//set {
			//	Tracing = value;
			//}
		}

		//protected CompilationUnit() { }

		protected static List<PackageUnit> packages = new List<PackageUnit>();
		protected static Dictionary<string, NamespaceUnit> namespaces = new Dictionary<string, NamespaceUnit>();
		protected static Dictionary<Type, ClassUnit> classes = new Dictionary<Type, ClassUnit>();

		public static PackageUnit GetPackage(Assembly assembly, bool create = true)
		{
			foreach (var package in packages) {
				if (package.Assembly == assembly)
					return package;
			}

			if (create) {
				var package = new PackageUnit(assembly, null);
				return package;
			}

			return null;
		}

		public static PackageUnit GetPackage(string name)
		{
			foreach (var package in packages) {
				if (package.Name == name)
					return package;
			}
			return null;
		}

		public static NamespaceUnit GetNamespace(string fullName, bool create = true)
		{
			var @namespace = namespaces.GetValue(fullName);
			if (@namespace == null && create) {
				@namespace = new NamespaceUnit(fullName, null);
			}
			return @namespace;
		}

		public static ClassUnit GetClass(Type type, bool create = true)
		{
			var @class = classes.GetValue(type);
			if (@class == null && create) {
				var @namespace = GetNamespace(type.Namespace);
				@class = new ClassUnit(type);
			}
			return @class;
		}

		public static List<Assembly> GetAssemblies()
		{
			var list = new List<Assembly>();
			foreach (var package in packages) {
				list.Add(package.Assembly);
			}
			return list;
		}

	}

	public class PackageUnit : CompilationUnit
	{
		public Assembly Assembly { get; }

		public PackageUnit(Type type, string name = null) :
			this(type.Assembly, name)
		{
		}

		public PackageUnit(Assembly assembly, string name = null)
		{
			Debug.AssertNotNull(assembly);
			Assembly = assembly;
			Namespace = GetType().Namespace;
			Name = name ?? Namespace;
			packages.Add(this);
		}

		public override string Name { get; }
		public override string FullName {
			get => Assembly.FullName;
		}

		public string Namespace { get; }

		protected bool initialized = false;
		public virtual void Initialize()
		{
			initialized = true;
		}

		public static Type GetTypeByName(string name, Assembly defaultAssembly = null)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			var sep = name.IndexOf(':');
			if (sep > 0) {
				var packageName = name.Substring(0, sep);
				var package = GetPackage(packageName);
				if (package == null) {
					Debug.Print($"PackageUnit.GetTypeByName(): unknown package {packageName}");
					return null;
				}

				name = name.Substring(sep + 1);
				var type = package.Assembly.GetTypeByName(name);
				if (type == null) {
					type = package.Assembly.GetTypeByName(package.Namespace + '.' + name);
				}
				return type;
			} else {
				return defaultAssembly?.GetTypeByName(name);
			}
		}
	}

	public class NamespaceUnit : CompilationUnit
	{
		public NamespaceUnit(String fullName, Assembly assembly)
		{
			Debug.Assert(!String.IsNullOrEmpty(fullName));
			FullName = fullName;

			int point = fullName.IndexOf('.');
			if (point > 0) {
				Name = fullName.Substring(point + 1);
				Parent = GetNamespace(fullName.Substring(0, point));
			} else {
				Name = fullName;

				if (assembly != null) {
					Parent = GetPackage(assembly);
				}
			}
			
			namespaces.Add(fullName, this);
		}

		public override string Name { get; }
		public override string FullName { get; }


	}

	public class ClassUnit : CompilationUnit
	{
		public Type Type { get; }
		public NamespaceUnit Namespace { get; }

		public ClassUnit(Type type, ClassUnit parent = null)
		{
			Debug.Assert(type != null);
			Type = type;
			Namespace = CompilationUnit.GetNamespace(type.Namespace);
			if (parent != null) {
				Parent = parent;
			} else {
				Parent = Namespace;
			}
			classes.Add(type, this);
		}

		public override string Name => Type.Name;
		public override string FullName => Type.FullName;
	}

}
