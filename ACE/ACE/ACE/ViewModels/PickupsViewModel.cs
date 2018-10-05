using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ACE.Models;
using ACE.Views;
using System.Threading.Tasks;

namespace ACE.ViewModels
{
	public class PickupsViewModel: CollectionViewModel<Pickup>
	{
		public PickupsViewModel(INavigation navigation) :
			base(navigation, AppData.Pickups)
		{
			AddCommand = new Command(OnAdd);
			EditCommand = new Command(OnEdit, HasSelected);
			DeleteCommand = new Command(OnDelete, HasSelected);
		}

		public ObservableCollection<Pickup> Pickups => Items;

		public Command AddCommand { get; }
		public Command EditCommand { get; }
		public Command DeleteCommand { get; }

		private async void OnAdd() => await AddOrEdit(null);

		private async void OnEdit() => await AddOrEdit(Selected);

		private Task AddOrEdit(Pickup pickup)
		{
			var page = new PickupDetailPage(pickup);
			return Navigation.PushModalAsync(new NavigationPage(page));
		}

		private async void OnDelete()
		{
			await AppData.RemovePickup(Selected);
		}

		protected override void OnSelectedItemChanged()
		{
			base.OnSelectedItemChanged();

			EditCommand.ChangeCanExecute();
			DeleteCommand.ChangeCanExecute();
		}
	}
}
