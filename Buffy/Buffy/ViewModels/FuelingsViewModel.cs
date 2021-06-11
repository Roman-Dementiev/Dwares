using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Dwares.Druid.ViewModels;
using Dwares.Dwarf;
using Buffy.Models;
using Buffy.Views;
using Xamarin.Forms;


namespace Buffy.ViewModels
{
	public class FuelingsViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(FuelingsViewModel));

		public ObservableCollection<FuelingCell> Fuelings { get; }

		public Command AddCommand { get; }
		public Command SyncCommand { get; }
		public Command<FuelingCell> CellTapped { get; }

		public FuelingsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Browse";

			Fuelings = new ObservableCollection<FuelingCell>();

			AddCommand = new Command(OnAdd);
			SyncCommand = new Command(async () => await ExecuteSyncCommand());

			CellTapped = new Command<FuelingCell>(OnItemSelected);

			CreateCells(false);
		}

		void CreateCells(bool reload)
		{
			if (reload) {
				App.Fuelings.CollectionChanged -= Fuelings_CollectionChanged;
				Fuelings.Clear();
			}
			foreach (var fueling in App.Fuelings) {
				Fuelings.Add(new FuelingCell(fueling));
			}

			App.Fuelings.CollectionChanged += Fuelings_CollectionChanged;
		}

		private void Fuelings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add) {
				foreach (var item in e.NewItems) {
					if (item is Fueling fueling) {
						Fuelings.Add(new FuelingCell(fueling));
					} else {
						Debug.Print($"Unknown item in Fueligs list: {item.GetType()}");
					}
				}
			} else {
				CreateCells(true);
			}
		}

		private async void OnAdd()
		{
			await Shell.Current.GoToAsync($"{nameof(FuelingForm)}?id=new");
		}

		async Task ExecuteSyncCommand()
		{
			IsBusy = true;

			try {
				//Items.Clear();
				//var items = await DataStore.GetItemsAsync(true);
				//foreach (var item in items) {
				//	Items.Add(item);
				//}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			finally {
				IsBusy = false;
			}
		}

		async void OnItemSelected(FuelingCell cell)
		{
			if (cell == null)
				return;

			await Shell.Current.GoToAsync($"{nameof(FuelingForm)}?id={cell.Fueling.Id}");
		}
	}
}
