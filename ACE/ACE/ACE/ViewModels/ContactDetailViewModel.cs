using System;
using System.Threading.Tasks;
using Dwares.Druid.Support;
using ACE.Models;


namespace ACE.ViewModels
{
	public class ContactDetailViewModel: FormScope
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
		}

		public Contact Source { get; }
		public ContactType ContactType { get; }

		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Comment { get; set; }

		protected override async Task DoAccept()
		{
			if (Source == null) {
				var newContact = new Contact(ContactType) {
					Name = this.Name,
					Phone = this.Phone,
					Address = this.Address,
					Comment = this.Comment
				};

				await AppData.NewContact(newContact);
			} else {
				Source.Name = this.Name;
				Source.Phone = this.Phone;
				Source.Address = this.Address;
				Source.Comment = this.Comment;
			}

			await AppData.SaveAsync();
		}
	}
}
