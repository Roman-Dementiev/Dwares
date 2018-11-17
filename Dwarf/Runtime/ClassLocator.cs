using System;
using System.Reflection;


namespace Dwares.Dwarf.Runtime
{
	public interface IClassLocator
	{
		Type TargetType { get; }
		//Type ReferenceBaseType { get; }

		Type LocateForRef(object reference);
		Type LocateForRefType(Type refType);
	}

	public abstract class BaseClassLocator<Target> : IClassLocator
	{
		public Type TargetType => typeof(Target);

		public virtual Type LocateForRef(object reference)
		{
			return LocateForRefType(reference?.GetType());
		}

		public abstract Type LocateForRefType(Type refType);
	}



	public class ClassLocator<Target> : BaseClassLocator<Target>
	{
		public string ReferenceClassNameSuffix { get; set; }
		public string ReferenceNamespaceSuffix { get; set; }
		public string TargetClassNameSuffix { get; set; }
		public string TargetNamespaceSuffix { get; set; }

		public override Type LocateForRefType(Type refType)
		{

			if (refType == null)
				return null;

			var typeInfo = refType.GetTypeInfo();
			var _className = typeInfo.Name;
			var _namespace = typeInfo.Namespace;

			if (ReferenceClassNameSuffix != null) {
				if (!_className.EndsWith(ReferenceClassNameSuffix))
					return null;

				_className = _className.Substring(0, _className.Length - ReferenceClassNameSuffix.Length);
			}

			if (ReferenceNamespaceSuffix != null) {
				if (!_namespace.EndsWith(ReferenceNamespaceSuffix))
					return null;

				_namespace = _namespace.Substring(0, _namespace.Length - ReferenceNamespaceSuffix.Length);
			}

			if (TargetClassNameSuffix != null) {
				_className += TargetClassNameSuffix;
			}

			if (TargetNamespaceSuffix != null) {
				_namespace += TargetNamespaceSuffix;
			}

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
	}

//	public class AssemblyQualifiedClassLocator<Target, ReferenceBase> : BaseClassLocator<Target, ReferenceBase>
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
