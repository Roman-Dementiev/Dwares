using System;
using System.Linq;
using Xamarin.Forms;
using Dwares.Dwarf;
using RouteOptimizer.Models;
using System.Threading.Tasks;
using Dwares.Druid;
using System.Collections.Generic;

namespace RouteOptimizer.ViewModels
{
	public class PlaceCardModel : CardViewModel<Place>
	{
		//static ClassRef @class = new ClassRef(typeof(PlaceCardModel));

		public PlaceCardModel()
		{
			//Debug.EnableTracing(@class);
		}

		public PlaceCardModel(Place source) : 
			base(source)
		{
			//Debug.EnableTracing(@class);
		}

		public PlaceCardModel(NewCard newCard)
		{
			//Debug.EnableTracing(@class);
			Source = new Place();
			IsNewCard = true;
		}

		public Command AddCommand => PlacesViewModel._AddCommand;
		public Command DeleteCommand => PlacesViewModel._DeleteCommand;
		public Command EditCommand => PlacesViewModel._EditCommand;
		public Command SaveCommand => PlacesViewModel._SaveCommand;
		public Command CancelCommand => PlacesViewModel._CancelCommand;

		public object Self => this;

		public string Id {
			get => Source?.Id ?? string.Empty;
		}

		public string Icon {
			get => Source?.Icon ?? string.Empty;
		}

		public string Name {
			get => Source?.Name ?? string.Empty;
		}
		
		public string Tags {
			get => Source?.Tags ?? string.Empty;
		}

		//public bool ShowTags {
		//	get => !string.IsNullOrEmpty(Tags);
		//}
		public bool ShowTags => false;

		public string Address {
			get => Source?.Address ?? string.Empty;
		}

		public string EditName {
			get => editName;
			set => SetProperty(ref editName, value);
		}
		static string editName = string.Empty;

		public string EditTags {
			get => editTags;
			set => SetProperty(ref editTags, value);
		}
		static string editTags = string.Empty;

		public string EditAddress {
			get => editAddress;
			set => SetProperty(ref editAddress, value);
		}
		static string editAddress = string.Empty;

		public List<string> SuggestedTags {
			get => suggestedTags ??= KnownTags.GetTagsListForType(typeof(Place));
		}
		List<string> suggestedTags;

		public object ChoosenTagSuggestion {
			get => choosenTagSuggestion;
			set {
				choosenTagSuggestion = value;

				//BUG: ??? It gains focus again and reopens suggestion list 
				//IsSuggestionListOpen = false;

				//FIX: Workaround to close  suggestion list 
				if (choosenTagSuggestion != null) {
					EditTags = choosenTagSuggestion.ToString() + " ";
				}
			}
		}
		object choosenTagSuggestion;

		public void StartEditing()
		{
			EditName = Source.Name;
			EditTags = Source.Tags;
			EditAddress = Source.Address;

			IsEditing = true;
		}

		public async Task<bool> StopEditing(bool save)
		{
			if (save) {
				if (string.IsNullOrWhiteSpace(EditName)) {
					await Alerts.DisplayAlert(null, "Please enter place name");
					return false;
				}
				if (string.IsNullOrWhiteSpace(EditAddress)) {
					await Alerts.DisplayAlert(null, "Please enter place address");
					return false;
				}
				if (App.Current.Places.GetByName(EditName) != null) {
					await Alerts.DisplayAlert(null, $"Place with name '{EditName}' already exists");
					return false;
				}

				Source.Name = EditName.Trim();
				Source.Tags = EditTags.Trim();
				Source.Address = EditAddress.Trim();
			}

			EditName = EditTags = EditAddress = string.Empty;
			IsEditing = false;

			return true;
		}

		protected override void OnSourceChanged(object sender, ModelChangedEventArgs e)
		{
			FirePropertiesChanged(e.ChangedProperties);

			//if (e.ChangedProperties.Contains(nameof(Tags))) {
			//	FirePropertyChanged(nameof(ShowTags));
			//}
		}
	}
}
