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

		public Task LoadPlaces(IList<Place> places)
		{
			try {
				string path = Path.Combine(FileSystem.AppDataDirectory, kPlacesFn);
				if (!File.Exists(path))
					return Task.CompletedTask;

				var text = File.ReadAllText(path);
				var json = DeserializeJson<PlacesJson>(text);

				foreach (var rec in json.Places)
				{
					var place = new Place {
						Name = rec.Name ?? string.Empty,
						Tags = rec.Tags ?? string.Empty,
						Address = rec.Address ?? string.Empty
					};
					places.Add(place);
				}
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}

			return Task.CompletedTask;
		}

		public void SavePlaces()
		{
			try {
				var text = SerializePlaces(App.Current.Places);

				string path = Path.Combine(FileSystem.AppDataDirectory, kPlacesFn);
				File.WriteAllText(path, text);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
		}

		public Task LoadRoute(Route route)
		{
			return Task.CompletedTask;
		}

		public Task AddPlace(Place place)
		{
			SavePlaces();
			return Task.CompletedTask;
		}

		public Task UpdatePlace(Place place)
		{
			SavePlaces();
			return Task.CompletedTask;
		}

		public Task DeletePlace(Place place)
		{
			SavePlaces();
			return Task.CompletedTask;
		}


		internal static PlacesJson PlacesToJson(IList<Place> places)
		{
			var json = new PlacesJson {
				Version = kVersion1,
				Places = new PlaceRecord[places.Count]
			};

			for (int i = 0; i < places.Count; i++) {
				var place = places[i];
				json.Places[i] = new PlaceRecord {
					Name = place.Name,
					Tags = place.Tags,
					Address = place.Address
				};
			}

			return json;
		}

		public static string SerializePlaces(IList<Place> places)
		{
			var json = PlacesToJson(places);
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

		public Json DeserializeJson<Json>(string text) where Json : class
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
	}


	internal struct PlaceRecord
	{
		public string Name { get; set; }
		public string Tags { get; set; }
		//public string Info { get; set; }
		public string Address { get; set; }
	}

	internal class PlacesJson
	{
		public int Version { get; set; }
		public PlaceRecord[] Places { get; set; }
	}
}
