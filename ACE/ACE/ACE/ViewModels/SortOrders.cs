using System;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using ACE.Models;


namespace ACE.ViewModels
{
	public class ContactSortOrder : SortOrder<Contact>
	{
		public ContactSortOrder(string name, Comparison<Contact> comparison, bool descending = false) :
			base(name, comparison, descending)
		{
		}
	}

	public class SortByName : ContactSortOrder
	{
		public SortByName() : base("By Name", Comparison) { }

		public static int Comparison(Contact c1, Contact c2) => String.Compare(c1.Name, c2.Name);
	}

	public class SortByFirstName : ContactSortOrder
	{
		public SortByFirstName() : base("By First Name", Comparison) { }

		public static int Comparison(Contact c1, Contact c2)
		{
			int result = String.Compare(PersonName.GetFirstName(c1.Name), PersonName.GetFirstName(c2.Name));
			if (result == 0) {
				result = String.Compare(c1.Name, c2.Name);
			}
			return result;
		}
	}

	public class SortByLastName : ContactSortOrder
	{
		public SortByLastName() : base("By Last Name", Comparison) { }

		public static int Comparison(Contact c1, Contact c2)
		{
			int result = String.Compare(PersonName.GetLastName(c1.Name), PersonName.GetLastName(c2.Name));
			if (result == 0) {
				result = String.Compare(c1.Name, c2.Name);
			}
			return result;
		}
	}

	public class SortByPhone : ContactSortOrder
	{
		public SortByPhone() : base("By Phone Number", Comparison) { }

		public static int Comparison(Contact c1, Contact c2) => String.Compare(c1.Phone, c2.Phone);
	}

}
