using System;
using System.Collections.Generic;
using System.Text;

namespace Beylen.Models
{
	interface ICustomer : IContact
	{
		string Address { get; }
		string ContactPerson { get; }
	}
}
