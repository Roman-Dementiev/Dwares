using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf;
using Drive.Models;


namespace Drive.ViewModels
{
	public class ContactCardViewModel : CardViewModel<IContact>
    {
		//static ClassRef @class = new ClassRef(typeof(ContactCardViewModel));

		public ContactCardViewModel(IContact contact) :
			base(contact)
		{
			//Debug.EnableTracing(@class);

			UpdateFromSource();
		}

		public IContact Contact => Source;

		string name;
		public string Name {
			get => name;
			set => SetProperty(ref name, value);
		}

		string phoneNumber;
		public string PhoneNumber {
			get => phoneNumber;
			set => SetProperty(ref phoneNumber, value);
		}

		bool showPhoneNumber;
		public bool ShowPhoneNumber {
			get => showPhoneNumber;
			set => SetProperty(ref showPhoneNumber, value);
		}

		string address;
		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}

		bool showAddress;
		public bool ShowAddress {
			get => showAddress;
			set => SetProperty(ref showAddress, value);
		}

		string regularPlace;
		public string RegularPlace {
			get => regularPlace;
			set => SetProperty(ref regularPlace, value);
		}

		string regularAddress;
		public string RegularAddress {
			get => regularAddress;
			set => SetProperty(ref regularAddress, value);
		}

		bool showRegularPlace;
		public bool ShowRegularPlace {
			get => showRegularPlace;
			set => SetProperty(ref showRegularPlace, value);
		}


		protected override void UpdateFromSource()
		{
			Name = Contact.Title;
			PhoneNumber = Contact.PhoneNumber;
			ShowPhoneNumber = !string.IsNullOrEmpty(Contact.PhoneNumber);

			Address = Contact.Address;
			ShowAddress = ShowDetails && !string.IsNullOrEmpty(Address);

			if (Contact is Client client && client.RegularPlace != null) {
				RegularPlace = client.RegularPlace.Title;
				RegularAddress = client.RegularPlace.Address;
				ShowRegularPlace = ShowDetails;
			} else {
				RegularPlace = RegularAddress = string.Empty;
				ShowRegularPlace = false;
			}
		}

		protected override void OnShowDetailsChanged()
		{
			ShowAddress = ShowDetails && !string.IsNullOrEmpty(Contact.Address);
			ShowRegularPlace = ShowDetails && (Contact is Client client && client.RegularPlace != null);
		}


		public static ContactItemsCollection CreateCollection(Type contactType)
		{
			var collection = new ContactItemsCollection(contactType);
			return collection;
			//return new ShadowCollection<ContactItem, IContact>(
			//	AppScope.Instance.Contacts,
			//	(contact) => {
			//		if (contactType == null || contact.GetType() == contactType) {
			//			return new ContactItem(contact);
			//		} else {
			//			return null;
			//		}
			//	});
		}
	}

	public class ContactItemsCollection : ShadowCollection<ContactCardViewModel, IContact>
	{
		public ContactItemsCollection() { }

		public ContactItemsCollection(Type contactType)
		{
			Recollect(contactType);
		}

		public void Recollect(Type contactType)
		{
			SetSource(AppScope.Instance.Contacts, (contact) => {
				if (contactType == null || contact.GetType() == contactType) {
					return new ContactCardViewModel(contact);
				} else {
					return null;
				}
			});
		}
	}
}
