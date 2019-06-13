using System;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;
using Dwares.Druid;


namespace Drive.Models
{
	public interface IContact : ITitleHolder, IModel
	{
		string Id { get; }

		//Tags Tags { get; }

		PhoneNumber PhoneNumber { get; set; }
		string Address { get; set; }
	}
}
