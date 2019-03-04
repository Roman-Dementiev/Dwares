using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;


namespace Dwares.Rookie.ViewModels
{
	public class HomeViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(MainViewModel));

		public HomeViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Rookie";
			AppScope.Instance.PropertyChanged += OnAppScopePropertyChanged;
		}

		public bool IsWorking => AppScope.Instance.IsWorking;
		public bool NotWorking => !AppScope.Instance.IsWorking;
		public bool HasWorkPeriod => AppScope.Instance.LastPeriod != null;

		private void OnAppScopePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(AppScope.IsWorking)) {
				PropertiesChanged(nameof(IsWorking), nameof(NotWorking));
			}
			else if (e.PropertyName == nameof(AppScope.LastPeriod)) {
				PropertiesChanged(nameof(HasWorkPeriod));
			}
		}

		public async void OnGoToWork()
		{
			bool addDatabase = false;
			if (AppScope.Instance.TripBase == null) {
				//await Alerts.Error("There is no database for current month/year");
				addDatabase = await Alerts.ActionAlert(null, "There is no database for current month/year", "Add Database");
				if (!addDatabase)
					return;
			}

			Page page;
			if (addDatabase) {
				page = Forge.CreatePage(typeof(AddBaseViewModel));
			} else {
				page = App.CreateForm<GoToWorkViewModel>();
			}
			await Navigator.PushPage(page);
		}

		public async void OnGoOffWork()
		{
			var page = App.CreateForm<GoOffWorkViewModel>();
			await Navigator.PushPage(page);
		}

		public async void OnLogout()
		{
			await AppScope.Instance.Logout();

			var page = App.CreateForm<LoginViewModel>();
			await Navigator.ReplaceTopPage(page);
		}

		public async void OnAddTrip()
		{
			var page = App.CreateForm<AddTripViewModel>();
			await Navigator.PushPage(page);
		}

		public async void OnSetupBases()
		{
			var page = App.CreateForm<BasesViewModel>();
			await Navigator.PushPage(page);
		}
	}
}
