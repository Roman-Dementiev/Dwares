using System;
using System.Windows.Input;


namespace Dwares.Druid
{
	public class RelayCommand : ICommand
	{
		public RelayCommand(string uid, ICommandTarget target = null)
		{
			Uid = uid;
			Target = target;
			Navigator.CurrentPageChanged += OnCurrentPageChanged;
		}

		private void OnCurrentPageChanged(object sender, EventArgs e)
		{
			if (target == null) {
				ChangeCanExecute();
			}
		}

		public string Uid { get; }
		public bool AlwaysEnabled { get; set; } = false;

		ICommandTarget target;
		public ICommandTarget Target {
			get {
				if (target != null)
					return target;

				return CommandTarget.Current();
			}

			set {
				if (value != target) {
					target = value;
					ChangeCanExecute();
				}
			}
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			if (AlwaysEnabled)
				return true;

			var target = Target;
			if (target != null) {
				return target.CanExecute(Uid, parameter);
			} else {
				return false;
			}
		}

		public void Execute(object parameter)
		{
			//Debug.Print("RelayCommand.Execute({0})", Uid);
			var target = Target;
			if (target != null && target.CanExecute(Uid, parameter)) {
				target.Execute(Uid, parameter);
			}
		}

		public void ChangeCanExecute()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
