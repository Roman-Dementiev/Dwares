using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteOptimizer.Models;


namespace RouteOptimizer.Storage
{
	public interface IAppStorage
	{
		Task LoadPlacesAsync(IList<Place> places);
		Task SavePlacesAsync(IList<Place> places);
		Task LoadRouteAsync(Route route);

		Task AddPlaceAsync(Place place);
		Task UpdatePlaceAsync(Place place);
		Task DeletePlaceAsync(Place place);
	}
}
