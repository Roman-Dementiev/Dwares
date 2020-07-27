using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	public class Contact : CardModel, IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Contact()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name {
			get => name;
			set => SetProperty(ref name, value);
		}
		string name;

		public PhoneNumber Phone {
			get => phone;
			set => SetProperty(ref phone, value);
		}
		PhoneNumber phone;
	}

}
