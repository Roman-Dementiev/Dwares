using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	public class Customer : Place, IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Customer()
		{
			//Debug.EnableTracing(@class);
		}

		string IContact.Name => CodeName;
		string IContact.Info => string.Empty;

		public PhoneNumber Phone {
			get => phone;
			set => SetProperty(ref phone, value);
		}
		PhoneNumber phone;

		//public string ContactName {
		//	get => contactPerson;
		//	set => SetProperty(ref contactPerson, value);
		//}
		//string contactPerson;

		//public PhoneNumber ContactPhone {
		//	get => contactPhone;
		//	set => SetProperty(ref contactPhone, value);
		//}
		//PhoneNumber contactPhone;

		public Contact Contact { 
			get => contact;
			set => SetProperty(ref contact, value);
		}
		Contact contact;
	}
}
