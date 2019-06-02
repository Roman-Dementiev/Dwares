using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ContactItem : ListViewItem
    {
		//static ClassRef @class = new ClassRef(typeof(ContactItemViewModel));

		public ContactItem(IContact contact)
		{
			//Debug.EnableTracing(@class);

			Contact = contact ?? throw new ArgumentNullException(nameof(contact));

			PropertiesChangedOnSelected = new string[] {
				nameof(IsSelected),
				nameof(ItemFrameStyle),
				nameof(ShowDetails),
				nameof(ShowAddress),
				nameof(ShowRegularPlace)
			};
		}

		public IContact Contact { get; }

        public string Name => Contact.Title;
        
        public string PhoneNumber => Contact.PhoneNumber;
        public bool HasPhone => !string.IsNullOrEmpty(Contact.PhoneNumber);

        public string Address => Contact.Address;
        public bool ShowAddress => !string.IsNullOrEmpty(Contact.Address) && ShowDetails;

		public bool ShowRegularPlace => HasRegularPlace && ShowDetails;

		public bool HasRegularPlace {
			get {
				if (Contact is Client client) {
					return client.RegularPlace != null;
				} else {
					return false;
				}
			}
		}

		public string RegularPlace {
			get {
				if (Contact is Client client && client.RegularPlace != null) {
					return client.RegularPlace.Title;
				}
				return string.Empty;
			}
		}

		public string RegularAddress {
			get {
				if (Contact is Client client && client.RegularPlace != null) {
					return client.RegularPlace.Address;
				}
				return string.Empty;
			}
		}

		public static ObservableCollection<ContactItem> CreateCollection(Type contactType)
		{
			return new ShadowCollection<ContactItem, IContact>(
				AppScope.Instance.Contacts,
				(contact) => {
					if (contactType == null || contact.GetType() == contactType) {
						return new ContactItem(contact);
					} else {
						return null;
					}
				});
		}
	}
}
