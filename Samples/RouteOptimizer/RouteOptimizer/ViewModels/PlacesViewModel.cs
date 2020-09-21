using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Druid.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using RouteOptimizer.Models;
using RouteOptimizer.Storage;
using System.ComponentModel;
using RouteOptimizer.Views;
using Dwares.Dwarf.Collections;
using System.Net.Http.Headers;
using System.Windows.Input;

namespace RouteOptimizer.ViewModels
{
	public class PlacesViewModel : CardCollectionViewModel<Place, PlaceCardModel>, 
		IActivatable, ISelectionHandler
	{
		//static ClassRef @class = new ClassRef(typeof(PlacesViewModel));

		public PlacesViewModel()
		{
			//Debug.EnableTracing(@class);

			Cards = new SortableShadowCollection<PlaceCardModel, Place>(App.Current.Places.List, DefaultFactory, null, SortOrder);
			Items = Cards;

			Title = "Places";
			
			HasPlaceholder = UseInPlaceEditor = App.Current.UseInPlaceEditor;
			App.Current.PropertyChanged += (s, e) => {
				if (e.PropertyName == nameof(App.UseInPlaceEditor)) {
					HasPlaceholder = UseInPlaceEditor = App.Current.UseInPlaceEditor;
				}
			};

			RefreshCommand = new Command(async () => await PullRefresh(), CanPerformAction);

			ExpandPanelCommand = new Command(() => IsPanelExpanded = !IsPanelExpanded );
			SearchCommand = new Command(Search);
		}

		public SortableShadowCollection<PlaceCardModel, Place> Cards { get; }

		public PlaceCardModel EditingCard { get; private set; }


		public Command RefreshCommand { get; }

		public Command ExpandPanelCommand { get; }
		public Command SearchCommand { get; }

		public bool IsRefreshing {
			get => isRefreshing;
			set => SetProperty(ref isRefreshing, value);
		}
		bool isRefreshing;

		public bool SortByCategory {
			get => sortByCategory;
			set {
				if (SetProperty(ref sortByCategory, value)) {
					Cards.SortOrder = SortOrder;
					FirePropertiesChanged(nameof(SortOrder));
				}
			}
		}
		bool sortByCategory = true;

		public bool SortDescending {
			get => sortDescending;
			set {
				if (SetProperty(ref sortDescending, value)) {
					Cards.SortOrder = SortOrder;
					FirePropertiesChanged(nameof(SortOrder));
				}
			}
		}
		bool sortDescending;

		public Comparison<Place> SortOrder {
			get {
				Comparison<Place> order;
				if (SortByCategory) {
					order = Place.CompareByCategory;
				} else {
					order = Place.CompareByNameOnly;
				}
				if (SortDescending) {
					return (p1,p2) => order(p2,p1);
				} else {
					return order;
				}
			}
		}

		public bool UseInPlaceEditor {
			get => useInPlaceEditor;
			set {
				if (SetPropertyEx(ref useInPlaceEditor, value, nameof(UseInPlaceEditor), nameof(NotInPlaceEditor))) {
					HasPlaceholder = value;
				}
			}
		}
		bool useInPlaceEditor;

		public bool NotInPlaceEditor {
			get => !UseInPlaceEditor;
			set => UseInPlaceEditor = !value;
		}

		public bool IsPanelExpanded {
			get => isPanelExpanded;
			set {
				SetPropertyEx(ref isPanelExpanded, value, nameof(IsPanelExpanded), nameof(ExpandPanelIcon));
			}
		}
		bool isPanelExpanded = false;

		public string ExpandPanelIcon {
			get => IsPanelExpanded ? "ic_expand_less" : "ic_expand_more";
		}

		public string SearchText {
			get => searchText;
			set {
				if (SetProperty(ref searchText, value))
					SearchResult = null;
			}
		}
		string searchText = string.Empty;

		public string SearchlIcon {
			get => SearchResult?.Card  != null ? "ic_search_again" : "ic_search";
		}

		internal SearchResult? SearchResult {
			get => searchResult;
			set => SetPropertyEx(ref searchResult, value, nameof(SearchResult), nameof(SearchlIcon));
		}
		SearchResult? searchResult;

		public bool IsPullToRefreshEnabled {
			get => isPullToRefreshEnabled;
			set => SetProperty(ref isPullToRefreshEnabled, value);
		}
		bool isPullToRefreshEnabled;

		async Task PullRefresh()
		{
			if (IsPullToRefreshEnabled) {
				try {
					await App.Current.LoadPlaces();
				} catch (Exception exc) {
					Debug.ExceptionCaught(exc);
				} finally {
					IsRefreshing = false;
				}
			} else {
				Debug.Fail("PlaceViewModel.PullRefresh() called with IsPullToRefreshEnabled=true");
				//await BusyTask(async () => {
				//	await App.Current.LoadPlaces();
				//});
			}
		}

		public async Task ExecuteAdd()
		{
			await AddCard();
		}

		public async Task AddCard()
		{
			if (UseInPlaceEditor)
			{
				if (EditingCard != null)
					return;

				SelectedItem = null;
				HasPlaceholder = false;

				var card = new PlaceCardModel(NewCard._);
				card.StartEditing();
				Items.Add(card);

				SelectedItem = EditingCard = card;
				WantToScrollTo = card;
			}
			else {
				await Shell.Current.GoToAsync($"PlaceEditPage");
			}
		}

		public async Task ExecuteDelete()
		{
			if (SelectedItem != null) {
				await DeleteCard(SelectedItem);
			}
		}

		public async Task DeleteCard(PlaceCardModel card)
		{
			if (await Alerts.ConfirmAlert($"Are you sure you want to delete '{card.Name}' ?")) {
				await App.Current.DeletePlace(card.Source);
			}
		}

		public async Task ExecuteEdit()
		{
			if (SelectedItem != null) {
				await EditCard(SelectedItem);
			}
		}

		public async Task EditCard(PlaceCardModel card)
		{
			if (UseInPlaceEditor)
			{
				if (EditingCard != null)
					return;

				HasPlaceholder = false;

				card.StartEditing();
				SelectedItem = EditingCard = card;
			}
			else {
				int index = App.Current.Places.IndexOf(card.Source);
				if (index < 0) {
					Debug.Fail($"## PlaceViewModel source of place '{card.Name}' not found");
					return;
				}

				await Shell.Current.GoToAsync($"PlaceEditPage?place={index}");
			}
		}

		public async Task ExecuteSave()
		{
			await EndEditing(true);
		}

		public async Task ExecuteCancelEditing()
		{
			await EndEditing(false);
		}

		public async Task EndEditing(bool save)
		{
			if (EditingCard == null)
				return;

			if (await EditingCard.StopEditing(save))
			{
				IsBusy = true;

				try {
					if (EditingCard.IsNewCard) {
						Items.Remove(EditingCard);
					}

					if (save) {
						if (EditingCard.IsNewCard) {
							await App.Current.AddPlace(EditingCard.Source);
						} else {
							await App.Current.UpdatePlace(EditingCard.Source);
						}
					}
				} catch (Exception exc) {
					Debug.ExceptionCaught(exc);
					await Alerts.ExceptionAlert(exc);
				}
				finally {
					IsBusy = false;
					EditingCard = null;
					HasPlaceholder = UseInPlaceEditor;
				}
			}
		}
		public void OnSelectedChanged(ref object selectedItem, int selectedIndex)
		{
			if (EditingCard != null) {
				selectedItem = EditingCard;
			}
		}

		public static PlacesViewModel ActiveModel { get; private set; }

		public bool IsActive {
			get => ActiveModel == this;
		}

		public void Activate()
		{
			ActiveModel = this;
		}

		public void Deactivate()
		{
			if (IsActive) {
				ActiveModel = null;
			}
		}

		//async Task Clear()
		//{
		//	if (await Alerts.ConfirmAlert("Are you sure you want to clear the whole list?")) {
		//		await App.Current.ClearPlaces();
		//	}
		//}

		public async Task ExecuteClear()
		{
			if (await Alerts.ConfirmAlert("Are you sure you want to clear the whole list?"))
			{
				await this.BusyTask(async () => {
					await App.Current.ClearPlaces();
				});
			}
		}

		public async Task ExecuteEmail()
		{
			await this.BusyTask(async () => {
				var json = JsonStorage.SerializePlaces(Items.Source);

				var message = new EmailMessage {
					Subject = string.Empty, //"RouteOptimizer sharing places",
					Body = string.Empty,
					BodyFormat = EmailBodyFormat.PlainText,
					To = null, //new List<string> { "someone@somewhere" },
					Cc = null,
					Bcc = null,
				};

				// create a temprary file
				var path = Path.Combine(FileSystem.CacheDirectory, "Places.txt");
				File.WriteAllText(path, json);

				message.Attachments.Add(new EmailAttachment(path));

				await Xamarin.Essentials.Email.ComposeAsync(message);
			});
		}

		public async Task ExecuteShare()
		{
			await this.BusyTask(async () => {
				var json = JsonStorage.SerializePlaces(Items.Source);

				// create a temprary file
				var path = Path.Combine(FileSystem.CacheDirectory, "Places.txt");
				File.WriteAllText(path, json);

				//var bounds = element.GetAbsoluteBounds();

				await Xamarin.Essentials.Share.RequestAsync(new ShareFileRequest {
					Title = Title,
					File = new ShareFile(path)
					//,PresentationSourceBounds = bounds.ToSystemRectangle() // for iOS only
				});
			});
		}

		//public async Task ExecuteLoadSample()
		//{
		//	await BusyTask(async () => {
		//		await App.Current.LoadSample(App.Phila_Ru_Sample, skipDuplicates: true);
		//	});
		//}

		public async Task ExecuteLoadSample()
		{
			await this.BusyTaskOnMainThread(async () => {
				await App.Current.LoadSample(App.Phila_Ru_Sample, skipDuplicates: true);
			},
			"Loading"
			);
		}

		void Search()
		{
			if (string.IsNullOrEmpty(SearchText))
				return;

			var text = SearchText.ToLower();
			int startIndex = 0;
			if (SearchResult != null && SearchResult?.Text == text) {
				startIndex = ((SearchResult)SearchResult).Index + 1;
			}
			
			SearchResult? result = null;
			for (int i = startIndex; i < Items.Count; i++) {
				var card = Items[i];
				if (card == null)
					continue;

				var name = card.Name.ToLower();
				if (name.Contains(text)) {
					result = new SearchResult {
						Text = text,
						Card = card,
						Index = i
					};
					break;
				}
			}

			if (result != null) {
				SearchResult = result;
				WantToScrollTo = result?.Card;
			} else {
				if (SelectedItem == SearchResult?.Card)
					SelectedItem = null;
				SearchResult = null;
			}
		}


		public PlaceCardModel WantToScrollTo {
			get => wantToScrollTo;
			set {
				wantToScrollTo = value;
				if (value != null) {
					FirePropertyChanged();
				}
			}
		}
		PlaceCardModel wantToScrollTo;

		public string WantToScrollToId {
			get => WantToScrollTo?.Id;
			set => WantToScrollTo = FindCardById(value);
		}

		PlaceCardModel FindCardById(string id)
		{
			return Items.FirstOrDefault((card) => card.Id == id);
		}
	}

	internal struct SearchResult
	{
		public string Text;
		public PlaceCardModel Card;
		public int Index;

	}
}
