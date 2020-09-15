using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dwares.Dwarf;


namespace RouteOptimizer.Models
{
	public class Places
	{
		//static ClassRef @class = new ClassRef(typeof(Places));

		public Places()
		{
			//Debug.EnableTracing(@class);
		}

		public ObservableCollection<Place> List { get; } = new ObservableCollection<Place>();
		public Dictionary<string, Place> Index { get;} = new Dictionary<string, Place>();

		public void Add(Place place)
		{
			Debug.Assert(place != null);
			Debug.Assert(!Index.ContainsKey(place.Name));

			List.Add(place);
			Index[place.Name] = place;
		}

		public bool Remove(Place place)
		{
			Debug.Assert(place != null);

			if (List.Remove(place)) {
				Index.Remove(place.Name);
				return true;
			} else {
				return false;
			}
		}

		public void Clear()
		{
			List.Clear();
			Index.Clear();
		}

		//public Place GetById(string id)
		//{
		//	return List.FirstOrDefault((place) => place.Id == id);
		//}

		public Place GetByName(string name)
		{
			return List.FirstOrDefault((place) => place.Name == name);
		}
	}
}
