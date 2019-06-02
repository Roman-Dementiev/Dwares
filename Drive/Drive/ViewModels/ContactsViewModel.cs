using System;
using Xamarin.Forms;
using Dwares.Dwarf.Collections;
using Dwares.Druid;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ContactsViewModel : CollectionViewModel<ContactItem>
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsViewModel1cs));

		public ContactsViewModel() : this(typeof(Client)) { }

		public ContactsViewModel(Type contactType) :
			base(ApplicationScope, ContactItem.CreateCollection(contactType))
		{
			//Debug.EnableTracing(@class);

			Title = "Contacts";
		}

	}
}
