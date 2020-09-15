using System;
using Dwares.Dwarf;
using Dwares.Druid;


namespace RouteOptimizer.Models
{
	public class Place : Model
	{
		//static ClassRef @class = new ClassRef(typeof(DwarfClass1));

		public Place()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id => Ids.PlaceId(this);

		public string Name {
			get => name;
			set => SetPropertyEx(ref name, value, nameof(Name), nameof(Id));
		}
		string name = string.Empty;

		public string Tags {
			get => tags;
			set => SetProperty(ref tags, value);
		}
		string tags = string.Empty;

		public string Address {
			get => address;
			set => SetPropertyEx(ref address, value, nameof(Address), nameof(Id));
		}
		string address = string.Empty;

	}
}
