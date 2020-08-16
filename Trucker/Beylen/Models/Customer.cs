using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	public class Customer : Place, ICustomer, IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Customer()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name => CodeName;

		//public string CodeName {
		//	get => codeName;
		//	set => SetProperty(ref codeName, value);
		//}
		//string codeName;

		//public string FullName {
		//	get => fullName;
		//	set => SetProperty(ref fullName, value);
		//}
		//string fullName;

		//public string Address {
		//	get => address;
		//	set => SetProperty(ref address, value);
		//}
		//string address;

		public PhoneNumber Phone {
			get => phone;
			set => SetProperty(ref phone, value);
		}
		PhoneNumber phone;

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
