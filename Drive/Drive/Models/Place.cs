using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;


namespace Drive.Models
{
	public class PlaceBase : Model, ITitleHolder, IPlace
	{
		string title;
		public string Title {
			get => title;
			set => SetPropertyEx(ref title, value, nameof(Title), nameof(RouteTitle));
		}

		string address;
		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}

		public virtual string RouteTitle {
			get => Title;
		}
	}

	public class Place : PlaceBase, IContact 
	{
		//static ClassRef @class = new ClassRef(typeof(Place));

		public Place()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id { get; set; }


		PhoneNumber phoneNumber;
		public PhoneNumber PhoneNumber {
			get => phoneNumber;
			set => SetProperty(ref phoneNumber, value);
		}
	}
}
