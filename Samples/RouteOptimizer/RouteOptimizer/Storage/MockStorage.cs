//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Dwares.Dwarf.Collections;
//using RouteOptimizer.Models;

//namespace RouteOptimizer.Storage
//{
//	public class MockStorage : IAppStorage
//	{
//		public MockStorage()
//		{
//		}

//		public Task LoadPlaces(IList<Place> places)
//		{
//			foreach (var place in Places) {
//				places.Add(place);
//			}

//			return Task.CompletedTask;
//		}

//		public Task LoadRoute(Route route)
//		{
//			return Task.CompletedTask;
//		}

//		public Task AddPlace(Place place)
//		{
//			Places.Add(place);
//			return Task.CompletedTask;
//		}

//		public Task UpdatePlace(Place place)
//		{
//			return Task.CompletedTask;
//		}

//		List<Place> Places { get; } = new List<Place> {
//			new Place {
//				Name = "Uzbekistan Restaurant",
//				Tags = "restaurant",
//				Address = "12012 Bustleton Ave,\nPhiladelphia, PA 19116"
//			},
//			new Place {
//				Name = "Philadelphia Produce Market",
//				Address = "6700 Essington Ave\nPhiladelphia, PA 19153"
//			}
//		};
//	}
//}
