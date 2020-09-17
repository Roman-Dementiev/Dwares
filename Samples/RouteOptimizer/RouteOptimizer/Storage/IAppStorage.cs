using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteOptimizer.Models;


namespace RouteOptimizer.Storage
{
	public interface IAppStorage
	{
		Task LoadPlacesAsync(IList<Place> places);
		Task SavePlacesAsync(Places places);
		Task LoadRouteAsync(Route route);

		Task<string> AddPlaceAsync(Place place);
		Task<string> UpdatePlaceAsync(string oldId, Place place);
		Task DeletePlaceAsync(string placeId);
	}
}
