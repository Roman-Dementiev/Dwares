using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;
using Xamarin.Forms;


namespace Dwares.Druid.Services
{
	internal class ReflectionWritSvc : IWritService
	{
		public IWritExecutor GetExecutor(string writ)
		{
			var targetCandidates = new List<object>();
			AddCandidate(targetCandidates, Application.Current.MainPage?.TopNavigationPage(false));
			AddCandidate(targetCandidates, ShellPageEx.Current);
			AddCandidate(targetCandidates, Shell.Current);
			AddCandidate(targetCandidates, Application.Current);

			foreach (var target in targetCandidates) {
				var execute = Reflection.GetMethod(target, ExecuteMethodName(writ), null, null, false);
				if (execute == null)
					continue;
				// TODO: check return type

				//var canExecute = Reflection.GetMethod(target, CanExecuteMethodName(writ), null, null, false);
				//// TODO: check return type
				//return new ReflectionWritExecutor(target, execute, canExecute);

				return new ReflectionWritExecutor(target, execute);
			}

			return null;
		}

		static void AddCandidate(IList candidates, object obj, bool checkBindable = true)
		{
			if (obj == null || candidates.Contains(obj))
				return;

			candidates.Add(obj);

			if (checkBindable && obj is BindableObject bindable) {
				AddCandidate(candidates, bindable.BindingContext, false);
			}
		}


		public static string ExecuteMethodNameFormat { get; set; } = "Execute{0}";
		public static string CanExecuteMethodNameFormat { get; set; } = "CanExecute{0}";
		public static string ExecuteMethodName(string writ) => string.Format(ExecuteMethodNameFormat, writ);
		public static string CanExecuteMethodName(string writ) => string.Format(CanExecuteMethodNameFormat, writ);
	}

	internal class ReflectionWritExecutor : IWritExecutor
	{
		public ReflectionWritExecutor(object target, MethodInfo executeMethod)
		{
			Target = target ?? throw new ArgumentNullException(nameof(target));
			ExecuteMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
		}

		//public ReflectionWritExecutor(object target, MethodInfo executeMethod, MethodInfo canExecuteMethod)
		//{
		//	Target = target ?? throw new ArgumentNullException(nameof(target));
		//	ExecuteMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
		//	CanExecuteMethod = canExecuteMethod;
		//}

		public object Target { get; }
		private MethodInfo ExecuteMethod { get; set; }
		//private MethodInfo CanExecuteMethod { get; set; }


		public Task ExecuteWrit(string writ)
		{
			try {
				var result = ExecuteMethod.Invoke(Target, Reflection.cNoArgs);
				if (result is Task task) {
					return task;
				}
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			return Task.CompletedTask;
		}

		//public bool CanExecuteWrit(string writ)
		//{
		//	try {
		//		var result = CanExecuteMethod.Invoke(Target, Reflection.cNoArgs);
		//		if (result is bool canExecute) {
		//			return canExecute;
		//		}
		//	}
		//	catch (Exception exc) {
		//		Debug.ExceptionCaught(exc);
		//	}
		//	return true;

		//}

	}
}
