using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Car
	{
		//static ClassRef @class = new ClassRef(typeof(Cars));

		public Car()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id { get; set; }
		public string Name { get; set; }

		public static List<Car> List { get; } = new List<Car> {
			new Car { Id = "A", Name = "Car A" },
			new Car { Id = "B", Name = "Car B" }
		};
		public static Car ById(string id)
		{
			foreach (var car in List) {
				if (car.Id == id)
					return car;
			}
			return null;
		}

		public static Car ByName(string name)
		{
			foreach (var car in List) {
				if (car.Name == name)
					return car;
			}
			return null;
		}
	}

}
