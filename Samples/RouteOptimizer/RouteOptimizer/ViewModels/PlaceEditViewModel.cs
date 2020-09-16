using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.ViewModels;
using RouteOptimizer.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using Dwares.Druid.UI;
using Dwares.Dwarf.Toolkit;

namespace RouteOptimizer.ViewModels
{
	[QueryProperty("QueryPlace", "place")]
	public class PlaceEditViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(PlaceEditViewModel));

		public PlaceEditViewModel()
		{
			//Debug.EnableTracing(@class);

			//Title = "Place";

			SaveCommand = new Command(Save);
		}

		public Command SaveCommand { get; }

		public Place Source { get; private set; }

		public string Name {
			get => name;
			set => SetProperty(ref name, value);
		}
		string name = string.Empty;

		public string Tags {
			get => tags;
			set => SetProperty(ref tags, value);
		}
		string tags = string.Empty;

		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address = string.Empty;

		public string Phone {
			get => phone;
			set => SetProperty(ref phone, value);
		}
		string phone = string.Empty;

		public List<string> SuggestedTags {
			get => suggestedTags ??= KnownTags.GetTagsListForType(typeof(Place));
		}
		List<string> suggestedTags;

		public object ChoosenTagSuggestion {
			get => choosenTagSuggestion;
			set => SetProperty(ref choosenTagSuggestion, value); // need this to set IsModified
		}
		object choosenTagSuggestion;

		public string QueryPlace {
			set {
				try {
					int index = int.Parse(value);
					var place = App.Current.Places.At(index);
					if (place != null) {
						Source = place;
						Name = place.Name;
						Tags = place.Tags;
						Address = place.Address;
						IsModified = false;
						return;
					}
					Debug.Fail($"## PlaceEditViewModel.QueryPlace(): Place #{index} not found");
				}
				catch (Exception exc) {
					Debug.ExceptionCaught(exc);
				}
				Source = null;
				Name = Tags = Address = string.Empty;
			}
		}
		public async Task<bool> CanGoBack()
		{
			if (IsModified) {
				bool proceed = await Alerts.ConfirmAlert("Place is not saved.\nDo you want to leave without saving?");
				if (!proceed)
					return false;
			}

			return true;
		}

		public async void Save()
		{
			try {
				string message = Validate(Name, Address, Phone);
				if (message != null) {
					await Alerts.DisplayAlert(null, message);
					return;
				}

				if (Source == null) {
					var newPlace = new Place
					{
						Name = Name,
						Tags = Tags,
						Address = Address,
						Phone = Phone
					};
					await App.Current.AddPlace(newPlace);
				} else {
					Source.Name = Name;
					Source.Tags = Tags;
					Source.Address = Address;
					Source.Phone = Phone;

					await App.Current.UpdatePlace(Source);
				}

				Source = null;
				Name = Tags = Address = string.Empty;
				ChoosenTagSuggestion = null;
				IsModified = false;

				await ShellPageEx.TryGoBack();
			}
			catch (Exception exc) {
				await Alerts.ErrorAlert(exc.Message);
			}

		}

		public static string Validate(string name, string address, string phone)
		{
			if (string.IsNullOrWhiteSpace(name))
				return "Please enter place name";

			if (string.IsNullOrWhiteSpace(address))
				return "Please enter address";

			if (App.Current.Places.GetByName(name) != null)
				return $"Place with name '{name}' already exists";

			return null;
		}
	}
}
