using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Places : ObservableCollection<Place>
	{
		//static ClassRef @class = new ClassRef(typeof(Places));

		public Places()
		{
			//Debug.EnableTracing(@class);
		}

		public Place GetByCodeName(string codeName)
		{
			if (string.IsNullOrEmpty(codeName))
				return null;

			foreach (var place in this) {
				if (place.CodeName == codeName)
					return place;
			}
			return null;
		}
	}
}
