using System;
using System.Windows.Input;


namespace Dwares.Druid.Support
{
	public class WritCommand: ICommand //Xamarin.Forms.Command
	{
		public event EventHandler CanExecuteChanged;

		public WritCommand() { }

		public WritCommand(string writ, BindingScope scope = null)
		{
			Writ = writ;
			Scope = scope;
		}

		public BindingScope Scope { get; set; }
		public string Writ { get; set; }

		Func<object, bool> canExecute;
		public bool CanExecute(object parameter)
		{
			if (canExecute != null) {
				return canExecute.Invoke(parameter);
			} else {
				return true;
			}
		}

		public void Execute(object parameter)
		{
			var scope = Scope ?? BindingScope.GetCurrentScope();
			scope?.ExecuteWrit(Writ, parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
