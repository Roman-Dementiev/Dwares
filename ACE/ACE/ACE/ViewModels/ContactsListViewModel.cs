using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using ACE.Models;
using ACE.Views;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid.Support;


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

		public bool CanDeleteContact() => AppData.CanDelete(Selected);

		public async void OnDeleteContact()
		{
			if (Selected == null)
				return;

			var message = String.Format("Do you want to delete contact for\n{0}?", Selected.Title);
			if (await Alerts.ConfirmAlert(message)) {
				await AppData.RemoveContact(Selected);
			}
		}

		public override void UpdateCommands()
		{
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "EditContact");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "DeleteContact");
		}
	}
}