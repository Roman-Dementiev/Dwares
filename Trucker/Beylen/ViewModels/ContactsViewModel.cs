using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;
using Beylen.ViewModels;


namespace Beylen.ViewModels
{
	public class ContactsViewModel : CollectionViewModel<ContactCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsViewModel));

		public ContactsViewModel() :
			base(ApplicationScope, ContactCardModel.CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Contacts";
		}
	}
}
