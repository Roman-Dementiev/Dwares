using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Beylen.Models
{
	public class Packing
	{
		//static ClassRef @class = new ClassRef(typeof(Unit));

		public Packing(string name, string plural = null)
		{
			//Debug.EnableTracing(@class);
			Name = name ?? throw new ArgumentNullException(nameof(name));
			if (string.IsNullOrEmpty(plural)) {
				Plural = name + 's';
			} else {
				Plural = plural;
			}
		}

		public string Name { get; }
		public string Plural { get; }


		public static List<Packing> List { get; } = new List<Packing> {
			new Packing("box", "boxes"),
			new Packing("bag"),
			new Packing("carton"),
			new Packing("barrel")
		};

		public static Packing Get(string name)
		{
			return List.Lookup((p) => p.Name == name);
		}
}
}
