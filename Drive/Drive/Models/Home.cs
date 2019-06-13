using System;
using Dwares.Dwarf;


namespace Drive.Models
{
	public class Home : Place
	{
		//static ClassRef @class = new ClassRef(typeof(Home));

		public Home(string address)
		{
			//Debug.EnableTracing(@class);
			Title = "Home";
			Address = address;
		}

		Person person;
		public Person Person { 
			get => person;
			set => SetProperty(ref person, value);
		}

		public override string RouteTitle {
			get {
				if (Person != null) {
					return $"{Title} ({person.FullName})";
				} else {
					return Title;
				}
			}
		}

		public static Home ForAddress(string address)
		{
			if (string.IsNullOrEmpty(address))
				return null;

			return new Home(address);
		}
	}
}
