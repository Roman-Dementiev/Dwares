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

			if (Index.Remove(place.Name)) {
				List.Remove(place);
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

		public int IndexOf(Place place) => List.IndexOf(place);

		public Place At(int index)
		{
			if (index >= 0 && index < List.Count) {
				return List[index];
			} else {
				return null;
			}
		}

		//public Place GetById(string id)
		//{
		//	return List.FirstOrDefault((place) => place.Id == id);
		//}

		public Place GetByName(string name)
		{
			//return List.FirstOrDefault((place) => place.Name == name);
			if (Index.ContainsKey(name))
				return Index[name];
			else
				return null;
		}
	}
}
