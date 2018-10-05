using System;
using System.Collections.Generic;
using System.Reflection;
using Dwares.Dwarf.Collections;


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

		static Dictionary<Assembly, PackageUnit> packages = new Dictionary<Assembly, PackageUnit>();
		static Dictionary<string, NamespaceUnit> namespaces = new Dictionary<string, NamespaceUnit>();
		static Dictionary<Type, ClassUnit> classes = new Dictionary<Type, ClassUnit>();

		public static PackageUnit GetPackage(Assembly assembly, bool create = true)
		{
			var package = packages.GetValue(assembly);
			if (package == null && create) {
				package = new PackageUnit(assembly, null);
				packages.Add(assembly, package);
			}
			return package;
		}

		public static NamespaceUnit GetNamespace(string fullName, bool create = true)
		{
			var @namespace = namespaces.GetValue(fullName);
			if (@namespace == null && create) {
				@namespace = new NamespaceUnit(fullName, null);
				namespaces.Add(fullName, @namespace);
			}
			return @namespace;
		}

		public static ClassUnit GetClass(Type type, bool create = true)
		{
			var @class = classes.GetValue(type);
			if (@class == null && create) {
				var @namespace = GetNamespace(type.Namespace);
				@class = new ClassUnit(type);
				classes.Add(type, @class);
			}
			return @class;
		}

		public static List<Assembly> GetAssemblies()
		{
			var list = new List<Assembly>();
			foreach (var package in packages.Values) {
				list.Add(package.Assembly);
			}
			return list;
		}
	}

	public class PackageUnit : CompilationUnit
	{
		public Assembly Assembly { get; }

		public PackageUnit(Type type, string name = null) :
			this(type.GetTypeInfo().Assembly, name)
		{
		}

		public PackageUnit(Assembly assembly, string name = null)
		{
			Debug.AssertNotNull(assembly);
			Assembly = assembly;
			Name = name ?? assembly.FullName;
		}

		public override string Name { get; }
		public override string FullName => Assembly.FullName;
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
		}

		public override string Name { get; }
		public override string FullName { get; }


	}

	public class ClassUnit : CompilationUnit
	{
		public Type Type { get; }
		public new NamespaceUnit Namespace { get; }

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
		}

		public override string Name => Type.Name;
		public override string FullName => Type.FullName;
	}

}
