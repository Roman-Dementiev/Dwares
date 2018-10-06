using System;
using Xamarin.Forms;
using ACE.Services;
using System.Threading;

namespace ACE.Models
{
	public enum ContactType
	{
		Company,
		Client,
		Office
	}

	public class Contact
	{
		public Contact(ContactType type)
		{
			ContactType = type;
		}

		//public Contact(Contact src) :
		//	this(src.ContactType)
		//{
		//	CopyFrom(src);
		//}

		//public void CopyFrom(Contact src)
		//{
		//	ContactType = src.ContactType;
		//	Name = src.Name;
		//	Phone = src.Phone;
		//	Address = src.Address;
		//	Comment = src.Comment;
		//}

		public ContactType ContactType { get; private set;  }
		public bool IsClient => ContactType == ContactType.Client;
		public bool IsFacility => ContactType == ContactType.Office;
		public bool IsCompany => ContactType == ContactType.Company;

		public string Name { get; set; }
		public string Phone { get; set; }
		public string AltPhone { get; set; }
		public string Address { get; set; }
		public string AltAddress { get; set; }
		public string Comment { get; set; }

		public bool HasName => !String.IsNullOrEmpty(Name);
		public bool HasPhone => !String.IsNullOrEmpty(Phone);
		public bool HasAddress => !String.IsNullOrEmpty(Address);
		public bool HasComment => !String.IsNullOrEmpty(Comment);

		Command callCommand = null;
		public Command CallCommand => LazyInitializer.EnsureInitialized(ref callCommand, () => new Command(Call));

		public async void Call()
		{
			await PhoneDialer.DialAsync(Phone, Name);
		}
	}
}

