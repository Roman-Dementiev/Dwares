using System;
using System.Collections.Generic;
using System.Reflection;


namespace Dwares.Dwarf.Runtime
{
	public static class Reflection
	{
		public static readonly Type[] cNoParams = new Type[0];
		public static readonly object[] cNoArgs = new object[0];

		public static TResult EvalForType<TResult>(Type type, Func<Type, TResult> eval) where TResult : class
		{
			while (type != null) {
				var result = eval(type);
				if (result != null)
					return result;

				type = type.GetTypeInfo().BaseType;
			}
			return null;
		}

		//public static TResult EvalForType<TResult>(Type type, Func<Type, TResult> eval, Func<TResult, bool> test = null) where TResult : struct
		//{
		//	if (test == null) {
		//		test = (result) => !result.Equals(default(TResult));
		//	}

		//	while (type != null) {
		//		var result = eval(type);
		//		if (test(result))
		//			return result;

		//		type = type.GetTypeInfo().BaseType;
		//	}
		//	return default(TResult);
		//}

		//public static bool TestType(Type type, Func<Type, bool> test = null)
		//{
		//	//return EvalForType(type, test, null);

		//	while (type != null) {
		//		if (test(type))
		//			return true;

		//		type = type.GetTypeInfo().BaseType;
		//	}
		//	return false;

		//}

		public static bool IsExplicitName(string name) => name.IndexOf('.') > 0;

		public static string[] GetPropertyNames(
			object target, 
			bool? isReadable = null, 
			bool? isWritable = null, 
			bool? isSpecial = false,
			bool? isExplicit = false)
		{
			var type = target as Type;
			if (type == null) {
				type = target.GetType();
			}

			var properties = type.GetRuntimeProperties();
			var names = new List<string>();
			foreach (var pi in properties) {
				if ((isReadable != null && pi.CanRead != isReadable) ||
					(isWritable != null && pi.CanWrite != isWritable) ||
					(isSpecial != null && pi.IsSpecialName != isSpecial) ||
					(isExplicit != null && IsExplicitName(pi.Name) != isExplicit))
					continue;

				names.Add(pi.Name);
			}

			return names.ToArray();
		}

		public static PropertyInfo GetProperty(object target, string propertyName, bool required = false)
		{
			var type = target as Type;
			if (type == null) {
				type = target.GetType();
			}

			PropertyInfo propertyInfo = type.GetRuntimeProperty(propertyName);
			if (required && propertyInfo == null) {
				throw new ArgumentException("Property not found", propertyName);
			}
			return propertyInfo;
		}

		public static Type GetPropertyType(object target, PropertyInfo propertyInfo)
		{
			if (propertyInfo == null) {
				throw new ArgumentNullException(nameof(propertyInfo));
			}

			if (propertyInfo.CanRead) {
				var value = GetPropertyValue(target, propertyInfo);
				if (value != null)
					return value.GetType();
			}

			//if (propertyInfo.CanWrite) {
			//	MethodInfo method = propertyInfo.SetMethod;
			//	ParameterInfo param = method.GetParameters()[0];
			//	return param.ParameterType;
			//}

			return propertyInfo.PropertyType;
		}

		public static object GetPropertyValue(object target, PropertyInfo propertyInfo, Type type = null)
		{
			if (target == null) {
				throw new ArgumentNullException(nameof(target));
			}
			if (propertyInfo == null) {
				throw new ArgumentNullException(nameof(propertyInfo));
			}
			if (type != null && propertyInfo.PropertyType != type) {
				throw new ArgumentException("Property type mismatch", propertyInfo.Name);
			}
			if (propertyInfo.GetMethod == null) {
				throw new ArgumentException("Property is not readable", propertyInfo.Name);
			}

			return propertyInfo.GetMethod.Invoke(target, null);
		}

		public static void SetPropertyValue(object target, PropertyInfo propertyInfo, object value, Type type = null)
		{
			if (target == null) {
				throw new ArgumentNullException(nameof(target));
			}
			if (propertyInfo == null) {
				throw new ArgumentNullException(nameof(propertyInfo));
			}
			if (type != null && propertyInfo.PropertyType != type) {
				throw new ArgumentException("Property type mismatch", propertyInfo.Name);
			}
			if (propertyInfo.SetMethod == null) {
				throw new ArgumentException("Property is not writable", propertyInfo.Name);
			}

			propertyInfo.SetMethod.Invoke(target, new object[] { value });
		}

		public static object GetPropertyValue(object target, string propertyName, bool required, Type type = null)
		{
			var propertyInfo = GetProperty(target, propertyName, required);
			if (propertyInfo != null) {
				return GetPropertyValue(target, propertyInfo, type);
			} else {
				return null;
			}
		}

		public static void SetPropertyValue(object target, string propertyName, object value, bool required, Type type = null)
		{
			PropertyInfo propertyInfo = GetProperty(target, propertyName, required);
			if (propertyInfo != null) {
				SetPropertyValue(target, propertyInfo, value, type);
			}
		}

		//public static TValue GetPropertyValue<TValue>(object target, string propertyName, bool required)
		//{
		//	return (TValue)GetPropertyValue(target, propertyName, required, typeof(TValue));
		//}

		//public static void SetPropertyValue<TValue>(object target, TValue value, string propertyName, bool required)
		//{
		//	SetPropertyValue(target, value, propertyName, required, typeof(TValue));
		//}

		public static bool HasMethod(object target, string methodName) =>
				GetMethod(target, methodName, false) != null;

