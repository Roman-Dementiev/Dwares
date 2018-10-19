﻿using System;
using Xamarin.Forms;
using System.Threading;
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

	public class Contact: PropertyNotifier
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
			//Uri uri = new Uri("bingmaps:?rtp=adr.Mountain%20View,%20CA~adr.San%20Francisco,%20CA&amp;mode=d&amp;trfc=1");
			//await MapSvc.OpenMapUri(uri);

			//await MapSvc.OpenAddress("122 Ridge Road Lyndhurst NJ 07071");
			await MapSvc.OpenAddress("4143 Paul street Philadelphia PA 19124");
			//await MapSvc.OpenDirections("4143 Paul street Philadelphia PA 19124", "10162 Bustleton Ave Philadelphia PA 19116");
			//await MapSvc.OpenDirections("10162 Bustleton Ave Philadelphia PA 19116", null);

			//await MapSvc.OpenDirections(null, Address);
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

