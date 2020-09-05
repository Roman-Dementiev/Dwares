using Beylen.Models;
using System;
using System.Threading.Tasks;


namespace Beylen.Storage
{
	public interface IAppStorage
	{
		Task Initialize();

		Task LoadData(string carId, bool initializing, bool resetProperties);

		Task LoadProduce();
		Task LoadContacts();
		Task LoadCustomers();
		Task LoadPlaces();
		Task LoadInvoices(string carId);
		Task LoadRoute();

		Task AddRouteStop(RouteStop stop);
		Task DeleteRouteStop(RouteStop stop);
		Task ChangeRouteStopOrdinal(RouteStop stop);
		Task ChangeRouteStopStatus(RouteStop stop);


		Task<string> GetProperty(string name, Car car);
		Task SetProperty(string name, Car car, string value);


		Task NewInvoice(Invoice invoice);
		Task UpdateInvoice(Invoice invoice);
	}
}