		public static MethodInfo GetMethod(object target, string methodName, bool required)
		{
			if (target == null) {
				throw new ArgumentNullException(nameof(target));
			}
			if (methodName == null) {
				throw new ArgumentNullException(nameof(methodName));
			}

			var type = target as Type;
			if (type == null) {
				type = target.GetType();
			}

			var methods = type.GetRuntimeMethods();
			foreach (var methodInfo in methods) {
				if (methodInfo.Name == methodName)
					return methodInfo;
			}

			if (required) {
				throw new ArgumentException("Method not found", methodName);
			}
			return null;
		}

		public static bool HasMethod(object target, string methodName, Type returnType = null, params Type[] argTypes) =>
			GetMethod(target, methodName, argTypes, returnType, false) != null;


		public static MethodInfo GetMethod(object target, string methodName, Type[] argTypes, bool required) =>
			GetMethod(target, methodName, null, required);

		public static MethodInfo GetMethod(object target, string methodName, Type[] argTypes, Type returnType, bool required)
		{
			if (target == null) {
				throw new ArgumentNullException(nameof(target));
			}
			if (methodName == null) {
				throw new ArgumentNullException(nameof(methodName));
			}

			var type = target as Type;
			if (type == null) {
				type = target.GetType();
			}

			var methodInfo = type.GetRuntimeMethod(methodName, argTypes ?? cNoParams);
			if (methodInfo == null) {
				if (required) {
					throw new ArgumentException("Method not found", methodName);
				}
				return null;
			}

			if (returnType != null && methodInfo.ReturnType != returnType) {
				throw new ArgumentException("Method return type mismatch", methodName);
			}

			return methodInfo;
		}

		public static object InvokeMethod(object target, string methodName, out bool invoked)
		{
			var methodInfo = GetMethod(target, methodName, cNoParams, null, false);
			if (methodInfo != null) {
				invoked = true;
				return methodInfo?.Invoke(target, cNoArgs);
			} else {
				invoked = false;
				return null;
			}
		}

		public static object InvokeMethod(object target, string methodName, bool required = true)
		{
			var methodInfo = GetMethod(target, methodName, cNoParams, null, required);
			return methodInfo?.Invoke(target, new object[] { });
		}

		public static object InvokeMethod(object target, string methodName, Type argType, object arg, bool required = true)
		{
			MethodInfo methodInfo = GetMethod(target, methodName, new Type[] { argType }, null, required);
			return methodInfo?.Invoke(target, new object[] { arg });
		}

		public static object InvokeMethod(object target, string methodName, Type[] argTypes, object[] args, bool required = true)
		{
			MethodInfo methodInfo = GetMethod(target, methodName, argTypes, null, required);
			return methodInfo?.Invoke(target, args);
		}

		//public static object InvokeMethod(object target, string methodName, bool required = true)
		//{
		//	MethodInfo methodInfo = GetMethod(target, methodName, cNoParams, null, required);
		//	return methodInfo?.Invoke(target, null);
		//}

		public static TReturn InvokeMethod<TReturn>(object target, string methodName, Type[] argTypes, object[] args, bool required = true)
		{
			MethodInfo methodInfo = GetMethod(target, methodName, argTypes, typeof(TReturn), required);
			if (methodInfo != null) {
				return (TReturn)methodInfo.Invoke(target, args);
			} else {
				return default(TReturn);
			}
		}

		public static Type[] GetArgumentTypes(object[] args)
		{
			Type[] argTypes = new Type[args.Length];
			for (int i = 0; i < args.Length; i++) {
				var arg = args[i] ?? throw new ArgumentNullException(String.Format("{0}[{1}]", nameof(args), i));
				if (arg is INull @null) {
					argTypes[i] = @null.Type();
				} else {
					argTypes[i] = args[i].GetType();
				}
			}
			return argTypes;
		}

		public static TReturn InvokeMethod<TReturn>(object target, string methodName, object[] args, bool required = true)
		{
			var argTypes = GetArgumentTypes(args);
			var methodInfo = GetMethod(target, methodName, argTypes, typeof(TReturn), required);
			if (methodInfo != null) {
				return (TReturn)methodInfo.Invoke(target, args);
			} else {
				return default(TReturn);
			}
		}

		public static object InvokeMethod(object target, string methodName, object[] args, bool required = true)
		{
			var argTypes = GetArgumentTypes(args);
			var methodInfo = GetMethod(target, methodName, argTypes, typeof(void), required);
			if (methodInfo != null) {
				return methodInfo.Invoke(target, args);
			} else {
				return typeof(void);
			}
		}

		public static string[] ParametersNames(MethodInfo methodInfo)
		{
			if (methodInfo == null)
				throw new ArgumentNullException(nameof(methodInfo));

			var parameters = methodInfo.GetParameters();
			var names = new string[parameters.Length];
			for (int i = 0; i < parameters.Length; i++) {
				names[i] = parameters[i].Name;
			}
			return names;
		}

		public static string[] ParametersNames(object target, string methodName, bool required = true)
		{
			var methodInfo = GetMethod(target, methodName, required);
			if (methodInfo == null)
				return null;

			return ParametersNames(methodInfo);
		}

		public static ConstructorInfo GetDefaultConstructor(Type type, bool publicOnly = true)
		{
			var typeInfo = type.GetTypeInfo();

			var constructors = typeInfo.DeclaredConstructors;
			foreach (var ctor in constructors)
			{
				if (ctor.GetParameters().Length == 0) {
					if (publicOnly && !ctor.IsPublic)
						break;
					return ctor;
				}
			}

			return null;
		}
	}
}
