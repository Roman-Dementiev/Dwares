using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using ACE.Models;
using ACE.Views;
using Dwares.Druid.Support;


namespace ACE.ViewModels
{
	public class ContactsListViewModel : CollectionViewModel<Contact>
	{
		public ObservableCollection<Contact> Contacts => Items;
		public ContactType ContactType { get;  }
		public bool Callable { get; set; }
		public Command AddCommand { get; }
		public Command EditCommand { get; }
		public Command DeleteCommand { get; }

		public ContactsListViewModel(INavigation navigation, ContactType contactType) :
			base(navigation)
		{
			ContactType = contactType;
			Callable = contactType == ContactType.Company;
			AddCommand = new Command(OnAdd);
			EditCommand = new Command(OnEdit, HasSelected);
			DeleteCommand = new Command(OnDelete, CanDelete);

			ResetContacts();

			AppData.Contacts.CollectionChanged += All_CollectionChanged;

			//MessagingCenter.Subscribe<ContactDetailPage, Contact>(this, "AddContact", async (obj, contact) => {
			//	if (contact.ContactType == ContactType) {
			//		Contacts.Add(contact);
			//		Debug.Print("ContactsViewModel[{0}] added: Phone={1}", ContactType, contact.Phone);
			//	}
			//});
		}

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

		protected virtual async void OnAdd()
		{
			await Navigation.PushModalAsync(new NavigationPage(new ContactDetailPage(ContactType)));
		}

		protected virtual async void OnEdit()
		{
			if (Selected != null) {
				await Navigation.PushModalAsync(new NavigationPage(new ContactDetailPage(Selected)));
			//} else {
			//	//await Navigation.PushModalAsync(new NavigationPage(new ContactDetailPage(ContactType)));
			//	await Navigator.NavigateTo(new NavigationPage(new ContactDetailPage(ContactType)));
			}
		}

		private bool CanDelete()
		{
			return Selected != null && !AppData.isEngaged(Selected);
		}

		private async void OnDelete()
		{
			await AppData.RemoveContact(Selected);
		}

		protected override void OnSelectedItemChanged()
		{
			base.OnSelectedItemChanged();

			UpdateCommands();
		}

		public void UpdateCommands()
		{
			EditCommand.ChangeCanExecute();
			DeleteCommand.ChangeCanExecute();
		}
	}
}