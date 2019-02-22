using System;
using System.Collections;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Rookie.Models;
//using Dwares.Rookie.Views;


namespace Dwares.Rookie.ViewModels
{
	public class LoginViewModel : FramedFormViewModel
	{	
		public LoginViewModel()
		{
			if (AppScope.Driver != null) {
				SelectedUser = AppScope.Instance.GetAccount(AppScope.Driver, null); 
			}
		}

		public override double FrameHeight => 240;

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

		public async Task<bool> OnLogin()
		{
			//Debug.Print("LoginViewModel.OnLogin");
			
			IsBusy = true;
			try {
				var exc = await AppScope.Instance.Login(SelectedUser.Username, Password, KeepLoggedIn);
				if (exc != null) {
					await Alerts.Error(exc.Message);
					return false;
				}

				var page = CreatePage(typeof(MainPageViewModel));
				await Navigator.ReplaceTopPage(page);
			}
			finally {
				IsBusy = false;
			}

			return true;
		}

		public bool CanGoToWork() => CanLogin();

		public async void OnGoToWork()
		{
			//Debug.Print("LoginViewModel.OnAddAccount");

			await OnLogin();

			if (AppScope.IsLoggedIn) {
				var page = CreatePage(typeof(GoToWorkViewModel));
				await Navigator.PushPage(page);
			}
		}

		public async void OnAddAccount()
		{
			//Debug.Print("LoginViewModel.OnAddAccount");

			var page = CreatePage(typeof(AddAccountViewModel));
			await Navigator.PushModal(page);
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
