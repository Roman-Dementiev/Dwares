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

namespace RouteOptimizer.ViewModels
{
	public class PlacesViewModel : CardCollectionViewModel<Place, PlaceCardModel>, 
		IActivatable, ISelectionHandler
	{
		//static ClassRef @class = new ClassRef(typeof(PlacesViewModel));

		public PlacesViewModel() :
			base(App.Current.Places.List)
		{
			//Debug.EnableTracing(@class);
			Title = "Places";
			
			HasPlaceholder = UseInPlaceEditor = App.Current.UseInPlaceEditor;
			App.Current.PropertyChanged += (s, e) => {
				if (e.PropertyName == nameof(App.UseInPlaceEditor)) {
					HasPlaceholder = UseInPlaceEditor = App.Current.UseInPlaceEditor;
				}
			};

			AddCommand = new Command(async () => await AddCard(), CanPerformAction);
			DeleteCommand = new Command(async (param) => await DeleteCard(param as PlaceCardModel), CanPerformAction);
			EditCommand = new Command(async (param) => await EditCard(param as PlaceCardModel), CanPerformAction);
			SaveCommand = new Command(async () => await EndEditing(true), CanPerformAction);
			CancelCommand = new Command(async () => await EndEditing(false), CanPerformAction);

			RefreshCommand = new Command(async () => await BusyTask(Reload), CanPerformAction);
			ClearCommand = new Command(async () => await BusyTask(Clear), CanPerformAction);
			EmailCommand = new Command(async () => await BusyTask(Email), CanPerformAction);
			ShareCommand = new Command(async () => await BusyTask(Share), CanPerformAction);
			LoadSampleCommand = new Command(async () => await BusyTask(LoadSample), CanPerformAction);
		}

		public ObservableCollection<PlaceCardModel> Places => Items;

		public PlaceCardModel EditingCard { get; private set; }

		//public bool CanPerformAction() => IsNotBusy;
		//public bool CanPerformAction(object param) => IsNotBusy;

		public Command AddCommand { get; }
		public Command DeleteCommand { get; }
		public Command EditCommand { get; }
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }

		public Command RefreshCommand { get; }
		public Command ClearCommand { get; }
		public Command EmailCommand { get; }
		public Command ShareCommand { get; }
		public Command LoadSampleCommand { get; }

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


		async Task Reload()
		{
			await App.Current.ReloadPlaces();
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
			}
			else {
				await Shell.Current.GoToAsync($"PlaceEditPage");
			}
		}

		public async Task DeleteCard(PlaceCardModel card)
		{
			if (await Alerts.ConfirmAlert($"Are you sure you want to delete '{card.Name}' ?")) {
				await App.Current.DeletePlace(card.Source);
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

		async Task Clear()
		{
			if (await Alerts.ConfirmAlert("Are you sure you want to clear the whole list?")) {
				await App.Current.ClearPlaces();
			}
		}
		
		async Task Email()
		{
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
		}

		async Task Share()
		{
			var json = JsonStorage.SerializePlaces(Items.Source);

			// create a temprary file
			var path = Path.Combine(FileSystem.CacheDirectory, "Places.txt");
			File.WriteAllText(path, json);

			//var bounds = element.GetAbsoluteBounds();

			await Xamarin.Essentials.Share.RequestAsync(new ShareFileRequest
			{
				Title = Title,
				File = new ShareFile(path)
				//,PresentationSourceBounds = bounds.ToSystemRectangle() // for iOS only
			});
		}

		async Task LoadSample()
		{
			Items.IsSuspended = true;
			await App.Current.LoadSample(App.HospitalsSample, skipDuplicates: true);
			Items.IsSuspended = false;
		}
	}
}
