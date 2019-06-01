using System;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;


namespace Drive.Models
{
	public interface IContact : ITitleHolder
	{
		string Id { get; }
		PhoneNumber PhoneNumber { get; set; }
		string Address { get; set; }
	}
}
