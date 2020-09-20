using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dwares.Dwarf.Runtime;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf
{
	public class DebugWire : Wire
	{
		public override void Send(string message) => System.Diagnostics.Debug.WriteLine(message);
	}


	public static class Debug
	{
		const string AssertFailed = "Assert.Failed";
		const string Failed = "Failed";
		const string ExceptionCaughtFormat = "Exception caught: {0}";
#if DEBUG
		public static readonly Tracer Tracer = new Tracer(new DebugWire());
		//public static IWire Wire => Tracer.Wire;
#else
		public static readonly Tracer Tracer = null;
#endif

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Assert(bool condition, string message = null, string detailFormat = null, params object[] detailArgs)
		{
			if (!String.IsNullOrEmpty(detailFormat)) {
				var details = String.Format(detailFormat, detailArgs);
				System.Diagnostics.Debug.Assert(condition, message ?? AssertFailed, details);
			} else if (String.IsNullOrEmpty(message)) {
				System.Diagnostics.Debug.Assert(condition);
			} else {
				System.Diagnostics.Debug.Assert(condition, message);
			}
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Fail(string message = null, string detailFormat = null, params object[] detailArgs)
		{
			if (String.IsNullOrEmpty(message))
				message = Failed;

			if (!String.IsNullOrEmpty(detailFormat)) {
				var details = String.Format(detailFormat, detailArgs);
				System.Diagnostics.Debug.Fail(message, details);
			} else {
				System.Diagnostics.Debug.Fail(message);
			}
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void AssertNotNull(object obj, string message = null, string detailFormat = null, params object[] detailArgs)
		{
			Assert(obj != null, message, detailFormat, detailArgs);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void AssertIsNull(object obj, string message = null, string detailFormat = null, params object[] detailArgs)
		{
			Assert(obj == null, message, detailFormat, detailArgs);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void AssertIsEmpty(object obj, string message = null, string detailFormat = null, params object[] detailArgs)
		{
			bool isEmpty = string.IsNullOrEmpty(obj?.ToString());
			Assert(isEmpty, message, detailFormat, detailArgs);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void AssertNotEmpty(object obj, string message = null, string detailFormat = null, params object[] detailArgs)
		{
			bool isEmpty = string.IsNullOrEmpty(obj?.ToString());
			Assert(!isEmpty, message, detailFormat, detailArgs);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void ExceptionCaught(Exception ex)
		{
			Print(ExceptionCaughtFormat, ex);
		}


		//[System.Diagnostics.Conditional("DEBUG")]
		//public static void MethodMessage(string message = null, [CallerMemberName] string method = null)
		//{
		//	if (string.IsNullOrWhiteSpace(message)) {
		//		if (method == null)
		//			return;
		//		message = $"[{method}]";
		//	}
		//	else {
		//		if (!string.IsNullOrEmpty(method))
		//			message = $"[{method}]: {message}";
		//	}
			
		//	System.Diagnostics.Debug.WriteLine(message);
		//}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Print(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Print(string format, object arg0)
		{
			var message = String.Format(format, arg0);
			System.Diagnostics.Debug.WriteLine(message);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Print(string format, params object[] args)
		{
			var message = String.Format(format, args);
			System.Diagnostics.Debug.WriteLine(message);
		}

#if DEBUG
		public static bool IsTracing(CompilationUnit unit) => Tracer.IsTracing(unit);
#else
		public static bool IsTracing(CompilationUnit unit) => false;
#endif

		//[System.Diagnostics.Conditional("DEBUG")]
		//public static bool? GetTracing(CompilationUnit unit) => Tracer.GetTracing(unit);
		
		[System.Diagnostics.Conditional("DEBUG")]
		public static void SetTracing(IEnumerable<CompilationUnit> units, bool? value) => Tracer.SetTracing(units, value);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void SetTracing(bool? value, params CompilationUnit[] units) => SetTracing(units, value);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void EnableTracing(params CompilationUnit[] units) => SetTracing(units, true);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void DisableTracing(params CompilationUnit[] units) => SetTracing(units, false);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void ClearTracing(params CompilationUnit[] units) => SetTracing(units, null);

		//[System.Diagnostics.Conditional("DEBUG")]
		//public static void TraceMessage(CompilationUnit unit, object arg = null)
		//	=> Tracer.TraceMessage(unit, arg?.ToString());
		//public static void TraceMessage(CompilationUnit unit, string format, params object[] args)
		//	=> Tracer.TraceMessage(unit, format, args);

		//[System.Diagnostics.Conditional("DEBUG")]
		//public static void Trace(ClassUnit @class, string method, string message = null)
		//	=> Tracer.Trace(@class, method, message);
		//public static void Trace(ClassUnit @class, string method, string format, params object[] args)
		//	=> Tracer.Trace(@class, method, format, args);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Trace(ClassUnit @class, string method)
			=> Tracer.Trace(@class, method, "");

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Trace(ClassUnit @class, string method, string format, params object[] args)
			=> Tracer.Trace(@class, method, format, args);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void TraceNamedValues(ClassUnit @class, string method, params object[] namesAndValues)
			=> Tracer.TraceNamedValues(@class, method, namesAndValues);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void TraceValues(ClassUnit @class, string method, string[] names, params object[] values)
			=> Tracer.TraceValues(@class, method, names, values);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void TraceProperties(ClassUnit @class, string method, object target, params string[] names)
			=> Tracer.TraceProperties(@class, method, target, names);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void TraceMethodArgs(ClassUnit @class, string method, params object[] args)
			=> Tracer.TraceMethodArgs(@class, method, args);
	}
}
