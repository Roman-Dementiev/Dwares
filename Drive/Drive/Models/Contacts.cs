using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Drive.Models
{
	public class Contacts : OrderedCollection<IContact>
	{
		//static ClassRef @class = new ClassRef(typeof(Contacts));

		public Contacts() :
			base(CompareByName)
		{
			//Debug.EnableTracing(@class);
		}

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

		public static int CompareByName(IContact c1, IContact c2)
		{
			string name1 = c1.Title?.ToLower() ?? string.Empty;
			string name2 = c2.Title?.ToLower() ?? string.Empty;

			return name1.CompareTo(name2);
		}
	}
}
