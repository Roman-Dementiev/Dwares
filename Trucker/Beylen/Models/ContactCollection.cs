using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Beylen.Models
{
	public class ContactCollection<T> : OrderedCollection<T> where T : IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Contacts));

		public ContactCollection() : base(CompareByName)
		{
			//Debug.EnableTracing(@class);
		}

		public T GetByName(string name)
			=> Lookup((contact) => contact.Name == name);

		public T GetByPhone(string phone)
			=> Lookup((contact) => contact.Phone == phone);

		public T Lookup(Func<IContact, bool> test)
		{
			foreach (var contact in this) {
				if (test(contact))
					return contact;
			}
			return default;
		}

		public static int CompareByName(T c1, T c2)
		{
			string name1 = c1.Name?.ToLower() ?? string.Empty;
			string name2 = c2.Name?.ToLower() ?? string.Empty;

			return name1.CompareTo(name2);
		}
	}
}
