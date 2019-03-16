using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Rookie.Views;


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
				page = App.CreateForm<AddBaseViewModel>();
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
			if (NotWorking) {
				await Alerts.Error("You have to Go to Work to enter trips.");
				return;
			}

			var page = App.CreateForm<AddTripViewModel>();
			await Navigator.PushPage(page);
		}

		public async void OnExpenses()
		{
			if (AppScope.Instance.LastPeriod == null) {
				await Alerts.Error("There is no any work period.\n Yo need to Go to Work first.");
				return;
			}

			var page = App.CreatePage<ExpensesView>();
			await Navigator.PushPage(page);
		}

		//public async void OnGas()
		//{
		//	var page = ExpensesViewModel.CreatePage(true);
		//	await Navigator.PushPage(page);
		//}

		public async void OnSetupBases()
		{
			var page = App.CreateForm<BasesViewModel>();
			await Navigator.PushPage(page);
		}

		public async void OnSetup()
		{
			var page = App.CreatePage<SetupView>();
			await Navigator.PushPage(page);
		}

	}
}
