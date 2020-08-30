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

	internal struct PackingSuggestion
	{
		public PackingSuggestion(Packing packing)
		{
			Packing = packing;
		}

		public Packing Packing { get; }
		public override string ToString() => Packing.Name;
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

		public static List<object> GetCustomersSuggestions(Func<Customer, bool> filter = null)
		{
			var list = new List<object>();
			CollectCustomers(list, filter);
			return list;
		}

		public static List<object> GetProduceSuggestion()
		{
			var list = new List<object>();
			var produce = AppScope.Instance.Produce;
			foreach (var item in produce) {
				list.Add(new ProduceSuggestion(item));
			}
			return list;
		}

		public static List<object> GetPackingsSuggestion()
		{
			var list = new List<object>();
			foreach (var item in Packing.List) {
				list.Add(new PackingSuggestion(item));
			}
			return list;
		}

		public static List<object> Customers {
			get => customers ??= GetCustomersSuggestions();
		}
		static List<object> customers;

		public static List<object> Produce {
			get => produce ??= GetProduceSuggestion();
		}
		static List<object> produce;

		public static List<object> Packings {
			get => packings ??= GetPackingsSuggestion();
		}
		static List<object> packings;
	}
}


