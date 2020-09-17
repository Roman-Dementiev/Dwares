using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;

namespace RouteOptimizer.Models
{
	public class Places
	{
		//static ClassRef @class = new ClassRef(typeof(Places));

		public Places()
		{
			//Debug.EnableTracing(@class);
		}

		public ObservableCollectionEx<Place> List { get; } = new ObservableCollectionEx<Place>();
		public Dictionary<string, Place> Index { get;} = new Dictionary<string, Place>();

		public void Add(Place place)
		{
//#if DEBUG
			Debug.AssertNotEmpty(place?.Id);
			if (Index.ContainsKey(place.Id)) {
				Debug.Fail($"Place with Id='{place.Id}' already exists");
				return;
			}
//#endif
			Index[place.Id] = place;
			List.Add(place);
		}

		public bool Remove(Place place)
		{
			Debug.AssertNotEmpty(place?.Id);

			if (Index.Remove(place.Id)) {
				List.Remove(place);
				return true;
			} else {
				//Debug.Fail($"Place with Id='{place.Id}' not found");
				return false;
			}
		}

		public void Replace(string oldId, Place newPlace)
		{
			Debug.AssertNotEmpty(newPlace?.Id);

			if (Index.ContainsKey(oldId))
			{
				var oldPlace = Index[oldId];
				if (newPlace.Id != oldId) {
					Index.Remove(oldId);
				}
				Index[newPlace.Id] = newPlace;

				if (newPlace != oldPlace) {
					List.Remove(oldPlace);
					List.Add(newPlace);
				}
			} else {
				Debug.Fail($"Place with Id='{oldId}' not found");
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
				Debug.Fail($"Places.A():  index={index} is out of range");
				return null;
			}
		}

		public Place GetById(string id)
		{
			if (!string.IsNullOrEmpty(id) && Index.ContainsKey(id)) {
				return Index[id];
			} else {
				return null;
			}
		}
		public Place GetByName(string name)
		{
			var id = Ids.PlaceId(name, null);
			return GetById(id);
		}
	}
}
