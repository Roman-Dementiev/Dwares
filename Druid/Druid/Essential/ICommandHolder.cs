using System;
using System.Windows.Input;


namespace Dwares.Druid.Essential
{
	public interface ICommandHolder
	{
		ICommand Command { get; set; }
	}
}
