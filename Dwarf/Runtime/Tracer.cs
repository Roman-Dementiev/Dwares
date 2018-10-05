using System;
using System.Collections;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Runtime
{
	public class Tracer
	{

		public Tracer(IWire wire)
		{
			Wire = wire;
		}

		public IWire Wire { get; }

		public bool IsTracing(CompilationUnit unit) => unit?.IsTracing == true;
		//public bool? GetTracing(CompilationUnit unit) => unit?.Tracing;
		//public void SetTracing(CompilationUnit unit, bool? value) { if (unit != null) unit.Tracing = value; }

		public void SetTracing(bool? value, params CompilationUnit[] units) => SetTracing(units, value);
		public void EnableTracing(params CompilationUnit[] units) => SetTracing(units, true);
		public void DisableTracing(params CompilationUnit[] units) => SetTracing(units, false);
		public void ClearTracing(params CompilationUnit[] units) => SetTracing(units, null);
		public void SetTracing(IEnumerable<CompilationUnit> units, bool? value)
		{
			foreach (var unit in units) {
				unit.Tracing = value;
			}
		}

		void SendMessage(CompilationUnit unit, string method, string message)
		{
			var source = Strings.JoinNonEmpty(".", unit.FullName, method);
			Wire.Send(source, message);
		}


		public void TraceMessage(CompilationUnit unit, string message = null)
		{
			if (!IsTracing(unit))
				return;

			SendMessage(unit, null, message);
		}

		public void TraceMessage(CompilationUnit unit, string format, params object[] args)
			=> TraceMessage(unit, String.Format(format, args));

		public void Trace(ClassUnit @class, string method, string message)
		{
			if (!IsTracing(@class))
				return;

			SendMessage(@class, method, message);
		}

		public void Trace(ClassUnit @class, string method, string format, object[] args)
		{
			if (!IsTracing(@class))
				return;

			var message = String.Format(format, args);
			SendMessage(@class, method, message);
		}

		public void TraceNamedValues(ClassUnit @class, string method, IEnumerable<string> names, IEnumerable values)
		{
			if (!IsTracing(@class))
				return;

			var message = Strings.NamedValues(names, values);
			SendMessage(@class, method, message);
		}

		public void TraceNamedValues(ClassUnit @class, string method, IEnumerable namesAndValues)
		{
			if (!IsTracing(@class))
				return;

			var message = Strings.NamedValues(namesAndValues);
			SendMessage(@class, method, message);
		}

		public void TraceUnnamedValues(ClassUnit @class, string method, IEnumerable values)
		{
			if (!IsTracing(@class))
				return;

			var message = Strings.UnnamedValues(values);
			SendMessage(@class, method, message);
		}

		public void TraceValues(ClassUnit @class, string method, string[] names, object[] values)
		{
			if (names != null) {
				TraceNamedValues(@class, method, names, values);
			} else {
				TraceUnnamedValues(@class, method, values);
			}
		}

		public void TraceProperties(ClassUnit @class, string method, object target, IEnumerable<string> names)
		{
			if (!IsTracing(@class))
				return;

			var message = Strings.Properties(target, names);
			SendMessage(@class, method, message);
		}

		public void TraceMethodArgs(ClassUnit @class, string method, IEnumerable args)
		{
			var names = Reflection.ParametersNames(@class.Type, method, false);
			if (names == null)
				return;

			var message = Strings.NamedValues(names, args);
			SendMessage(@class, method, message);
		}

		//public bool IsTracing(Type type) => IsTracing(CompilationUnit.Class(type, false));
		//public bool? GetTracing(Type type) => GetTracing(CompilationUnit.Class(type, false));
		//public void SetTracing(IEnumerable<Type> types, bool? value)
		//{
		//	foreach (var type in types) {
		//		SetTracing(value, CompilationUnit.Class(type, true));
		//	}
		//}

		//public void SetTracing(bool? value, params Type[] types) => SetTracing(types, value);
		//public void EnableTracing(params Type[] types) => SetTracing(types, true);
		//public void DisableTracing(params Type[] types) => SetTracing(types, false);
		//public void ClearTracing(params Type[] types) => SetTracing(types, null);
	}
}
