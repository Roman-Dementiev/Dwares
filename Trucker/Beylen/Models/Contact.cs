using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;

namespace Beylen.Models
{
	public class Contact : IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Contact()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name { get; set; }
		public PhoneNumber Phone { get; set; }
	}

	//public class Person : Contact
	//{
	//	//static ClassRef @class = new ClassRef(typeof(Person));

	//	public Person()
	//	{
	//		//Debug.EnableTracing(@class);
	//	}
	//}

	public class Customer : Contact, ICustomer
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Customer()
		{
			//Debug.EnableTracing(@class);
		}

		public string Address { get; set; }
		public string ContactPerson { get; set; }
	}
}
