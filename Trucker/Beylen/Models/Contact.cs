using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid;


namespace Beylen.Models
{
	public class Contact : Model, IContact
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

		public string Info {
			get => info;
			set => SetProperty(ref info, value);
		}
		string info;
	}
}
