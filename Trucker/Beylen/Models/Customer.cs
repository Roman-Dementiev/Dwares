using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


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

		public string ContactName {
			get => contactPerson;
			set => SetProperty(ref contactPerson, value);
		}
		string contactPerson;

		public PhoneNumber ContactPhone {
			get => contactPhone;
			set => SetProperty(ref contactPhone, value);
		}
		PhoneNumber contactPhone;
	}
}
