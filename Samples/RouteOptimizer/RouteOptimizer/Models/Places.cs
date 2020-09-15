using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dwares.Dwarf;


namespace RouteOptimizer.Models
{
	public class Places : ObservableCollection<Place>
	{
		//static ClassRef @class = new ClassRef(typeof(Places));

		public Places()
		{
			//Debug.EnableTracing(@class);
		}

		public Place GetById(string id)
		{
			return this.FirstOrDefault((place) => place.Id == id);
		}

		public Place GetByName(string name)
		{
			return this.FirstOrDefault((place) => place.Name == name);
		}
	}
}
