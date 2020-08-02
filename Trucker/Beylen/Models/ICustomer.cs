using System;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	interface ICustomer : IContact
	{
		string Address { get; }
		string ContactName { get; }

		PhoneNumber ContactPhone { get; }
	}
}
