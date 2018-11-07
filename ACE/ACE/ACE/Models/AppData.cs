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

		static ScheduleTime latestPickup = new ScheduleTime();
		static ObservableCollection<Pickup> pickups = null;
		public static ObservableCollection<Pickup> Pickups => LazyInitializer.EnsureInitialized(ref pickups);

		static Route route = null;
		public static Route Route => LazyInitializer.EnsureInitialized(ref route, () => new Route(ACE));

		static Contact _ACE = null;
		public static Contact ACE {
			get {
				if (_ACE == null) {
					//TODO
					_ACE = new Contact {
						ContactType = ContactType.ACE,
						Name = "ACE",
						Address = "10162 Bustleton Ave\nPhiladelphia PA 19116"
					};
				}
				return _ACE;
			}
		}

		public static async Task NewContact(Contact newContact, bool save)
		{
			AddContact(Contacts, newContact);

			if (save) {
				await SaveAsync();
			}
		}

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
			AddRouteRun(Route, newPickup);

			if (save) {
				await SaveAsync();
			}
		}

		static void AddPickup(ObservableCollection<Pickup> pickups, Pickup newPickup, Pickup oldPickup = null)
		{
			Debug.Trace(@class, nameof(AddPickup), "{0}", newPickup);
			pickups.AddOrReplace(newPickup, oldPickup);

			if (!latestPickup.IsSet || newPickup.PickupTime.IsAfter(latestPickup.DateTime)) {
				latestPickup = newPickup.PickupTime;
			}
		}

		public static async Task ClearSchedule(bool save = true)
		{
			route?.Clear();
			pickups?.Clear();
			latestPickup.Unset();

			if (save) {
				await SaveAsync();
			}
		}

		public static async Task NewSchedule(DateTime startDate, TimeSpan startTime)
		{
			await ClearSchedule(false);


		}

		static void AddRouteRun(Route route, Run run)
		{
			route.AddRun(run);
		}


		public static async Task RemovePickup(Pickup pickup)
		{
			if (Pickups.Remove(pickup)) {
				await SaveAsync();
			}
		}

		public static bool EstimateNextPickup(out ScheduleTime pickupTime, out ScheduleTime appoitmentTime)
		{
			if (!latestPickup.IsSet) {
				pickupTime = new ScheduleTime(DateTime.Now.Hour, 30);
			} else if (latestPickup.Minute < 30) {
				pickupTime = new ScheduleTime(latestPickup.Hour, 30);
			} else if (latestPickup.Hour < 23) {
				pickupTime = new ScheduleTime(latestPickup.Hour + 1, 0);
			} else {
				pickupTime = appoitmentTime = ScheduleTime.Tomorrow;
				return false;
			}

			appoitmentTime = new ScheduleTime(pickupTime, new TimeSpan(hours: 1, minutes: 0, seconds: 0));
			return true;
		}

		public static Contact GetContactByName(string name)
		{
			if (String.IsNullOrEmpty(name))
				return null;

			foreach (var contact in Contacts) {
				if (contact.Name == name)
					return contact;
			}
			return null;
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
