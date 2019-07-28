using System;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;
using Dwares.Druid.UI;
using Xamarin.Forms;
using Drive.ViewModels;


namespace Drive.Views
{
	public class ContactsButton : ArtButtonEx
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsButton));

		public ContactsButton()
		{
			//Debug.EnableTracing(@class);

			DefaultFlavor = "TabButton-default";
			SelectedFlavor = "TabButton-active";

			MessageBroker.Subscribe<ActiveContactsMessage>(this, (message) => {
				activeTab = message.ActiveContactsTab;
				IsSelected = contactsTab == activeTab;
			});
		}

		ContactsTab activeTab = default;
		ContactsTab contactsTab;
		public ContactsTab ContactsTab {
			get => contactsTab;
			set {
				contactsTab = value;
				IsSelected = contactsTab == activeTab;
			}
		}
	}
}
