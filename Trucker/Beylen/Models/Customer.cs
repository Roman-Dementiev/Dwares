using System;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Customer : Contact, ICustomer
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Customer()
		{
			//Debug.EnableTracing(@class);
		}

		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address;

		public string ContactPerson {
			get => contactPerson;
			set => SetProperty(ref contactPerson, value);
		}
		string contactPerson;
	}
}
