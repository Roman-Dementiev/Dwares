using System;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid;


namespace Beylen.Models
{
	public interface IContact : IModel
	{
		string Name { get; }
		PhoneNumber Phone { get; }
	}
}
