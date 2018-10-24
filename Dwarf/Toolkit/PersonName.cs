using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public class PersonName : GeneralName
	{
		public PersonName() { }

		public PersonName(string fullName, bool normalize = false) :
			base(fullName, normalize)
		{}

		public PersonName(string firstName, string lastName) :
			base(Strings.JoinNonEmpty(" ", firstName, lastName))
		{
		}

		public PersonName(string firstName, string secondName, string lastName) :
			base(Strings.JoinNonEmpty(" ", firstName, secondName, lastName))
		{
		}

		public string FirstName {
			get {
				var names = NameParts;
				if (names.Length > 0) {
					return names[0];
				} else {
					return String.Empty;
				}
			}
		}

		public string SecondName {
			get {
				var names = NameParts;
				if (names.Length > 2) {
					return names[1];
				} else {
					return String.Empty;
				}
			}
		}

		public string LastName {
			get {
				var names = NameParts;
				switch (names.Length) {
				case 0:
				case 1:
					return String.Empty;
				case 2:
					return names[1];
				case 3:
					return names[2];
				default:
					return String.Join(" ", names, 2, names.Length - 2);
				}
			}
		}

		public static implicit operator string(PersonName name) => name.FullName;
		public static implicit operator PersonName(string name) => new PersonName(name);
	}
}
