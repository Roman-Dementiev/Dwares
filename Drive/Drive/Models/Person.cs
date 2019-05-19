﻿using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Drive.Models
{
	public class Person : PropertyNotifier, IContact
	{
		//static ClassRef @class = new ClassRef(typeof(Person));

		public Person()
		{
			//Debug.EnableTracing(@class);
		}

		string fullName;
		public string FullName {
			get => fullName;
			set => SetPropertyEx(ref fullName, value, nameof(FullName), nameof(Title));
		}

		PhoneNumber phoneNumber;
		public PhoneNumber PhoneNumber {
			get => phoneNumber;
			set => SetProperty(ref phoneNumber, value);
		}

		Home home;
		public Home Home {
			get => home;
			set => SetProperty(ref home, value);
		}

		public string Title {
			get => FullName;
			set => FullName = value;
		}

		public string Address {
			get => Home?.Address;
			set {
				if (value != Home?.Address) {
					Home = value == null ? null : new Home(value);
					FirePropertyChanged();
				}
			}
		}
	}
}
