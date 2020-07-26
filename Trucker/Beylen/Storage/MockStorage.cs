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
	}
}
