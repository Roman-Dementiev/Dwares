using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Dwares.Druid.Support;
using ACE.Models;
using ACE.Views;


namespace ACE.ViewModels
{
	public class PickupsListViewModel: CollectionViewModel<Pickup>
	{
		public PickupsListViewModel() :
			base(AppScope, AppData.Pickups)
		{
			//AboutWrit = new WritCommand("About", this);
			////AboutCommand = new Command(OnAbout);
			//AboutCommand = AboutWrit;

			AddCommand = new Command(OnAdd);
			EditCommand = new Command(OnEdit, HasSelected);
			DeleteCommand = new Command(OnDelete, HasSelected);
		}

		public ObservableCollection<Pickup> Pickups => Items;

		public Command AddCommand { get; }
		public Command EditCommand { get; }
		public Command DeleteCommand { get; }

		public async void OnAdd() => await AddOrEdit(null);

		public async void OnEdit() => await AddOrEdit(Selected);

		private Task AddOrEdit(Pickup pickup)
		{
			var page = new PickupDetailPage(pickup);
			return Navigator.PushModal(page);
		}

		public async void OnDelete()
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
