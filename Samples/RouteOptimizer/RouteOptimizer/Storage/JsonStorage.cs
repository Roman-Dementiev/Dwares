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

		public async Task LoadPlacesAsync(Places places)
		{
			try {
				string path = Path.Combine(FileSystem.AppDataDirectory, kPlacesFn);
				if (!File.Exists(path))
					return;

				var text = await Files.ReadAllText(path, async: true);
				var json = DeserializeJson<PlacesJson>(text);

				foreach (var rec in json.Places)
				{
					var place = new Place {
						Name = rec.Name ?? string.Empty,
						Tags = rec.Tags ?? string.Empty,
						//Tnfo = rec.Info ?? string.Emty,
						Address = rec.Address ?? string.Empty,
						Phone = rec.Info ?? string.Empty
					};
					places.Add(place);
				}
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
		}

		public void SavePlaces()
		{
			SavePlaces(App.Current.Places);
		}

		public Task SavePlacesAsync(Places places)
		{
			SavePlaces(places);
			return Task.CompletedTask;
		}

		public void SavePlaces(Places places)
		{
			try {
				var text = SerializePlaces(places.List);

				string path = Path.Combine(FileSystem.AppDataDirectory, kPlacesFn);
				File.WriteAllText(path, text);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
		}

		public Task LoadRouteAsync(Route route)
		{
			return Task.CompletedTask;
		}

		public Task AddPlaceAsync(Place place)
		{
			SavePlaces();
			return Task.CompletedTask;
		}

		public Task UpdatePlaceAsync(Place place)
		{
			SavePlaces();
			return Task.CompletedTask;
		}

		public Task DeletePlaceAsync(Place place)
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
					//Info = place.Info,
					Address = place.Address,
					Phone = place.Phone
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
	}


	internal struct PlaceRecord
	{
		public string Name { get; set; }
		public string Tags { get; set; }
		public string Info { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
	}

	internal class PlacesJson
	{
		public int Version { get; set; }
		public PlaceRecord[] Places { get; set; }
	}
}
