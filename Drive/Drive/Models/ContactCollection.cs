using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Drive.Models
{
	public class ContactCollection : ObservableCollection<IContact>
	{
		public ContactCollection() { }

		public IContact GetByName(string name)
			=> Lookup((contact) => contact.Title == name);

		public IContact GetByPhone(string phone)
			=> Lookup((contact) => contact.PhoneNumber == phone);

		public IContact GetByAddress(string address)
			=> Lookup((contact) => contact.Address == address);

		public IContact Lookup(Func<IContact, bool> test)
		{
			foreach (var contact in this) {
				if (test(contact))
					return contact;
			}
			return null;
		}
	}
}
