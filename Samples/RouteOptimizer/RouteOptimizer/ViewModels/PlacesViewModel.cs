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


namespace RouteOptimizer.ViewModels
{
	public class PlacesViewModel : CardCollectionViewModel<Place, PlaceCardModel>, 
		IActivatable, ISelectionHandler
	{
		//static ClassRef @class = new ClassRef(typeof(PlacesViewModel));

		const bool UsePlaceholde = false;

		public PlacesViewModel() :
			base(App.Current.Places.List)
		{
				//Debug.EnableTracing(@class);

			Title = "Places";
			HasPlaceholder = UsePlaceholde;

			RefreshCommand = new Command(Reload);
			ClearCommand = new Command(async () => await BusyTask(Clear));
			EmailCommand = new Command(async () => await BusyTask(Email));
			ShareCommand = new Command(async () => await BusyTask(Share));
			LoadSampleCommand = new Command(async () => await BusyTask(LoadSample));
		}

		public ObservableCollection<PlaceCardModel> Places => Items;

		public PlaceCardModel EditingCard { get; private set; }

		public Command AddCommand => _AddCommand;
		public Command RefreshCommand { get; }
		public Command ClearCommand { get; }
		public Command EmailCommand { get; }
		public Command ShareCommand { get; }
		public Command LoadSampleCommand { get; }

		async void Reload()
		{
			await BusyTask(async () => await App.Current.ReloadPlaces());
		}

		void AddCard()
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

		async void DeleteCard(PlaceCardModel card)
		{
			await App.Current.DeletePlace(card.Source);
			
		}

		void EditCard(PlaceCardModel card)
		{
			if (EditingCard != null)
				return;

			HasPlaceholder = false;

			card.StartEditing();
			SelectedItem = EditingCard = card;
		}

		async void EndEditing(bool save)
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
					HasPlaceholder = UsePlaceholde;
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
			await App.Current.ClearPlaces();
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

		async Task LoadSample() => await LoadSample(skipDuplicates: true);

		async Task LoadSample(bool skipDuplicates)
		{
			using (var stream = await FileSystem.OpenAppPackageFileAsync("Places.txt"))
			using (var reader = new StreamReader(stream)) {
				var text = await reader.ReadToEndAsync();
				var json = JsonStorage.DeserializeJson<PlacesJson>(text);

				var places = App.Current.Places;

				foreach (var rec in json.Places) {
					var found = places.GetByName(rec.Name);
					if (found != null) {
						if (skipDuplicates)
							continue;
						places.Remove(found);
					}
					places.Add(new Place {
						Name = rec.Name,
						Tags = rec.Tags,
						Address = rec.Address
					});
				}
			}

			await App.Current.SavePlaces();
		}


		public static Command _AddCommand {
			get => addCommand ??= new Command(() => ActiveModel?.AddCard());
		}
		static Command addCommand;

		public static Command _DeleteCommand {
			get => deleteCommand ??= new Command((param) => ActiveModel?.DeleteCard(param as PlaceCardModel));
		}
		static Command deleteCommand;

		public static Command _EditCommand {
			get => editCommand ??= new Command((param) => ActiveModel?.EditCard(param as PlaceCardModel));
		}
		static Command editCommand;

		public static Command _SaveCommand {
			get => saveCommand ??= new Command(() => ActiveModel?.EndEditing(true));
		}
		static Command saveCommand;

		public static Command _CancelCommand {
			get => cancelCommand ??= new Command(() => ActiveModel?.EndEditing(false));
		}
		static Command cancelCommand;

	}
}
