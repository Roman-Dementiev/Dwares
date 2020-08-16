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
			customers.Add(new Customer { CodeName = "pepper house", Address = "3111 NJ-38 #21\nMt Laurel Township, NJ 08054", Phone = "(856) 234-2929", ContactPhone="111-111-1111", ContactName="Name 1"  });
			customers.Add(new Customer { CodeName = "pizza rs", Tags = "pizza", Address = "7266 Rising Sun Ave\nPhiladelphia, PA 19111", Phone = "(215) 722-5600" });
			customers.Add(new Customer { CodeName = "pizza cot", Tags = "pizza", Address = "3542 Cottman Ave\nPhiladelphia, PA 19149", Phone = "(215) 722-5600" });
			customers.Add(new Customer { CodeName = "judah", Tags="restaurant", Address = "9311 Krewstown Rd\nPhiladelphia, PA 19115", Phone = "(215) 331-7699" });
			customers.Add(new Customer { CodeName = "palace royl", Tags = "restaurant", Address = "9859 Bustleton Ave\nPhiladelphia, PA 19115", Phone = "(215) 677-3323", ContactPhone = "333-333-3333" });
			customers.Add(new Customer { CodeName = "passage", ContactPhone="444-444-4444" });

			return Task.CompletedTask;
		}

		public Task LoadPlaces()
		{
			var appScope = AppScope.Instance;
			appScope.StartPoint = new Place { CodeName = "market", FullName = "Produce Market", Tags = "startpoint", Address = "6700 Essington Ave\nPhiladelphia, PA 19153" };
			appScope.EndPoint = new Place { CodeName = "parking", FullName = "Parking lot", Tags = "endpoint", Address = "12012 Bustleton Ave, Philadelphia, PA 19116" };
			
			var places = appScope.Places;
			places.Add(appScope.StartPoint);
			places.Add(appScope.EndPoint);

			return Task.CompletedTask;
		}

		public async Task LoadRoute()
		{
			var route = AppScope.Instance.Route;
			//var startStop = new RouteStop(AppScope.Instance.StartPoint) { Status = RouteStatus.Arrived };
			//route.Add(startStop, false);
			await route.Add(new CustomerStop(AppScope.GetCustomer("pepper house")), false);
			await route.Add(new CustomerStop(AppScope.GetCustomer("pizza rs")), false);
			await route.Add(new CustomerStop(AppScope.GetCustomer("pizza cot")), false);
			await route.Add(new CustomerStop(AppScope.GetCustomer("judah")), false);
			await route.Add(new CustomerStop(AppScope.GetCustomer("passage")), false);
			await route.Add(new CustomerStop(AppScope.GetCustomer("palace royl")), false);			
		}

		public Task AddRouteStop(RouteStop stop) => Task.CompletedTask;
		public Task DeleteRouteStop(RouteStop stop) => Task.CompletedTask;
		public Task ChangeRouteStopSeq(RouteStop stop) => Task.CompletedTask;
		public Task ChangeRouteStopStatus(RouteStop stop) => Task.CompletedTask;
	}
}
