using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RouteOptimizer.Models;


namespace RouteOptimizer.Storage
{
	public enum SortOrder
	{
		None,
		ByNameOnly,
		ByCategory
	}

	public interface IAppStorage
	{
		Task<Place[]> LoadPlacesAsync();
		Task SavePlacesAsync(IList<Place> places);
		Task<RouteStop[]> LoadRouteAsync();

		Task<string> AddPlaceAsync(Place place);
		Task<string> UpdatePlaceAsync(string oldId, Place place);
		Task DeletePlaceAsync(string placeId);
	}

	public static class AppStorage
	{
		public static IAppStorage Instance {
			get => instance ??= new JsonStorage();
		}
		static IAppStorage instance;
	}
}
