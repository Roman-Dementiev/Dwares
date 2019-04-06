using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;
using Dwares.Dwarf.Toolkit;
using Nito.AsyncEx.Standard.Synchronous;

namespace Dwares.Druid.Satchel
{
	public interface IWritExecutor
	{
		object ExecuteWrit(string writ);
		bool CanExecuteWrit(string writ);
	}

	public class WritExecutor : IWritExecutor
	{
		public WritExecutor(object target)
		{
			Target = target;
		}

		public object Target { get; }

		public object ExecuteWrit(string writ)
		{
			var methodName = ExecuteMethodName(writ);
			return Invoke(methodName, out var invoked);
		}

		public bool CanExecuteWrit(string writ)
		{
			var methodName = CanExecuteMethodName(writ);
			var result = Invoke(methodName, out var invoked);
			if (!invoked) {
				return true;
			}

			if (result is bool canExecute) {
				return canExecute;
			} else {
				return false; //result != null;
			}
		}

		protected object Invoke(string methodName, out bool invoked)
		{
			invoked = false;
			
			for (var target = Target; target != null; target = Descendant.GetParent(target))
			{
				object result = null;
				try {
					result = Reflection.InvokeMethod(target, methodName, out invoked);
				}
				catch (Exception exc) {
					Debug.Print("Exception in WritExecutor calling InvokeMethod");
					Debug.ExceptionCaught(exc);
				}
			}

			return null;	
		}

		public static string ExecuteMethodNameFormat { get; set; } = "On{0}";
		public static string CanExecuteMethodNameFormat { get; set; } = "Can{0}";
		//public const string ExecuteMethodNameFormat = "On{0}";
		//public const string CanExecuteMethodNameFormat = "Can{0}";
		protected virtual string ExecuteMethodName(string writ) => string.Format(ExecuteMethodNameFormat, writ);
		protected virtual string CanExecuteMethodName(string writ) => string.Format(CanExecuteMethodNameFormat, writ);

	}
}
