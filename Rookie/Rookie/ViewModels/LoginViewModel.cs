using System;
using System.Collections;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Druid.Satchel;
using Dwares.Rookie.Models;


namespace Dwares.Rookie.ViewModels
{
	public class LoginViewModel : FormViewModel
	{	
		public LoginViewModel()
		{
			Title = "Login";

			if (AppScope.Driver != null) {
				SelectedUser = AppScope.Instance.GetAccount(AppScope.Driver); 
			}
		}

		public override double FormHeight { get; set; } = FitContent;

		public IList Accounts => AppScope.Instance.Accounts;

		public bool HasUserSelected => SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.Username);

		Account selectedUser = null;
		public Account SelectedUser {
			get => selectedUser;
			set => SetPropertyEx(ref selectedUser, value, nameof(SelectedUser), nameof(HasUserSelected));
		}

		public string Password { get; set; }
		public bool KeepLoggedIn { get; set; }

		public bool CanLogin()
		{
			return !IsBusy && HasUserSelected;
		}

		public async Task OnLogin()
		{
			//Debug.Print("LoginViewModel.OnLogin");
			
			Exception error = null;
			try {
				StartBusy("Logging in...");
				PropertiesChanged(nameof(CanLogin));

				await AppScope.Instance.Login(SelectedUser.Username, Password, KeepLoggedIn);
			}
			catch (Exception exc) {
				error = exc;
			}
			finally {
				ClearBusy();
				PropertiesChanged(nameof(CanLogin));
			}

			if (error != null) {
				await Alerts.Error(error.Message);
				return;
			}

			var page = App.CreateForm<HomeViewModel>();
			await Navigator.ReplaceTopPage(page);
		}

		public bool CanGoToWork() => CanLogin();

		public async void OnGoToWork()
		{
			//Debug.Print("LoginViewModel.OnAddAccount");

			await OnLogin();

			if (AppScope.Instance.IsLoggedIn) {
				var page = App.CreateForm<GoToWorkViewModel>();
				await Navigator.PushPage(page);
			}
		}

		public async void OnAddAccount()
		{
			//Debug.Print("LoginViewModel.OnAddAccount");

			var page = App.CreateForm<AddAccountViewModel>();
			//await Navigator.PushModal(page);
			await Navigator.PushPage(page);
		}

		public override void UpdateCommands()
		{
			//Debug.Print("LoginViewModel.UpdateCommands");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "Login");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "GoOnDuty");
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "AddAccount");
		}

	}
}
