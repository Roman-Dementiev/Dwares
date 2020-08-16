using System;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	interface ICustomer
	{
		string CodeName { get; }
		string FullName { get; }

		PhoneNumber Phone { get; }
		string Address { get; }
		string ContactName { get; }

		PhoneNumber ContactPhone { get; }
	}
}
