using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using Dwares.Dwarf.Toolkit;


namespace ACE.Models
{
	public enum ContactType
	{
		ACE,
		Client,
		Office
	}

	public static class Tag
	{
		public const string Wheelchair = "Wheelchair";
		public const string Escort = "Escort";
	}

	public class Contact: PropertyNotifier
	{
		public Contact(ContactType type)
		{
			ContactType = type;

			Tags = new Tags();
			//Tags.TagChanged += (s, e) => {
			//	if (e.Tag == Tag.Wheelchair) {
			//		RaisePropertyChanged(nameof(Wheelchair));
			//	} else if (e.Tag == Tag.Escort) {
			//		RaisePropertyChanged(nameof(Escort));
			//	}
			//};
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
		public bool IsCompany => ContactType == ContactType.ACE;

		string name;
		public string Name {
			get => name;
			set {
				if (SetProperty(ref name, value)) {
					RaisePropertyChanged(nameof(HasName));
				}
			}
		}

		string shortName;
		public string ShortName {
			get => shortName;
			set => SetProperty(ref shortName, value);
		}

		string phone;
		public string Phone {
			get => phone;
			set {
				if (SetProperty(ref phone, value)) {
					RaisePropertyChanged(nameof(HasPhone));
				}
			}
		}

		string altPhone;
		public string AltPhone {
			get => altPhone;
			set => SetProperty(ref altPhone, value);
		}

		string address;
		public string Address {
			get => address;
			set {
				if (SetProperty(ref address, value)) {
					RaisePropertyChanged(nameof(HasAddress));
				}
			}
		}

		string altAddress;
		public string AltAddress {
			get => altAddress;
			set => SetProperty(ref altAddress, value);
		}

		string comment;
		public string Comment {
			get => comment;
			set {
				if (SetProperty(ref comment, value)) {
					RaisePropertyChanged(nameof(HasComment));
				}
			}
		}

		public Tags Tags { get; }

		//HashSet<string> tags = new HashSet<string>();
		//public bool HasTag(string tag) => tags.Contains(tag);
		//public bool AddTag(string tag) => tags.Add(tag);
		//public bool RemoveTag(string tag) => tags.Remove(tag);
		
		//public void SetTag(string tag, bool onOff)
		//{
		//	if (onOff) {
		//		tags.Add(tag);
		//	} else {
		//		tags.Remove(tag);
		//	}
		//}

		public static string NameAndDescription(string name, string desription)
		{
			if (String.IsNullOrEmpty(desription)) {
				return name ?? String.Empty;
			}
			if (String.IsNullOrEmpty(name)) {
				return desription;
			}
			return String.Format("{0} ({1})", name, desription);
		}

		public string NameSuggestion {
			get {
				string name, desc;
				if (ContactType == ContactType.Office) {
					name = String.IsNullOrEmpty(ShortName) ? Strings.FirstWord(Name) : ShortName;
					desc = Strings.FirstLine(Address);
				} else {
					name = Name;
					desc = Phone;
				}
				return NameAndDescription(name, desc);
			}
		}

		public string PhoneSuggestion {
			get {
				string name;
				if (ContactType == ContactType.Office) {
					if (!String.IsNullOrEmpty(ShortName)) {
						name = ShortName;
					} else if (!String.IsNullOrEmpty(Name)) {
						name = Name;
					} else {
						name = Strings.FirstLine(Address);
					}
				} else {
					name = Name;
				}
				return NameAndDescription(Phone, name);
			}
		}

		public bool HasName => !String.IsNullOrEmpty(Name);
		public bool HasPhone => !String.IsNullOrEmpty(Phone);
		public bool HasAddress => !String.IsNullOrEmpty(Address);
		public bool HasComment => !String.IsNullOrEmpty(Comment);

		Command callCommand = null;
		public Command CallCommand => LazyInitializer.EnsureInitialized(ref callCommand, () => new Command(Call));

		Command directionsCommand = null;
		public Command DirectionsCommand => LazyInitializer.EnsureInitialized(ref directionsCommand, () => new Command(Directions));

		public async void Call()
		{
			await PhoneDialer.DialAsync(Phone, Name);
		}

		public async void Directions()
		{
			string address = Address;
			await MapSvc.OpenDirections(null, address);
		}

			public bool NeedUpdate(string newName = null, string newAddress = null)
		{
			return
				(!String.IsNullOrEmpty(newName) && newName != Name) ||
				(!String.IsNullOrEmpty(newAddress) && Strings.CompareLines(newAddress, Address) != 0);
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

