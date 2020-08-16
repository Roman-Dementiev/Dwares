using Beylen.Models;
using System;
using System.Threading.Tasks;


namespace Beylen.Storage
{
	public interface IAppStorage
	{
		Task Initialize();

		Task LoadContacts();
		Task LoadCustomers();
		Task LoadPlaces();
		Task LoadRoute();

		Task AddRouteStop(RouteStop stop);
		Task DeleteRouteStop(RouteStop stop);
		Task ChangeRouteStopSeq(RouteStop stop);
		Task ChangeRouteStopStatus(RouteStop stop);
	}
}
