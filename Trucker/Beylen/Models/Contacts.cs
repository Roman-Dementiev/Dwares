using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Beylen.Models
{
	public class Contacts<T> : OrderedCollection<T> where T : IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Contacts));

		public Contacts() : base(CompareByName)
		{
			//Debug.EnableTracing(@class);
		}

		public IContact GetByName(string name)
			=> Lookup((contact) => contact.Name == name);

		public IContact GetByPhone(string phone)
			=> Lookup((contact) => contact.Phone == phone);

		public IContact Lookup(Func<IContact, bool> test)
		{
			foreach (var contact in this) {
				if (test(contact))
					return contact;
			}
			return null;
		}

		public static int CompareByName(T c1, T c2)
		{
			string name1 = c1.Name?.ToLower() ?? string.Empty;
			string name2 = c2.Name?.ToLower() ?? string.Empty;

			return name1.CompareTo(name2);
		}
	}
}
