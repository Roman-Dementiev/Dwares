using System;
using System.Collections.Generic;
using System.Reflection;


namespace Dwares.Dwarf.Runtime
{
	//
	// Summary:
	//     Filters the classes represented in an array of System.Type objects.
	//
	// Parameters:
	//   m:
	//     The Type object to which the filter is applied.
	//
	//   filterCriteria:
	//     An arbitrary object used to filter the list.
	public delegate bool TypeFilter(Type m, object filterCriteria);

	public static class Extensions
	{
		public static bool IsClass(this Type type) => type.GetTypeInfo().IsClass;
		public static bool IsPublic(this Type type) => type.GetTypeInfo().IsPublic;
		public static bool IsSealed(this Type type) => type.GetTypeInfo().IsSealed;
		public static bool IsAbstract(this Type type) => type.GetTypeInfo().IsAbstract;
		public static bool IsInterface(this Type type) => type.GetTypeInfo().IsInterface;
		public static bool IsPrimitive(this Type type) => type.GetTypeInfo().IsPrimitive;
		public static bool IsValueType(this Type type) => type.GetTypeInfo().IsValueType;
		public static bool IsGenericType(this Type type) => type.GetTypeInfo().IsGenericType;
		public static bool IsGenericParameter(this Type type) => type.GetTypeInfo().IsGenericParameter;
		public static bool IsGenericTypeDefinition(this Type type) => type.GetTypeInfo().IsGenericTypeDefinition;
		public static Type BaseType(this Type type) => type.GetTypeInfo().BaseType;
		public static IEnumerable<Type> ImplementedInterfaces(this Type type) => type.GetTypeInfo().ImplementedInterfaces;
		public static string Namespace(this Type type) => type.GetTypeInfo().Namespace;
		public static Assembly Assembly(this Type type) => type.GetTypeInfo().Assembly;
		public static TypeAttributes Attributes(this Type type) => type.GetTypeInfo().Attributes;

		public static bool ImplementsInterface(this Type type, Type interfaceType)
		{
			var interfaces = type.ImplementedInterfaces();
			foreach (var i in interfaces) {
				if (i == interfaceType) {
					return true;
				}
			}
			return false;
		}

		public static bool ImplementsInterface(this Type type, string interfaceName)
		{
			var interfaces = type.ImplementedInterfaces();
			foreach (var i in interfaces) {
				if (i.Name == interfaceName) {
					return true;
				}
			}
			return false;
		}

		public static bool IsAssignableFrom(this Type type, Type assignmentType)
		{
			return type.GetTypeInfo().IsAssignableFrom(assignmentType.GetTypeInfo());
		}

		public static bool IsDerivedFrom(this Type type, Type baseType)
		{
			while (type != null) {
				if (type == baseType)
					return true;

				type = type.BaseType;
			}

			return false;
		}


		//public virtual Type[] FindInterfaces(this TypeInfo typeInfo, TypeFilter filter, object filterCriteria)
		//{

		//}

		//public static Type[] GetGenericArguments(this Type type)
		//{
		//	return type.GetTypeInfo().GetGenericArguments();
		//}
	}
}
