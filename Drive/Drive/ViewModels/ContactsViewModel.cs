using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Drive.ViewModels
{
	public class ContactsViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsViewModel1cs));

		public ContactsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Contacts";
		}

		public Color ActiveIconTextColor {
			get => AppScope.ActiveIconTextColor;
		}
	}
}
