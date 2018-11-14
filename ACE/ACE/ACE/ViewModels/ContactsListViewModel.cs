using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using ACE.Models;
using ACE.Views;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Dwarf.Collections;


namespace ACE.ViewModels
{
	public class ContactsListViewModel : CollectionViewModel<Contact>
	{
		public ContactsListViewModel(ContactsListPage page, ContactType contactType, bool switchable = false) :
			base(AppScope, new SortedCollection<Contact>(AppData.Contacts, 
				(c1, c2) => String.Compare(c1.Name, c2.Name),
				(contact) => contact.ContactType==contactType))
		{
			Page = page;
			ContactType = contactType;
			Switchable = switchable;
			Callable = (contactType == ContactType.ACE);
			Contacts = Items as SortedCollection<Contact>;

			if (contactType == ContactType.Client) {
				SortOrders = new ContactSortOrder[] {
					new SortByFirstName(),
					new SortByLastName(),
					new SortByPhone()
				};
			} else {
				SortOrders = new ContactSortOrder[] {
					new SortByName(),
					new SortByPhone()
				};
			}
			// TODO?: Preferences
			SelectedSortOrder = SortOrders[0];
		}

		ContactsListPage Page { get; }
		public SortedCollection<Contact> Contacts { get; }
		public ContactType ContactType { get; }
		public bool Callable { get; set; }
		public bool Switchable { get; set; }

		public ContactSortOrder[] SortOrders { get; }

		ContactSortOrder selectedSortOrder;
		public ContactSortOrder SelectedSortOrder {
			get => selectedSortOrder;
			set {
				if (value != selectedSortOrder) {
					value.Descending = Contacts.Descending;
					selectedSortOrder = value;
					Contacts.Comparer = selectedSortOrder;
				}
			}
		}

		public bool Descending {
			get => Contacts.Descending;
			set => Contacts.Descending = value;
		}

		public void OnShowSortPanel()
		{
			Page.ShowSortPanel();
		}

		public void OnShowFindPanel()
		{
			Page.ShowFindPanel();
		}

		public void OnClosePanel()
		{
			Page.HidePanel();
		}

		public virtual async void OnAddContact()
		{
			var page = new ContactDetailPage(ContactType);
			await Navigator.PushModal(page);
		}

		public bool CanEditContact() => HasSelected();

		public virtual async void OnEditContact()
		{
			if (Selected != null) {
				var page = new ContactDetailPage(Selected);
				await Navigator.PushModal(page);
			}
		}

		public bool CanDeleteContact()
		{
			var contact = Selected;
			if (contact == null)
				return false;

			if (contact.ContactType == ContactType.ACE)
				return Settings.CanDeleteCompanyContacts;

			return !AppData.Schedule.IsEngaged(contact);
		}

		public async void OnDeleteContact()
		{
			var contact = Selected;
			if (contact == null)
				return;

			var message = String.Format("Do you want to delete contact for\n{0}?", contact.Title);
			if (await Alerts.ConfirmAlert(message)) {
				if (AppData.Contacts.Remove(contact)) {
					await AppStorage.SaveAsync();
				}
			}
		}

		public override void UpdateCommands()
		{
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "EditContact");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "DeleteContact");
		}
	}
}