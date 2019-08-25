using System;
using Dwares.Dwarf;


namespace Drive.Models
{
	public class Home : PlaceBase, IPlace
	{
		//static ClassRef @class = new ClassRef(typeof(Home));

		public Home(Person person, string address)
		{
			//Debug.EnableTracing(@class);
			Title = "Home";
			Person = person;
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
	}
}
