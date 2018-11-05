using System;
using Dwares.Dwarf;
using Dwares.Druid.Essential;


namespace Dwares.Druid.Support
{
	internal class WritMixin
	{
		public WritMixin(ICommandHolder owner)
		{
			Owner = owner ?? throw new ArgumentNullException(nameof(owner));
		}

		/*public*/ ICommandHolder Owner { get; }

		//public ICommand Command {
		//	get => Owner.Command;
		//	set => Owner.Command = value;
		//}

		public WritCommand WritCommand {
			get => Owner.Command as WritCommand;
			set => Owner.Command = value;
		}

		public string Writ {
			get => WritCommand?.Writ;
			set {
				if (Owner.Command == null) {
					Owner.Command = new WritCommand(value);
				} else if (Owner.Command is WritCommand writCommand) {
					writCommand.Writ = value;
				} else {
					Debug.Print("Can not set Writ={0} to {1}", value, Owner.Command);
				}
			}
		}
	}

}
