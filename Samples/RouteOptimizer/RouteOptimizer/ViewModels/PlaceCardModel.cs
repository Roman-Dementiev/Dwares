﻿using System;
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
			
			AddCommand = new Command(async () => await PlacesViewModel.ActiveModel?.AddCard(), CanPerformAction);
			DeleteCommand = new Command(async (param) => await PlacesViewModel.ActiveModel?.DeleteCard(param as PlaceCardModel), CanPerformAction);
			EditCommand = new Command(async (param) => await PlacesViewModel.ActiveModel?.EditCard(param as PlaceCardModel), CanPerformAction);
			SaveCommand = new Command(async () => await PlacesViewModel.ActiveModel?.EndEditing(true), CanPerformAction);
			CancelCommand = new Command(async () => await PlacesViewModel.ActiveModel?.EndEditing(false), CanPerformAction);
		}

		public PlaceCardModel(NewCard newCard) :
			this()
		{
			//Debug.EnableTracing(@class);
			Source = new Place();
			IsNewCard = true;

		}

		public Command AddCommand { get; }
		public Command DeleteCommand { get; }
		public Command EditCommand { get; }
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }

		public bool CanPerformAction() {
			return PlacesViewModel.ActiveModel?.CanPerformAction() == true;
		}
		public bool CanPerformAction(object param) => CanPerformAction();

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

		public string Phone {
			get => Source?.Phone ?? string.Empty;
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

		public string EditPhone {
			get => editPhone;
			set => SetProperty(ref editPhone, value);
		}
		static string editPhone = string.Empty;

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
			EditPhone = Source.Phone;

			IsEditing = true;
		}

		public async Task<bool> StopEditing(bool save)
		{
			if (save) {
				var message = PlaceEditViewModel.Validate(EditName, EditAddress, EditPhone);
				if (message != null) {
					await Alerts.DisplayAlert(null, message);
					return false;
				}

				Source.Name = EditName.Trim();
				Source.Tags = EditTags.Trim();
				Source.Address = EditAddress.Trim();
				Source.Phone = EditPhone.Trim();
			}

			EditName = EditTags = EditAddress = string.Empty;
			IsEditing = false;

			return true;
		}

		protected override void OnSourceChanged(object sender, ModelChangedEventArgs e)
		{
			FirePropertiesChanged(e.ChangedProperties);

			//if (e.ChangedProperties.Contains(nameof(Tags)) || e.JustAdded) {
			//	FirePropertyChanged(nameof(ShowTags));
			// e.JustAdded = false;
			//}
		}
	}
}
