using System;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Place : Model, IPlace
	{
		//static ClassRef @class = new ClassRef(typeof(Place));

		public Place()
		{
			//Debug.EnableTracing(@class);
		}

		public string CodeName {
			get => codeName;
			set => SetProperty(ref codeName, value);
		}
		string codeName;

		public string FullName {
			get => fullName;
			set => SetProperty(ref fullName, value);
		}
		string fullName;

		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address;
	}
}
