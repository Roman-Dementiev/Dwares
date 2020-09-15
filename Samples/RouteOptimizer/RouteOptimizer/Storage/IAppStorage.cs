using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteOptimizer.Models;


namespace RouteOptimizer.Storage
{
	public interface IAppStorage
	{
		Task LoadPlaces(IList<Place> places);
		Task LoadRoute(Route route);

		Task AddPlace(Place place);
		Task UpdatePlace(Place place);
		Task DeletePlace(Place place);
	}
}
