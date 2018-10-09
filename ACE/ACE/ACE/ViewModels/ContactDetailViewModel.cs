using System;
using Xamarin.Forms;
using ACE.Models;
using Dwares.Druid.Support;


namespace ACE.ViewModels
{
	public class ContactDetailViewModel: BindingScope
	{
		public ContactDetailViewModel(Contact source) : this(source, source.ContactType) { }
		
		public ContactDetailViewModel(ContactType type) : this(null, type) { }

		private ContactDetailViewModel(Contact source, ContactType type)
		{
			Source = source;
			ContactType = type;

			switch (type)
			{
			case ContactType.Client:
				Title = "Client Details";
				break;
			case ContactType.Office:
				Title = "Office Details";
				break;
			default:
				Title = "Contact Details";
				break;
			}

			if (source != null) {
				Name = source.Name;
				Phone = source.Phone;
				Address = source.Address;
				Comment = source.Comment;
			}

			AcceptCommand = new Command(OnAccept);
			CancelCommand = new Command(OnCancel);
		}

		public Contact Source { get; }
		public ContactType ContactType { get; }

		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Comment { get; set; }

		public Command AcceptCommand { get; }
		public Command CancelCommand { get; }


		public async void OnAccept()
		{
			var newContact = new Contact(ContactType) {
				Name = this.Name,
				Phone = this.Phone,
				Address = this.Address,
				Comment = this.Comment
			};

			await AppData.ReplaceContact(newContact, Source);
			//await Navigation.PopModalAsync();
		}

		public async void OnCancel()
		{
			//await Navigation.PopModalAsync();
			await Navigator.PopPageAsync();
		}
	}
}
