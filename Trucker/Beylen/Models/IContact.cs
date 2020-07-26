using System;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	public interface IContact
	{
		string Name { get; }
		PhoneNumber Phone { get; }
	}
}
