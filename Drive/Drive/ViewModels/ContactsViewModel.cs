using System;
using System.Windows.Input;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Drive.Models;
using Drive.Views;


namespace Drive.ViewModels
{
	public enum ContactsTab
	{
		Phones,
		Places,
		Clients
	}

	public class ActiveContactsMessage
	{
		public ContactsTab ActiveContactsTab { get; set; }
	}


	public class ContactsViewModel : CollectionViewModel<ContactCardViewModel>, IRootContentViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsViewModel1cs));
		static readonly ContactsTab initialContactTab = ContactsTab.Phones;

		public ContactsViewModel() : this(ContactsTab.Phones, typeof(Person)) { }

		public ContactsViewModel(ContactsTab tab, Type contactType) :
			base(ApplicationScope, ContactCardViewModel.CreateCollection(contactType))
		{
			//Debug.EnableTracing(@class);

			Title = "Contacts";
			ContactsTab = tab;

			PhonesCommand = new Command(() => SelectTab(ContactsTab.Phones, typeof(Person)));
			PlacesCommand = new Command(() => SelectTab(ContactsTab.Places, typeof(Place)));
			ClientsCommand = new Command(() => SelectTab(ContactsTab.Clients, typeof(Client)));
		}

		public ContactItemsCollection ContactItems {
			get => Items as ContactItemsCollection;
		}

		public ICommand PhonesCommand { get; set; }
		public ICommand PlacesCommand { get; set; }
		public ICommand ClientsCommand { get; set; }

		ContactsTab contactsTab;
		public ContactsTab ContactsTab {
			get => contactsTab;
			set {
				if (SetProperty(ref contactsTab, value)) {
					var message = new ActiveContactsMessage() { ActiveContactsTab = contactsTab };
					MessageBroker.Send(message);
				}
			}
		}

		//public bool PhonesViewIsActive {
		//	//get => ContactsType == typeof(Person);
		//	get {
		//		var value = ContactsType == typeof(Person);
		//		Debug.Print($"PhonesViewIsActive={value}");
		//		return value;
		//	}

		//}
		//public bool PlacesViewIsActive {
		//	get => ContactsType == typeof(Place);
		//}
		//public bool ClientsViewIsActive {
		//	get => ContactsType == typeof(Client);
		//}

		void SelectTab(ContactsTab tab, Type contactsType)
		{
			//Debug.Print($"ContactsViewModel.SelectView(): selector={selector}");

			if (tab != ContactsTab) {
				ContactsTab = tab;
				ContactItems?.Recollect(contactsType);
			}
		}

		public Type ContentViewType()
		{
			return typeof(ContactsView);
		}

		public Type ControlsViewType(bool landscape)
		{
			return landscape ? typeof(ContactsSideControls) : typeof(ContactsTopControls);
		}
	}
}
