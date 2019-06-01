using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Drive.Models
{
	public class Contacts : ObservableCollection<IContact>
	{
		public Contacts() { }

		public IContact GetById(string id, Type type = null)
			=> Lookup((contact) => contact.Id == id && (type == null || contact.GetType() == type));

		public T GetById<T>(string id, Type type = null) where T : IContact
			=> (T)GetById(id, typeof(T));

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
