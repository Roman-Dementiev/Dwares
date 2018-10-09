using System;
using Xamarin.Forms;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Druid.Services;


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

		public override string ToString()
		{
			return Strings.Properties(this,
				new string[] { nameof(Name), nameof(Phone), nameof(AltPhone), nameof(Address), nameof(Comment) },
				skipNull: true);
		}

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

		public bool NeedUpdate(string newName = null, string newAddress = null)
		{
			return
				(!String.IsNullOrEmpty(newName) && newName != Name) ||
				(!String.IsNullOrEmpty(newAddress) && newAddress != Address);
		}

		public void Update(string newName = null, string newAddress = null)
		{
			if (!String.IsNullOrEmpty(newName))
				Name = newName;
			if (!String.IsNullOrEmpty(newAddress))
				Address = newAddress;
		}
	}
}

