using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf.Collections;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ContactItem
	{
		//static ClassRef @class = new ClassRef(typeof(ContactItemViewModel));

		public ContactItem(IContact contact)
		{
			//Debug.EnableTracing(@class);

			Contact = contact ?? throw new ArgumentNullException(nameof(contact));
		}

		public IContact Contact { get; }


		public static ObservableCollection<ContactItem> CreateCollection(Type contactType)
		{
			return new ShadowCollection<ContactItem, IContact>(
				AppScope.Instance.Contacts,
				(contact) => {
					if (contactType == null || contact.GetType() == contactType) {
						return new ContactItem(contact);
					} else {
						return null;
					}
				});
		}
	}
}
