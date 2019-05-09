using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Drive.Models
{
	public class ContactCollection<TContact> : ObservableCollection<TContact> where TContact : Contact
	{
		public ContactCollection() { }

		public TContact GetByName(string name)
			=> Lookup((contact) => contact.Title == name);

		public TContact GetByPhone(string phone)
			=> Lookup((contact) => contact.PhoneNumber == phone);

		public TContact GetByAddress(string address)
			=> Lookup((contact) => contact.Address == address);

		public TContact Lookup(Func<TContact, bool> test)
		{
			foreach (var contact in this) {
				if (test(contact))
					return contact;
			}
			return null;
		}
	}
}
