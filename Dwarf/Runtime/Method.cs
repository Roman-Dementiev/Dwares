#if IoC
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


namespace Dwares.Dwarf.Runtime
{
	public class MethodOptions
	{
		// TODO
	}

	public class Method<Info, Options> 
		where Info : MethodBase
		where Options : MethodOptions
	{
		protected Method(Type methodType, Info methodInfo)
		{
			_Type = methodType ?? throw new AggregateException(nameof(methodType));
			_Info = methodInfo; //?? throw new AggregateException(nameof(methodType));
		}

		protected Type _Type;
		protected Info _Info;



		protected object[] GeArguments(ParametersOverloads parameters, Options options)
		{
			Debug.AssertNotNull(_Info);
			var methodParams = _Info.GetParameters();
			int paramsCount = methodParams.Count();
			object[] args = new object[paramsCount];

			for (int parameterIndex = 0; parameterIndex < paramsCount; parameterIndex++) {
				var param = methodParams[parameterIndex];

				if (parameters.ContainsKey(param.Name)) {
					args[parameterIndex] = parameters[param.Name];
				} else {
					args[parameterIndex] = ResolveParameter(param.Name, param.ParameterType, parameters, options);
				}
			}

			return args;
		}

		protected static Info GetBestMatch(IEnumerable<Info> methods, ParametersOverloads parameters, Options options = null)
		{
			// Get methods in reverse order based on the number of parameters
			// i.e. be as "greedy" as possible so we satify the most amount of dependencies possible
			//var ordered = methods.OrderByDescending(info => info.GetParameters().Count());
			var ordered = from m in methods
						  orderby m.GetParameters().Count() descending
						  select m;

			foreach (var method in ordered) {
				if (IsMatch(method, parameters, options))
					return method;
			}

			return null;
		}

		protected static bool IsMatch(Info info, ParametersOverloads parameters, Options options)
		{
			foreach (var parameter in info.GetParameters()) {
				if (string.IsNullOrEmpty(parameter.Name))
					return false;

				var isParameterOverload = parameters.ContainsKey(parameter.Name);
				if (!isParameterOverload) {
					if (parameter.ParameterType.IsPrimitive())
						return false;

					if (!CanResolveParameter(parameter.Name, parameter.ParameterType, parameters, options))
						return false;
				}
			}

			return true;
		}

		protected static bool CanResolveParameter(string parameterName, Type parameterType, ParametersOverloads parameters, Options options)
		{
			// TODO:
			return false;
		}

		private static object ResolveParameter(string parameterName, Type parameterType, IDictionary<string, object> parameters, Options options)
		{
			// TODO:
			return null;
		}
	}

	public class Method: Method<MethodInfo, MethodOptions>
	{
		public Method(Type methodType, MethodInfo methodInfo) : base(methodType, methodInfo) { }

		public Type MethodType => _Type;
		public MethodInfo MethodInfo { 
			get => _Info; 
			private set => _Info = value; 
		}

		//public object Invoke(object target, object[] args)
		//{
		//	return MethodInfo.Invoke(target, args);
		//}

		public static MethodInfo GetBestMatch(Type type, string methodName, ParametersOverloads parameters, MethodOptions options = null)
		{
			var methods = type.GetTypeInfo().GetDeclaredMethods(methodName);
			return GetBestMatch(methods, parameters, options);
		}

		// TODO
	}
}
#endif
