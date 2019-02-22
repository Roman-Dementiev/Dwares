using System;
using System.ComponentModel;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Dwares.Rookie.ViewModels
{
	public class MainPageViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(MainPageViewModel));

		public MainPageViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Rookie";

			Rookie.AppScope.Instance.PropertyChanged += AppDataPropertyChanged;
		}


		public bool IsWorking => AppScope.Instance.IsWorking;
		public bool NotWorking => !AppScope.Instance.IsWorking;
		public bool HasWorkPeriod => AppScope.Instance.LastPeriod != null;

		private void AppDataPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(AppScope.IsWorking)) {
				PropertiesChanged(nameof(IsWorking), nameof(NotWorking));
			}
			else if (e.PropertyName == nameof(AppScope.LastPeriod)) {
				PropertiesChanged(nameof(HasWorkPeriod));
			}
		}

		//public bool CanGoOnDuty() => AppData.TripBase != null;

		public async void OnGoToWork()
		{
			if (AppScope.Instance.TripBase == null) {
				await Alerts.Error("There is no database for current month/year");
				return;
			}

			var page = CreatePage(typeof(GoToWorkViewModel));
			await Navigator.PushPage(page);
		}

		public async void OnGoOffWork()
		{
			var page = CreatePage(typeof(GoOffWorkViewModel));
			await Navigator.PushPage(page);
		}

		public async void OnLogout()
		{
			await Rookie.AppScope.Instance.Logout();

			var page = CreatePage(typeof(LoginViewModel));
			await Navigator.ReplaceTopPage(page);
		}

		public async void OnAddTrip()
		{
			Debug.Print("MainPageViewModel.OnAddTrip");

			var page = CreatePage(typeof(AddTripViewModel));
			await Navigator.PushPage(page);
		}

		public async void OnSetupBases()
		{
			Debug.Print("MainPageViewModel.OnSetupBases");

			var page = CreatePage(typeof(BasesViewModel));
			await Navigator.PushPage(page);
		}
	}
}
