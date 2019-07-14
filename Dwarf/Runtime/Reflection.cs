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

			var propertyInfo = type.GetRuntimeProperty(propertyName);
			Guard.Verify(!required || propertyInfo != null,
				$"Property '{propertyName}' not found for {type}");

			return propertyInfo;
		}

		public static Type GetPropertyType(object target, PropertyInfo propertyInfo)
		{
			Guard.ArgumentNotNull(propertyInfo, nameof(propertyInfo));

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

		public static object GetPropertyValue(object target, PropertyInfo propertyInfo)
		{
			Guard.ArgumentNotNull(target, nameof(target));
			Guard.ArgumentNotNull(propertyInfo, nameof(propertyInfo));
			Guard.ArgumentIsValid(nameof(propertyInfo), propertyInfo.GetMethod != null,
				$"Peoperty '{propertyInfo.Name} is not readable");

			return propertyInfo.GetMethod.Invoke(target, null);
		}

		public static void SetPropertyValue(object target, PropertyInfo propertyInfo, object value)
		{
			Guard.ArgumentNotNull(target, nameof(target));
			Guard.ArgumentNotNull(propertyInfo, nameof(propertyInfo));
			Guard.ArgumentIsValid(nameof(propertyInfo), propertyInfo.SetMethod != null,
				$"Peoperty '{propertyInfo.Name} is not writable");

			if (propertyInfo == null) {
				throw new ArgumentNullException(nameof(propertyInfo));
			}

			propertyInfo.SetMethod.Invoke(target, new object[] { value });
		}

		public static object GetPropertyValue(object target, string propertyName, bool required)
		{
			var propertyInfo = GetProperty(target, propertyName, required);
			if (propertyInfo != null) {
				return GetPropertyValue(target, propertyInfo/*, type*/);
			} else {
				return null;
			}
		}

		public static void SetPropertyValue(object target, string propertyName, object value, bool required)
		{
			var propertyInfo = GetProperty(target, propertyName, required);
			if (propertyInfo != null) {
				SetPropertyValue(target, propertyInfo, value);
			}
		}


		public static bool TrySetPropertyValue(object target, string propertyName, object value)
			=> TrySetPropertyValue<object>(target, propertyName, value);

		public static bool TrySetPropertyValue<Target>(Target target, string propertyName, object value)
		{
			Guard.ArgumentNotNull(target, nameof(target));
			Guard.ArgumentNotEmpty(propertyName, nameof(propertyName));

			var propertyInfo = target.GetPropertyInfo(propertyName);
			if (propertyInfo?.SetMethod == null)
				return false;

			if (value != null && !propertyInfo.PropertyType.IsAssignableFrom(value.GetType())) {
				try {
					value = Convert.ChangeType(value, propertyInfo.PropertyType);
				}
				catch (Exception exc) {
					Debug.Print($"Reflection.TrySetPropertyValue(): can not convert {value} to {propertyInfo.PropertyType} => {exc}");
					return false;
				}
			}

			try {
				SetPropertyValue(target, propertyInfo, value);
			}
			catch (Exception exc) {
				Debug.Print($"Reflection.TrySetPropertyValue(): can not set property {propertyName} to {value} => {exc}");
				return false;
			}

			return true;
		}

		//public static TValue GetPropertyValue<TValue>(object target, string propertyName, bool required)
		//{
		//	return (TValue)GetPropertyValue(target, propertyName, required, typeof(TValue));
		//}

		//public static void SetPropertyValue<TValue>(object target, TValue value, string propertyName, bool required)
		//{
		//	SetPropertyValue(target, value, propertyName, required, typeof(TValue));
		//}

		public static string GetStringProperty(object target, string propertyName, bool required, bool convert = false)
		{
			var value = GetPropertyValue(target, propertyName, required);
			if (value is string str)
				return str;

			if (convert && value != null) {
				return value.ToString();
			} else {
				return null;
			}
		}

		public static bool HasMethod(object target, string methodName) =>
				GetMethod(target, methodName, false) != null;

		public static MethodInfo GetMethod(object target, string methodName, bool required)
		{
			Guard.ArgumentNotNull(target, nameof(target));
			Guard.ArgumentNotEmpty(methodName, nameof(methodName));

			var type = target as Type;
			if (type == null) {
				type = target.GetType();
			}

			var methods = type.GetRuntimeMethods();
			foreach (var methodInfo in methods) {
				if (methodInfo.Name == methodName)
					return methodInfo;
			}

			Guard.Verify(!required, $"Method '{methodName}' not found for {type}");
			return null;
		}

		public static bool HasMethod(object target, string methodName, Type returnType = null, params Type[] argTypes) =>
			GetMethod(target, methodName, argTypes, returnType, false) != null;


		public static MethodInfo GetMethod(object target, string methodName, Type[] argTypes, bool required) =>
			GetMethod(target, methodName, null, required);

		public static MethodInfo GetMethod(object target, string methodName, Type[] argTypes, Type returnType, bool required)
		{
			Guard.ArgumentNotNull(target, nameof(target));
			Guard.ArgumentNotEmpty(methodName, nameof(methodName));

			var type = target as Type;
			if (type == null) {
				type = target.GetType();
			}

			var methodInfo = type.GetRuntimeMethod(methodName, argTypes ?? cNoParams);
			if (methodInfo == null) {
				Guard.Verify(!required, $"Method '{methodName}' not found for {type}");
				return null;
			}

			Guard.Verify(returnType == null || methodInfo.ReturnType != returnType, 
				$"Method '{methodName}' return type mismatch for {type}");

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
			var methodInfo = GetMethod(target, methodName, argTypes, typeof(TReturn), required);
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
