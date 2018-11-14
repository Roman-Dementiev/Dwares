using System;
using System.Windows.Input;


namespace Dwares.Druid.Satchel
{
	public interface ICommandHolder
	{
		ICommand Command { get; set; }
	}
}
