using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


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
		static Settings settings = new Settings {
			CompanyContactsAreEditable = false
		};

		static ObservableCollection<Contact> contacts = null;
		public static ObservableCollection<Contact> Contacts => LazyInitializer.EnsureInitialized(ref contacts);

		static DateTime latestPickup = DateTime.Today;
		static ObservableCollection<Pickup> pickups = null;
		public static ObservableCollection<Pickup> Pickups => LazyInitializer.EnsureInitialized(ref pickups);

		public static async Task AddContact(Contact newContact) => await AddOrReplaceContact(newContact, null);
		public static async Task ReplaceContact(Contact newContact, Contact oldContact) => await AddOrReplaceContact(newContact, oldContact);

		static void AddContactInternal(ObservableCollection<Contact> contacts, Contact newContact, Contact oldContact = null)
		{
			contacts.AddOrReplace(newContact, oldContact);
			//Debug.Print("Contact Added: Type={0} Phone={1} [Count={2}]", newContact.ContactType, newContact.Phone, Contacts.Count);
		}

		static async Task AddOrReplaceContact(Contact newContact, Contact oldContact)
		{
			AddContactInternal(Contacts, newContact, oldContact);
			await SaveAsync();
		}

		public static async Task RemoveContact(Contact contact)
		{
			if (Contacts.Remove(contact)) {
				await SaveAsync();
			}
		}


		public static async Task AddPickup(Pickup newPickup) => await AddOrReplacePickup(newPickup, null);
		public static async Task ReplacePickup(Pickup newPickup, Pickup oldPickup) => await AddOrReplacePickup(newPickup, oldPickup);

		static void AddPickupInternal(ObservableCollection<Pickup> pickups, Pickup newPickup, Pickup oldPickup = null)
		{
			pickups.AddOrReplace(newPickup, oldPickup);
			//Debug.Print("Pickup {0}: {1}, Count={2}", oldPickup==null ? "added" : "replaced",  newPickup.ClientPhone, Contacts.Count);

			if (newPickup.PickupTime.IsAfter(latestPickup)) {
				latestPickup = newPickup.PickupTime;
			}
		}

		static async Task AddOrReplacePickup(Pickup newPickup, Pickup oldPickup)
		{
			AddPickupInternal(Pickups, newPickup, oldPickup);

			if (oldPickup == null) {
				var client = GetContactByPhone(newPickup.ClientPhone);
				if (client == null) {
					contacts.Add(newPickup.Client);
				} else {
					AddContactInfo(client, newPickup.Client);
				}

				var office = GetContactByPhone(newPickup.OfficePhone);
				if (office == null) {
					contacts.Add(newPickup.Office);
				} else {
					AddContactInfo(office, newPickup.Office);
				}
			}

			await SaveAsync();
		}

		static void AddContactInfo(Contact contact, Contact newContact)
		{
			if (newContact.ContactType != contact.ContactType) {
				// TODO
				return;
			}

			if (String.IsNullOrEmpty(contact.Name)) {
				contact.Name = newContact.Name;
			} else if (!String.IsNullOrEmpty(newContact.Name) && newContact.Name != contact.Name) {
				// TODO
				return;
			}

			if (String.IsNullOrEmpty(contact.Address)) {
				contact.Address = newContact.Address;
			} else if (!String.IsNullOrEmpty(newContact.Address) && newContact.Address != contact.Address) {
				// TODO
			}

			if (String.IsNullOrEmpty(contact.AltPhone)) {
				contact.AltPhone = newContact.AltPhone;
			}
			if (String.IsNullOrEmpty(contact.AltAddress)) {
				contact.AltPhone = newContact.AltAddress;
			}
		}

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
			foreach (var contact in Contacts) {
				if (contact.Phone == phone)
					return contact;
			}
			return null;
		}

		public static bool IsEditable(Contact contact)
		{
			if (contact.ContactType == ContactType.Company)
				return settings.CompanyContactsAreEditable;

			var pickups = Pickups;
			foreach (var pickup in Pickups) {
				if (pickup.Client == contact || pickup.Office == contact)
					return false;
			}
			return true;
		}
	}
}
