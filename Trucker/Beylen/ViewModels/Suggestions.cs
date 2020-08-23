using System;
using Dwares.Dwarf;
using Beylen.Models;
using System.Collections;
using System.Collections.Generic;

namespace Beylen.ViewModels
{
	internal struct CustomerSuggestion
	{
		public CustomerSuggestion(Customer customer)
		{
			Customer = customer;
		}

		public Customer Customer { get; }
		public override string ToString() => Customer.CodeName;
	}

	internal struct ProduceSuggestion
	{
		public ProduceSuggestion(Produce produce)
		{
			Produce = produce;
		}

		public Produce Produce { get; }
		public override string ToString() => Produce.Name;
	}

	public static class Suggestions
	{
		public static void CollectCustomers(IList list, Func<Customer, bool> filter)
		{
			var customers = AppScope.Instance.Customers;
			if (filter == null) {
				foreach (var item in customers) {
					list.Add(new CustomerSuggestion(item));
				}
			} else {
				foreach (var item in customers) {
					if (filter(item)) {
						list.Add(new CustomerSuggestion(item));
					}
				}
			}
		}

		public static List<object> Customers(Func<Customer, bool> filter = null)
		{
			var list = new List<object>();
			CollectCustomers(list, filter);
			return list;
		}

		public static List<object> Produce()
		{
			var list = new List<object>();
			var produce = AppScope.Instance.Produce;
			foreach (var item in produce) {
				list.Add(new ProduceSuggestion(item));
			}
			return list;
		}
	}
}


