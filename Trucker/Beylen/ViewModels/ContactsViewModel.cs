using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;


namespace Beylen.ViewModels
{
	public class ContactsViewModel : CollectionViewModel<Contact>
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsViewModel));

		public ContactsViewModel() :
			base(ApplicationScope, AppScope.Instance.Contacts)
		{
			//Debug.EnableTracing(@class);

			Title = "Contacts";
		}
	}
}
