#if IoC
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Dwares.Dwarf.Runtime
{
	public class ConstructorOptions: MethodOptions
	{
		// TODO
		public static readonly ConstructorOptions Default = new ConstructorOptions();
	}

	public class Constructor: Method<ConstructorInfo, ConstructorOptions>
	{
		public Constructor(Type typeToConstruct, ConstructorInfo constructorInfo) :
			base(typeToConstruct, constructorInfo)
		{}

		public Constructor(Type typeToConstruct) :
			base(typeToConstruct, null)
		{
			ConstructorInfo = GetDefaultConstructor(typeToConstruct);
			if (ConstructorInfo == null) {
				ConstructorException.Raise(typeToConstruct);
			}
		}

		public Constructor(Type typeToConstruct, ParametersOverloads parameters, ConstructorOptions options = null) :
			base(typeToConstruct, null)
		{
			if (parameters == null) {
				ConstructorInfo = GetDefaultConstructor(typeToConstruct);
			} else {
				ConstructorInfo = GetBestConstructor(typeToConstruct, parameters, options);
			}

			if (ConstructorInfo == null) {
				ConstructorException.Raise(typeToConstruct);
			}
		}

		public Type TypeToConstruct => _Type;
		ConstructorInfo ConstructorInfo {
			get => _Info;
			set => _Info = value;
		}

		public object Construct()
		{
			var ctor = ConstructorInfo ?? GetDefaultConstructor(TypeToConstruct);
			try {
				return ctor.Invoke(new object[0]);
			}
			catch (Exception ex) {
				return ConstructorException.Raise(TypeToConstruct, ex);
			}
		}

		private object Construct(ParametersOverloads parameters, ConstructorOptions options)
		{
			if (parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			if (options == null)
				options = ConstructorOptions.Default;

			var ctor = ConstructorInfo ?? GetBestConstructor(TypeToConstruct, parameters, options);
			try {
				var ctorParams = ConstructorInfo.GetParameters();
				object[] args = new object[ctorParams.Count()];

				for (int parameterIndex = 0; parameterIndex < ctorParams.Count(); parameterIndex++) {
					var param = ctorParams[parameterIndex];

					if (parameters.ContainsKey(param.Name)) {
						args[parameterIndex] = parameters[param.Name];
					} else {
						args[parameterIndex] = ResolveParameter(param.Name, param.ParameterType, parameters, options);
					}
				}

				return ctor.Invoke(args);
			}
			catch (Exception ex) {
				return ConstructorException.Raise(TypeToConstruct, ex);
			}
		}


		public static T New<T>() => (T)New(typeof(T));

		public static T New<T>(ParametersOverloads parameters, ConstructorOptions options = null)
			=> (T)New(typeof(T), parameters, options);

		public static object New(Type type)
		{
			var ctor = new Constructor(type);
			return ctor.Construct();
		}

		public static object New(Type type, ParametersOverloads parameters = null, ConstructorOptions options = null)
		{
			Constructor ctor;
			if (parameters == null) {
				ctor = new Constructor(type);
				return ctor.Construct();
			} else {
				ctor = new Constructor(type, parameters);
				return ctor.Construct(parameters, options);
			}
		}

		public static object New(Type type, ConstructorInfo constructorInfo, ParametersOverloads parameters = null, ConstructorOptions options = null)
		{
			Constructor ctor;
			if (constructorInfo != null) {
				ctor = new Constructor(type, constructorInfo);
				return ctor.Construct(parameters, options);
			}
			else if (parameters == null) {
				ctor = new Constructor(type);
				return ctor.Construct();
			} else {
				ctor = new Constructor(type, parameters);
				return ctor.Construct(parameters, options);
			}
		}

		private static ConstructorInfo GetDefaultConstructor(Type type)
		{
			var typeInfo = type.GetTypeInfo();

			// Get constructors in reverse order based on the number of parameters
			// i.e. be as "greedy" as possible so we satify the most amount of dependencies possible
			var ctors = typeInfo.DeclaredConstructors;

			foreach (var ctor in ctors) {
				if (ctor.GetParameters().Length == 0)
					return ctor;
			}

			return null;
		}

		private static ConstructorInfo GetBestConstructor(Type type, ParametersOverloads parameters, ConstructorOptions options)
		{
			var typeInfo = type.GetTypeInfo();

			// Get constructors in reverse order based on the number of parameters
			// i.e. be as "greedy" as possible so we satify the most amount of dependencies possible
			var ctors = typeInfo.DeclaredConstructors.OrderByDescending(ctor => ctor.GetParameters().Count());
			
			foreach (var ctor in ctors) {
				if (CanConstruct(ctor, parameters, options))
					return ctor;
			}

			return ctors.LastOrDefault();
		}

		private static bool CanConstruct(ConstructorInfo ctor, ParametersOverloads parameters, ConstructorOptions options)
		{
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));

			foreach (var parameter in ctor.GetParameters()) {
				if (string.IsNullOrEmpty(parameter.Name))
					return false;

				var isParameterOverload = parameters.ContainsKey(parameter.Name);
				if (!isParameterOverload) {
					if (parameter.ParameterType.IsPrimitive())
						//#endif
						return false;

					if (!CanResolveParameter(parameter.Name, parameter.ParameterType, parameters, options))
						return false;
				}
			}

			return true;
		}

		private static bool CanResolveParameter(string parameterName, Type parameterType, ParametersOverloads parameters, ConstructorOptions options)
		{
			// TODO:
			return false;
		}

		private static object ResolveParameter(string parameterName, Type parameterType, IDictionary<string, object> parameters, ConstructorOptions options)
		{
			// TODO:
			return null;
		}

	}

	public class ConstructorException : Exception
	{
		public ConstructorException(Type typeToConstruct, string message = null) :
			base(message)
		{
			TypeToConstruct = typeToConstruct;
		}

		public ConstructorException(Type typeToConstruct, string message, Exception innerException) :
			base(message, innerException)
		{
			TypeToConstruct = typeToConstruct;
		}


		public Type TypeToConstruct { get; }

		public static Never Raise(Type typeToConstruct, Exception innerException = null)
			=> Raise(typeToConstruct, null, innerException);

		public static Never Raise(Type typeToConstruct, string message, Exception innerException = null)
		{
			ConstructorException ex;
			if (innerException == null) {
				ex = new ConstructorException(typeToConstruct, message);
			} else {
				ex = new ConstructorException(typeToConstruct, message, innerException);
			}

			throw ex;
		}
	}
}
#endif
