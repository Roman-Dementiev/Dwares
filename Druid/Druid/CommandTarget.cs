using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace Dwares.Druid
{
	public interface ICommandTarget
	{
		ICommandTarget ChainOfCommand { get; set; }
		bool CanExecute(string commandUid, object parameter);
		void Execute(string commandUid, object parameter);
		Command GetCommand(string commandUid, bool ownOnly = false);
		void ChangeCanExecute();
	}

	public class CommandTarget : ICommandTarget
	{
		static ICommandTarget root;
		public static ICommandTarget Root {
			get => root ?? Application.Current.BindingContext as ICommandTarget;
			set => root = value;
		}

		public CommandTarget(ICommandTarget chainOfCommand = null)
		{
			ChainOfCommand = chainOfCommand;
		}

		public ICommandTarget ChainOfCommand { get; set; }

		IEnumerable<Command> commands = null;
		public IEnumerable<Command> Commands {
			get => GetGommands();
			protected set => SetCommands(value);
		}

		public virtual IEnumerable<Command> GetGommands(bool create = false)
		{
			if (commands == null && create) {
				commands = new List<Command>();
			}
			return commands;
		}

		public virtual void SetCommands(IEnumerable<Command> commands)
		{
			this.commands = commands;
		}

		public Command GetCommand(string commandUid, bool ownOnly = false)
		{
			if (String.IsNullOrEmpty(commandUid))
				return null;

			var commands = GetGommands();
			if (commands != null) {
				foreach (var command in commands) {
					if (command.Uid == commandUid) {
						return command;
					}
				}
			}
			if (ownOnly || ChainOfCommand == null) {
				return null;
			} else {
				return ChainOfCommand.GetCommand(commandUid);
			}
		}

		public bool CanExecute(string commandUid, object parameter)
		{
			var command = GetCommand(commandUid);
			if (command != null) {
				return command.CanExecute(parameter);
			} else {
				return false;
			}
		}

		public void Execute(string commandUid, object parameter)
		{
			var command = GetCommand(commandUid);
			command?.Execute(parameter);
		}

		public void ChangeCanExecute()
		{
			foreach (var command in GetGommands(true)) {
				command.ChangeCanExecute();
			}

			ChainOfCommand?.ChangeCanExecute();
		}


		public static ICommandTarget ForObject(object obj)
		{
			var bindable = obj as BindableObject;
			var element = obj as Element;
			while (obj != null) {
				if (obj is ICommandTarget self) {
					return self;
				}

				if (bindable != null && bindable.BindingContext is ICommandTarget context) {
					return context;
				}

				if (element != null) {
					obj = bindable = element = element.Parent;
				} else {
					break;
				}
			}

			return null;
		}

		public static ICommandTarget Current()
		{
			var target = ForObject(Navigator.CurrentPage);
			if (target == null) {
				target = ForObject(Application.Current);
			}
			return target;
		}
	}

}
