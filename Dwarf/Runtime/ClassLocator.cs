using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;


namespace Dwares.Dwarf.Runtime
{
	public interface IClassLocator
	{
		Type TargetType { get; }

		Type LocateByReference(object reference);
		Type LocateByRefType(Type refType);
	}

	public class ClassLocator
	{
		static ClassLocator instance;
		public static ClassLocator Instance => LazyInitializer.EnsureInitialized(ref instance);

		/*public*/
		List<IClassLocator> Locators { get; } = new List<IClassLocator>();

		public void Add(IClassLocator locator)
		{
			Locators.Add(locator ?? throw new ArgumentNullException(nameof(locator)));
		}

		public object LocateByReference(Type targetType, object reference, Func<Type, object> func)
		{
			foreach (var locator in Locators) {
				if (locator.TargetType != targetType)
					continue;

				var type = locator.LocateByReference(reference);
				if (type == null)
					continue;

				var obj = func(type);
				if (obj != null)
					return obj;
			}

			return null;
		}

		public object LocateByRefType(Type targetType, Type refType, Func<Type, object> func)
		{
			foreach (var locator in Locators) {
				if (locator.TargetType != targetType)
					continue;

				var type = locator.LocateByRefType(refType);
				if (type == null)
					continue;

				var obj = func(type);
				if (obj != null)
					return obj;
			}

			return null;
		}


		public object CreateForReference(Type targetType, object reference)
			=> LocateByReference(targetType, reference, Construct);

		public object CreateForRefType(Type targetType, Type refType)
			=> LocateByRefType(targetType, refType, Construct);

		public static object Construct(Type type)
		{
			var ctor = Reflection.GetDefaultConstructor(type);
			if (ctor != null) {
				return ctor.Invoke(Reflection.cNoArgs);
			}
			else {
				return null;
			}
		}

		public static void AddLocator(IClassLocator locator)
			=> Instance.Add(locator);

		public static TargetType Create<TargetType>(object typeOrReference)
			=> (TargetType)Create(typeof(TargetType), typeOrReference);

		public static object Create(Type targetType, object typeOrReference)
		{
			if (typeOrReference is Type refType) {
				return Instance.CreateForRefType(targetType, refType);
			} else {
				return Instance.CreateForReference(targetType, typeOrReference);
			}
		}
	}

	public class DefaultClassLocator<Target> : IClassLocator
	{
		public Type TargetType => typeof(Target);

		public string ReferenceClassNameSuffix { get; set; }
		public string ReferenceNamespaceSuffix { get; set; }
		public string TargetClassNameSuffix { get; set; }
		public string TargetNamespaceSuffix { get; set; }

		public virtual Type LocateByReference(object reference)
		{
			if (reference == null)
				return null;

			return LocateByRefType(reference.GetType());
		}

		public virtual Type LocateByRefType(Type refType)
		{
			if (!Resolve(refType, out string _className, out string _namespace))
				return null;

			var name = _namespace + '.' + _className;
			var type = Type.GetType(name);
			if (type == null) {
				type = refType.Assembly().GetType(name);
			}
			if (type == null) {
				type = TargetType.Assembly().GetType(name);
			}
			return type;
		}

		public bool Resolve(Type refType, out string _className, out string _namespace)
		{
			if (refType == null) {
				_className = null;
				_namespace = null;
				return false;
			}

			var typeInfo = refType.GetTypeInfo();
			_className = typeInfo.Name;
			_namespace = typeInfo.Namespace;

			if (ReferenceClassNameSuffix != null) {
				if (!_className.EndsWith(ReferenceClassNameSuffix))
					return false;

				_className = _className.Substring(0, _className.Length - ReferenceClassNameSuffix.Length);
			}

			if (ReferenceNamespaceSuffix != null) {
				if (!_namespace.EndsWith(ReferenceNamespaceSuffix))
					return false;

				_namespace = _namespace.Substring(0, _namespace.Length - ReferenceNamespaceSuffix.Length);
			}

			if (TargetClassNameSuffix != null) {
				_className += TargetClassNameSuffix;
			}

			if (TargetNamespaceSuffix != null) {
				_namespace += TargetNamespaceSuffix;
			}

			return true;
		}
	}

	//	public class AssemblyQualifiedClassLocator<Target> : IClassLocator
	//	{
	//		//Type objType = typeof(System.Array);

	//		//// Print the full assembly name.
	//		//Console.WriteLine("Full assembly name:\n   {0}.", 
	//  //                         objType.Assembly.FullName.ToString()); 

	//  //      // Print the qualified assembly name.
	//  //      Console.WriteLine("Qualified assembly name:\n   {0}.", 
	//  //                         objType.AssemblyQualifiedName.ToString()); 
	//// The example displays the following output if run under the .NET Framework 4.5:
	////    Full assembly name:
	////       mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089.
	////    Qualified assembly name:
	////       System.Array, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089.
	//	}
}
