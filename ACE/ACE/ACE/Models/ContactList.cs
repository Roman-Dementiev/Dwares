using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ACE.Models
{
	public class ContactList : ObservableCollection<Contact>
	{
		public ContactList() { }

		public Contact GetContactByName(string name)
		{
			if (String.IsNullOrEmpty(name))
				return null;

			foreach (var contact in this) {
				if (contact.Name == name)
					return contact;
			}
			return null;
		}

		public Contact GetContactByPhone(string phone)
		{
			if (String.IsNullOrEmpty(phone))
				return null;

			foreach (var contact in this) {
				if (contact.Phone == phone)
					return contact;
			}
			return null;
		}

		public Contact GetContact(string name, string phone)
		{
			foreach (var contact in this) {
				if (!String.IsNullOrEmpty(phone) && contact.Phone == phone)
					return contact;
				if (!String.IsNullOrEmpty(name) && contact.Name == name)
					return contact;
			}
			return null;
		}

		public List<Contact> GetContacts(ContactType contactType)
		{
			var list = new List<Contact>();
			foreach (var contact in this) {
				if (contact.ContactType == contactType)
					list.Add(contact);
			}
			return list;
		}

		public List<Contact> GetClients() => GetContacts(ContactType.Client);
		public List<Contact> GetOffices() => GetContacts(ContactType.Office);
	}
}
