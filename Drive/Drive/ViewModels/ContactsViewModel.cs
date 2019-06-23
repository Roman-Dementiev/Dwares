using System;
using System.Windows.Input;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Drive.Models;
using Drive.Views;


namespace Drive.ViewModels
{
	//public enum ContactsSelector
	//{
	//	Phones,
	//	Places,
	//	Clients
	//}

	public class ContactsViewModel : CollectionViewModel<ContactItem>, ITabContentViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsViewModel1cs));
		static readonly Type initialContactType = typeof(Person);

		public ContactsViewModel() : this(initialContactType) { }

		public ContactsViewModel(Type contactType) :
			base(ApplicationScope, ContactItem.CreateCollection(contactType))
		{
			//Debug.EnableTracing(@class);

			Title = "Contacts";
			ContactsType = contactsType;

			PhonesCommand = new Command(() => SelectView(typeof(Person)));
			PlacesCommand = new Command(() => SelectView(typeof(Place)));
			ClientsCommand = new Command(() => SelectView(typeof(Client)));
		}

		public ContactItemsCollection ContactItems {
			get => Items as ContactItemsCollection;
		}

		public ICommand PhonesCommand { get; set; }
		public ICommand PlacesCommand { get; set; }
		public ICommand ClientsCommand { get; set; }

		Type contactsType = initialContactType;
		public Type ContactsType {
			get => contactsType;
			set => SetPropertyEx(ref contactsType, value, nameof(ContactsType),
				nameof(PhonesViewIsActive), nameof(PlacesViewIsActive), nameof(ClientsViewIsActive));
		}

		public bool PhonesViewIsActive {
			//get => ContactsType == typeof(Person);
			get {
				var value = ContactsType == typeof(Person);
				Debug.Print($"PhonesViewIsActive={value}");
				return value;
			}

		}
		public bool PlacesViewIsActive {
			get => ContactsType == typeof(Place);
		}
		public bool ClientsViewIsActive {
			get => ContactsType == typeof(Client);
		}

		void SelectView(Type contactsType)
		{
			//Debug.Print($"ContactsViewModel.SelectView(): selector={selector}");

			if (contactsType != ContactsType) {
				ContactsType = contactsType;
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
