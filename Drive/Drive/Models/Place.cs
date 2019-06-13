using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;


namespace Drive.Models
{
	public class Place : Model, ITitleHolder, IPlace, IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Place));

		public Place()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id { get; set; }

		//string fullTitle;
		//public string FullTitle {
		//	get => fullTitle;
		//	set => SetProperty(ref fullTitle, value);
		//}

		string title;
		public string Title {
			get => title;
			set => SetPropertyEx(ref title, value, nameof(Title), nameof(RouteTitle));
		}

		public virtual string RouteTitle {
			get => Title;
		}

		PhoneNumber phoneNumber;
		public PhoneNumber PhoneNumber {
			get => phoneNumber;
			set => SetProperty(ref phoneNumber, value);
		}

		string address;
		public string Address {
			get => address; 
			set => SetProperty(ref address, value);
		}
	}
}
