using System;
using System.Windows.Input;


namespace Dwares.Druid.Support
{
	public class Order: ICommand //Xamarin.Forms.Command
	{
		public event EventHandler CanExecuteChanged;

		public Order(string uid)
		{
			Uid = uid;
		}

		public String Uid { get; set; }

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
			var scope = BindingScope.GetCurrentScope();
			scope.ExecuteOrder(Uid, parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
