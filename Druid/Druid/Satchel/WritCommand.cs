using System;
using System.Windows.Input;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Dwares.Druid.Satchel
{
	public class WritCommand: ICommand //Xamarin.Forms.Command
	{
		public event EventHandler CanExecuteChanged;

		public WritCommand() { }

		public WritCommand(string writ, BindingScope scope = null)
		{
			Writ = writ;
			Scope = scope;

			WritMessage.Subscribe(this, WritMessage.WritCanExecuteChanged, (sender, args) => {
				if (sender == null || sender == Target) {
					RaiseCanExecuteChanged();
				}
			});
		}

		public string Writ { get; set; }
		public BindingScope Scope { get; set; }
		public BindingScope Target => Scope ?? BindingScope.GetCurrentScope();

		public bool CanExecute(object parameter)
		{
			var target = Target;
			if (target != null) {
				return target.WritExecutor.CanExecuteWrit(Writ);
			} else {
				return false;
			}
		}


		public void Execute(object parameter)
		{
			var target = Target;
			if (target != null) {
				target.WritExecutor.ExecuteWrit(Writ);
			}
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		public static void ExecuteWrit(string writ)
		{
			var target = BindingScope.GetCurrentScope();
			if (target != null) {
				target.WritExecutor.ExecuteWrit(writ);
			}
		}
	}

	public class WritMessage
	{
		public const string WritCanExecuteChanged = nameof(WritCanExecuteChanged);

		public string Writ { get; set; }

		public static void Subscribe(object subscriber, string messageId, Action<object, WritMessage> callback)
		{
			MessagingCenter.Subscribe(subscriber, messageId, callback);
		}

		public static void Unsubscribe(object subscriber, string messageId)
		{
			MessagingCenter.Unsubscribe<object>(subscriber, messageId);
		}

		public static void Send(object sender, string messageId, string writ)
		{
			var message = new WritMessage { Writ = writ };
			MessagingCenter.Send(sender, messageId, message);
		}
	}
}
