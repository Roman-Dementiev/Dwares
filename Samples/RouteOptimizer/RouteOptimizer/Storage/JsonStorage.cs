using Dwares.Druid;
using Dwares.Dwarf;
using Newtonsoft.Json;
using RouteOptimizer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace RouteOptimizer.Storage
{
	public class JsonStorage : IAppStorage
	{
		const int kVersion1 = 1;
		const string kPlacesFn = "Places.json";
		const string kRouteFn = "Route.json";

		//public async Task LoadPlacesAsync(Places places)
		public async Task<Place[]> LoadPlacesAsync()
		{
			try {
				string path = Path.Combine(FileSystem.AppDataDirectory, kPlacesFn);
				if (!File.Exists(path))
					return null;

				var text = await Files.ReadTextAsync(path);
				var json = DeserializeJson<PlacesJson>(text);

				int count = json.Places.Length;
				var array = new Place[count];
				for (int i = 0; i < count; i++) {
					var rec = json.Places[i];
					array[i] = JsonToPlace(rec);
				}
				return array;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return null;
			}
		}

		public async Task SavePlacesAsync(IList<Place> places) => await SavePlacesAsync(places, null);

		public async Task SavePlacesAsync(IList<Place> places, Place addPlace)
		{
			try {
				var text = SerializePlaces(places, addPlace);

				string path = Path.Combine(FileSystem.AppDataDirectory, kPlacesFn);
				await Files.WriteTextAsync(path, text);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
		}

		public Task<RouteStop[]> LoadRouteAsync()
		{
			return Task.FromResult<RouteStop[]>(null);
		}

		public async Task<string> AddPlaceAsync(Place place)
		{
			place.Id = Ids.PlaceId(place.Name, place.Address);
			await SavePlacesAsync(App.Current.Places.List, place);
			return place.Id;
		}

		public async Task<string> UpdatePlaceAsync(string oldId, Place place)
		{
			place.Id = Ids.PlaceId(place.Name, place.Address);
			await SavePlacesAsync(App.Current.Places.List);
			return place.Id;
		}

		public async Task DeletePlaceAsync(string placeId)
		{
			await SavePlacesAsync(App.Current.Places.List);
		}

		public static string SerializePlaces(IList<Place> places, Place addPlace = null)
		{
			int count = places.Count;
			if (addPlace != null)
				count++;

			var json = new PlacesJson {
				Version = kVersion1,
				Places = new PlaceRecord[count]
			};

			for (int i = 0; i < places.Count; i++) {
				var place = places[i];
				json.Places[i] = PlaceToJson(place);
			}

			if (addPlace != null) {
				json.Places[count-1] = PlaceToJson(addPlace);
			}

			return SerializeJson(json);
		}

		// TODO
		//public static string SerializeRoute(Route route)
		//{
		//}

		public static string SerializeJson(object json)
		{
			try {
				var serializer = new JsonSerializer {
					NullValueHandling = NullValueHandling.Ignore,
					Formatting = Formatting.Indented
				};

				using (var sw = new StringWriter())
				using (var writer = new JsonTextWriter(sw)) {
					serializer.Serialize(writer, json);
					var text = sw.ToString();

					return text;
				}
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return null;
			}
		}

		public static Json DeserializeJson<Json>(string text) where Json : class
		{
			Json json;
			try {
				json = JsonConvert.DeserializeObject<Json>(text);
				return json;
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}

		internal static Place JsonToPlace(PlaceRecord rec)
		{
			string id = Ids.PlaceId(rec.Name, rec.Address);
			var place = new Place {
				Id = id,
				Name = rec.Name ?? string.Empty,
				Tags = rec.Tags ?? string.Empty,
				Note = rec.Note ?? string.Empty,
				Address = rec.Address ?? string.Empty,
				Phone = rec.Phone ?? string.Empty
			};
			return place;
		}

		internal static PlaceRecord PlaceToJson(Place place)
		{
			return new PlaceRecord {
				Name = place.Name,
				Tags = place.Tags,
				Note = place.Note,
				Address = place.Address,
				Phone = place.Phone
			};
		}
	}


	internal struct PlaceRecord
	{
		public string Name { get; set; }
		public string Tags { get; set; }
		public string Note { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
	}

	internal class PlacesJson
	{
		public int Version { get; set; }
		public PlaceRecord[] Places { get; set; }
	}
}
