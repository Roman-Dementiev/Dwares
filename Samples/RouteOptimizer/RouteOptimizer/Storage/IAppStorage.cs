using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteOptimizer.Models;


namespace RouteOptimizer.Storage
{
	public interface IAppStorage
	{
		Task<Place[]> LoadPlacesAsync();
		Task SavePlacesAsync(Places places);
		Task LoadRouteAsync(Route route);

		Task AddPlaceAsync(Place place);
		Task UpdatePlaceAsync(Place place);
		Task DeletePlaceAsync(Place place);
	}
}
