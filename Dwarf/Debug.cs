using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf
{
	public class DebugWire : Wire
	{
		public override void Send(string message) => System.Diagnostics.Debug.WriteLine(message);
	}


	public static class Debug
	{
		const string ExceptionCaughtFormat = "Exception caught: {0}";
		public static readonly Tracer Tracer = new Tracer(new DebugWire());
		//public static IWire Wire => Tracer.Wire;

		public static void Assert(bool condition, string message = null, string detailFormat = null, params object[] detailArgs)
		{
			if (!String.IsNullOrEmpty(detailFormat)) {
				System.Diagnostics.Debug.Assert(condition, message ?? "Assert.Failed", detailFormat, detailArgs);
			} else if (String.IsNullOrEmpty(message)) {
				System.Diagnostics.Debug.Assert(condition);
			} else {
				System.Diagnostics.Debug.Assert(condition, message);
			}
		}

		public static void AssertNotNull(object obj, string message = null, string detailFormat = null, params object[] detailArgs)
		{
			Assert(obj != null, message, detailFormat, detailArgs);
		}

		public static void ExceptionCaught(Exception ex)
		{
			Print(ExceptionCaughtFormat, ex);
		}

		public static void Print(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static void Print(string format, object arg0)
		{
			var message = String.Format(format, arg0);
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static void Print(string format, params object[] args)
		{
			var message = String.Format(format, args);
			System.Diagnostics.Debug.WriteLine(message);
		}

		public static bool IsTracing(CompilationUnit unit) => Tracer.IsTracing(unit);
		//public static bool? GetTracing(CompilationUnit unit) => Tracer.GetTracing(unit);
		public static void SetTracing(IEnumerable<CompilationUnit> units, bool? value) => Tracer.SetTracing(units, value);	
		public static void SetTracing(bool? value, params CompilationUnit[] units) => SetTracing(units, value);
		public static void EnableTracing(params CompilationUnit[] units) => SetTracing(units, true);
		public static void DisableTracing(params CompilationUnit[] units) => SetTracing(units, false);
		public static void ClearTracing(params CompilationUnit[] units) => SetTracing(units, null);

		//public static void TraceMessage(CompilationUnit unit, object arg = null)
		//	=> Tracer.TraceMessage(unit, arg?.ToString());
		//public static void TraceMessage(CompilationUnit unit, string format, params object[] args)
		//	=> Tracer.TraceMessage(unit, format, args);

		//public static void Trace(ClassUnit @class, string method, string message = null)
		//	=> Tracer.Trace(@class, method, message);
		//public static void Trace(ClassUnit @class, string method, string format, params object[] args)
		//	=> Tracer.Trace(@class, method, format, args);

		public static void Trace(ClassUnit @class, string method)
			=> Tracer.Trace(@class, method, "");

		public static void Trace(ClassUnit @class, string method, string format, params object[] args)
			=> Tracer.Trace(@class, method, format, args);

		public static void TraceNamedValues(ClassUnit @class, string method, params object[] namesAndValues)
			=> Tracer.TraceNamedValues(@class, method, namesAndValues);

		public static void TraceValues(ClassUnit @class, string method, string[] names, params object[] values)
			=> Tracer.TraceValues(@class, method, names, values);

		public static void TraceProperties(ClassUnit @class, string method, object target, params string[] names)
			=> Tracer.TraceProperties(@class, method, target, names);

		public static void TraceMethodArgs(ClassUnit @class, string method, params object[] args)
			=> Tracer.TraceMethodArgs(@class, method, args);
	}
}
