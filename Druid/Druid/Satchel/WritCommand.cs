using System;
using System.Windows.Input;
using Dwares.Druid.Services;


namespace Dwares.Druid.Satchel
{
	public class WritCommand: ICommand
	{
		public event EventHandler CanExecuteChanged;

		public WritCommand(string writ, IWritExecutor executor = null)
		{
			Writ = writ;
			Executor = executor;
		}

		public string Writ { get; set; }

		public bool IsDinamic { get; set; }

		protected IWritExecutor Executor {
			set {
				executor = value;
			}
			get {
				if (executor == null || IsDinamic) {
					executor = WritService.GetExecutor(Writ);
				}
				return executor;
			}
		}
		IWritExecutor executor;

		public async void Execute(object parameter)
		{
			var executor = Executor;
			await executor?.ExecuteWrit(Writ);
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}


		public void FireCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
