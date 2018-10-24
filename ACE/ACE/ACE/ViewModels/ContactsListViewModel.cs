using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using ACE.Models;
using ACE.Views;
using Dwares.Dwarf.Collections;
using Dwares.Druid.Support;


namespace ACE.ViewModels
{
	// TODO
	public class SortOrder
	{
		public SortOrder(string name)
		{
			Name = name;
		}

		public string Name { get; }
		public override string ToString() => Name;
	}

	public class ContactsListViewModel : CollectionViewModel<Contact>
	{
		public ContactsListViewModel(ContactsListPage page, ContactType contactType) :
			base(AppScope)
		{
			Page = page;
			ContactType = contactType;
			Callable = (contactType == ContactType.ACE);

			ResetContacts();

			AppData.Contacts.CollectionChanged += All_CollectionChanged;
		}

		ContactsListPage Page { get; }
		public ObservableCollection<Contact> Contacts => Items;
		public ContactType ContactType { get; }
		public bool Callable { get; set; }

		private void ResetContacts()
		{
			Contacts.Clear();

			foreach (var contact in AppData.Contacts) {
				if (contact.ContactType == ContactType) {
					Contacts.Add(contact);
				}
			}
		}

		private void All_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				foreach (var item in e.NewItems) {
					if (item is Contact contact && contact.ContactType == ContactType) {
						Contacts.Add(contact);
					}
				}
				break;

			case NotifyCollectionChangedAction.Remove:
				foreach (var item in e.OldItems) {
					if (item is Contact contact) {
						Contacts.Remove(contact);
					}
				}
				break;

			default:
				ResetContacts();
				break;
			}
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
			await AppData.RemoveContact(Selected);
		}

		public override void UpdateCommands()
		{
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "EditContact");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "DeleteContact");
		}
	}
}