using System;
using Dwares.Dwarf;


namespace Drive.Models
{
	public class Client : Person
	{
		//static ClassRef @class = new ClassRef(typeof(Client));

		public Client()
		{
			//Debug.EnableTracing(@class);
		}

		public bool Escort { get; set; }
	}
}
