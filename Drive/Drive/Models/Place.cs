using System;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;


namespace Drive.Models
{
	public class Place : TitleHolder, IPlace, IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Place));

		public Place()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id { get; set; }

		string fullTitle;
		public string FullTitle {
			get => fullTitle;
			set => SetProperty(ref fullTitle, value);
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


	public class Home : Place
	{
		public Home(string address)
		{
			Title = "Home";
			Address = address;
		}

		public static Home ForAddress(string address)
		{
			if (string.IsNullOrEmpty(address))
				return null;
			
			return new Home(address);
		}
	}
}
