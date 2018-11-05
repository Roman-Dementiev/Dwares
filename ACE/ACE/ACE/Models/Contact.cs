using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid.Services;
using Dwares.Dwarf.Toolkit;
using Dwares.Drums;


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

	public class Contact: PropertyNotifier, ILocation
	{
		//public Contact()
		//{
		//	//ContactType = type;
		//}

		public override string ToString()
		{
			return Strings.Properties(this,
				new string[] { nameof(Name), nameof(Phone), nameof(AltPhone), nameof(Address), nameof(Comment) },
				skipNull: true);
		}

		public ContactType ContactType { get; set;  }
		public bool IsClient => ContactType == ContactType.Client;
		public bool IsFacility => ContactType == ContactType.Office;
		public bool IsCompany => ContactType == ContactType.ACE;

		string name;
		public string Name {
			get => name;
			set {
				if (SetProperty(ref name, value)) {
					FirePropertyChanged(nameof(HasName));
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
					FirePropertyChanged(nameof(HasPhone));
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
					FirePropertyChanged(nameof(HasAddress));
				}
			}
		}

		public ICoordinate Coordinate { get; set;  }

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
					FirePropertyChanged(nameof(HasComment));
				}
			}
		}

		public Tags Tags { get; } = new Tags();
		public bool HasTag(string tag) => Tags.HasTag(tag);
		public void SetTag(string tag, bool onOff, [CallerMemberName] string propertyName = "")
		{
			if (onOff != HasTag(tag)) {
				Tags.SwitchTag(tag, onOff);
				FirePropertyChanged(propertyName);
			}
		}
		public void AddTags(string[] tags)
		{
			foreach (var tag in tags) {
				Tags.AddTag(tag);
			}
		}
		public string[] GetTags()
		{
			return  Collection.ToArray(Tags.GetTags());
		}

		public bool Wheelchair {
			get => HasTag(Tag.Wheelchair);
			set => SetTag(Tag.Wheelchair, value);
		}

		public bool Escort {
			get => HasTag(Tag.Escort);
			set => SetTag(Tag.Escort, value);
		}

		public string Title {
			get {
				if (!String.IsNullOrEmpty(Name))
					return Name;
				if (!String.IsNullOrEmpty(Phone))
					return Phone;
				return "?";
			}
		}

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

			var info = await Drum.GetRouteInfo("4143 Paul St Philadelphia PA 19124", address);
			Debug.Print("Contact.Directions(): RouteInfo={0}", info);

			await Drum.OpenDirections(null, address);
		}

			public bool NeedUpdate(string newName = null, string newAddress = null)
		{
			return
				(!String.IsNullOrEmpty(newName) && newName != Name) ||
				(!String.IsNullOrEmpty(newAddress) && Strings.CompareLines(newAddress, Address) != 0);
		}

		public void Update(string newName = null, string newAddress = null, bool? wheelchair = null, bool? escort = null)
		{
			if (!String.IsNullOrEmpty(newName))
				Name = newName;
			if (!String.IsNullOrEmpty(newAddress))
				Address = newAddress;
			if (wheelchair != null)
				Wheelchair = (bool)wheelchair;
			if (escort != null)
				Escort = (bool)escort;
		}
	}
}

