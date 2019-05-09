using System;
using Dwares.Dwarf;


namespace Drive.Models
{
	public class Person : Contact
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Person()
		{
			//Debug.EnableTracing(@class);
		}

		public string FullName {
			get => Title;
			set {
				if (value != Title) {
					Title = value;
					FirePropertyChanged();
				}
			}
		}
	}
}
