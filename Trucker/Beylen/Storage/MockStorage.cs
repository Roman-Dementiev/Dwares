using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Beylen.Models;


namespace Beylen.Storage
{
	public class MockStorage : IAppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(MockStorage));

		public MockStorage()
		{
			//Debug.EnableTracing(@class);
		}

		public Task Initialize()
		{
			return Task.CompletedTask;
		}

		public Task LoadContacts()
		{
			var contacts = AppScope.Instance.Contacts;
			contacts.Add(new Contact { Name = "Leonid", Phone = "267-474-1744"});
			contacts.Add(new Contact { Name = "Artem", Phone = "267-699-8528" });
			contacts.Add(new Contact { Name = "Stepan", Phone = "267-516-1842" });
			contacts.Add(new Contact { Name = "Boris", Phone = "267-694-2683" });
			contacts.Add(new Contact { Name = "Roman", Phone = "610-809-9619" });

			return Task.CompletedTask;
		}

		public Task LoadCustomers()
		{
			var customers = AppScope.Instance.Customers;
			customers.Add(new Customer { Name = "pepper house", Address = "3111 NJ-38 #21\nMt Laurel Township, NJ 08054", Phone = "(856) 234-2929" });
			customers.Add(new Customer { Name = "judah", Address = "9311 Krewstown Rd\nPhiladelphia, PA 19115", Phone = "(215) 613-6110" });
			customers.Add(new Customer { Name = "palace royl", Address = "9859 Bustleton Ave\nPhiladelphia, PA 19115", Phone = "215) 677-3323" });
			customers.Add(new Customer { Name = "passage" });

			return Task.CompletedTask;
		}
	}
}
