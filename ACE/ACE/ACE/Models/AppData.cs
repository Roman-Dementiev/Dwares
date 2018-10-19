using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;

namespace ACE.Models
{
	public static partial class Extensions
	{
		public static void AddOrReplace<T>(this ObservableCollection<T> collection, T newItem, T oldItem)
		{
			int index = collection.IndexOf(oldItem);
			if (index >= 0) {
				collection.RemoveAt(index);
				collection.Insert(index, newItem);
			} else {
				collection.Add(newItem);
			}
		}

		public static T[] ToArray<T>(this ObservableCollection<T> collection)
		{
			var length = collection.Count;
			var array = new T[length];
			for (int i = 0; i < length; i++) {
				array[i] = collection[i];
			}
			return array;
		}
	};

	public static partial class AppData
	{
		static ClassRef @class = new ClassRef(typeof(AppData));

		static Settings settings = new Settings {
			CanDeleteCompanyContacts = false
		};

		static ObservableCollection<Contact> contacts = null;
		public static ObservableCollection<Contact> Contacts => LazyInitializer.EnsureInitialized(ref contacts);

		static DateTime latestPickup = DateTime.Now;
		static ObservableCollection<Pickup> pickups = null;
		public static ObservableCollection<Pickup> Pickups => LazyInitializer.EnsureInitialized(ref pickups);

		private static void Clear()
		{
			contacts?.Clear();
			pickups?.Clear();
			latestPickup = DateTime.Now;
		}

		public static async Task NewContact(Contact newContact)
		{
			AddContact(Contacts, newContact);
			await SaveAsync();
		}

		//public static async Task ReplaceContact(Contact newContact, Contact oldContact)
		//{
		//	AddContact(Contacts, newContact, oldContact);

		//	if (oldContact != null) {
		//		var pickups = Pickups;
		//		foreach (var pickup in pickups) {
		//			if (pickup.Client == oldContact) {
		//				pickup.Client = newContact;
		//			}
		//			if (pickup.Office == oldContact) {
		//				pickup.Office = newContact;
		//			}
		//		}
		//	}

		//	await SaveAsync();
		//}

		static void AddContact(ObservableCollection<Contact> contacts, Contact newContact, Contact oldContact = null)
		{
			Debug.Trace(@class, nameof(AddContact), "{0}", newContact);
			contacts.AddOrReplace(newContact, oldContact);
		}

		public static async Task RemoveContact(Contact contact)
		{
			if (Contacts.Remove(contact)) {
				await SaveAsync();
			}
		}


		public static async Task NewPickup(Pickup newPickup, bool save)
		{
			AddPickup(Pickups, newPickup);

			if (save) {
				await SaveAsync();
			}
		}

		//public static async Task ReplacePickup(Pickup newPickup, Pickup oldPickup)
		//{
		//	AddPickup(Pickups, newPickup, oldPickup);

		//	if (oldPickup == null) {
		//		var client = GetContactByPhone(newPickup.ClientPhone);
		//		if (client == null) {
		//			contacts.Add(newPickup.Client);
		//		} else {
		//			AddContactInfo(client, newPickup.Client);
		//		}

		//		var office = GetContactByPhone(newPickup.OfficePhone);
		//		if (office == null) {
		//			contacts.Add(newPickup.Office);
		//		} else {
		//			AddContactInfo(office, newPickup.Office);
		//		}
		//	}

		//	await SaveAsync();

		//}

		static void AddPickup(ObservableCollection<Pickup> pickups, Pickup newPickup, Pickup oldPickup = null)
		{
			Debug.Trace(@class, nameof(AddPickup), "{0}", newPickup);
			pickups.AddOrReplace(newPickup, oldPickup);
			//Debug.Print("Pickup {0}: {1}, Count={2}", oldPickup==null ? "added" : "replaced",  newPickup.ClientPhone, Contacts.Count);

			if (newPickup.PickupTime.IsAfter(latestPickup)) {
				latestPickup = newPickup.PickupTime;
			}
		}

		//static void AddContactInfo(Contact contact, Contact newContact)
		//{
		//	if (newContact.ContactType != contact.ContactType) {
		//		// TODO
		//		return;
		//	}

		//	if (String.IsNullOrEmpty(contact.Name)) {
		//		contact.Name = newContact.Name;
		//	} else if (!String.IsNullOrEmpty(newContact.Name) && newContact.Name != contact.Name) {
		//		// TODO
		//		return;
		//	}

		//	if (String.IsNullOrEmpty(contact.Address)) {
		//		contact.Address = newContact.Address;
		//	} else if (!String.IsNullOrEmpty(newContact.Address) && newContact.Address != contact.Address) {
		//		// TODO
		//	}

		//	if (String.IsNullOrEmpty(contact.AltPhone)) {
		//		contact.AltPhone = newContact.AltPhone;
		//	}
		//	if (String.IsNullOrEmpty(contact.AltAddress)) {
		//		contact.AltPhone = newContact.AltAddress;
		//	}
		//}

		public static async Task RemovePickup(Pickup pickup)
		{
			if (Pickups.Remove(pickup)) {
				await SaveAsync();
			}
		}

		public static ScheduleTime ApproximateNextPickup()
		{
			if (latestPickup.Minute < 30) {
				return new ScheduleTime(latestPickup.Hour, 30);
			} else if (latestPickup.Hour < 23) {
				return new ScheduleTime(latestPickup.Hour + 1, 0);
			} else {
				// TODO
				return DateTime.Today;
			}
		}

		public static Contact GetContactByPhone(string phone)
		{
			if (String.IsNullOrEmpty(phone))
				return null;

			foreach (var contact in Contacts) {
				if (contact.Phone == phone)
					return contact;
			}
			return null;
		}

		public static bool CanDelete(Contact contact)
		{
			if (contact == null)
				return false;

			if (contact.ContactType == ContactType.ACE)
				return settings.CanDeleteCompanyContacts;

			return !IsEngaged(contact);
		}

		delegate bool ContactIsEngaged(Pickup pickup);
		static bool IsEngaged(Contact contact, ContactIsEngaged isEngaged)
		{
			if (contact.ContactType != ContactType.Client && contact.ContactType != ContactType.Office)
				return false;

			var pickups = Pickups;
			foreach (var pickup in pickups) {
				if (isEngaged(pickup))
					return true;
			}
			return false;
		}

		public static bool IsEngaged(Contact contact)
		{
			return IsEngaged(contact, (pickup) => pickup.Client == contact || pickup.Office == contact);
		}

		public static bool HasPickup(Contact contact)
		{
			return IsEngaged(contact, (pickup) => pickup.Client == contact);
		}

		public static bool HasAppoitment(Contact contact)
		{
			return IsEngaged(contact, (pickup) => pickup.Office == contact);
		}

		public static List<Contact> GetContacts(ContactType contactType)
		{
			var contacts = Contacts;
			var list = new List<Contact>();
			foreach (var contact in contacts) {
				if (contact.ContactType == contactType)
					list.Add(contact);
			}
			return list;
		}

		public static List<Contact> GetClients() => GetContacts(ContactType.Client);
		public static List<Contact> GetOffices() => GetContacts(ContactType.Office);
	}
}
