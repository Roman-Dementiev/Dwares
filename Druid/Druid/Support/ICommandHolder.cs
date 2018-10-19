using System;
using System.Windows.Input;


namespace Dwares.Druid.Support
{
	public interface ICommandHolder
	{
		ICommand Command { get; set; }
	}
}
