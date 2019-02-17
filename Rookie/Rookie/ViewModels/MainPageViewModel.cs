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


		public bool IsWorking => Rookie.AppScope.Instance.WorkPeriod != null;
		public bool NotWorking => Rookie.AppScope.Instance.WorkPeriod == null;

		private void AppDataPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Rookie.AppScope.WorkPeriod)) {
				PropertiesChanged(nameof(IsWorking), nameof(NotWorking));
			}
		}

		//public bool CanGoOnDuty() => AppData.TripBase != null;

		public async void OnGoToWork()
		{
			if (Rookie.AppScope.Instance.TripBase == null) {
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

		public void OnAddTrip()
		{
			Debug.Print("MainPageViewModel.OnAddTrip");
		}

		public async void OnSetupBases()
		{
			Debug.Print("MainPageViewModel.OnSetupBases");

			var page = CreatePage(typeof(BasesViewModel));
			await Navigator.PushPage(page);
		}
	}
}
