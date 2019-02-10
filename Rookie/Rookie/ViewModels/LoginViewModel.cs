using System;
using System.Collections;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Rookie.Models;
using Dwares.Rookie.Views;


namespace Dwares.Rookie.ViewModels
{
	public class LoginViewModel : ViewModel
	{	
		public LoginViewModel()
		{
		}	

		public IList Accounts => AppData.Accounts;

		public bool HasUserSelected => SelectedUser != null;

		Account selectedUser = null;
		public Account SelectedUser {
			get => selectedUser;
			set => SetPropertyEx(ref selectedUser, value, nameof(SelectedUser), nameof(HasUserSelected));
		}

		public string Password { get; set; }

		public bool CanLogin()
		{
			return SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.Username);
		}

		public async void OnLogin()
		{
			Debug.Print("LoginViewModel.OnLogin");

			var exc = await AppData.Instance.Login(SelectedUser.Username, Password);
			if (exc != null) {
				await Alerts.Error(exc.Message);
				return;
			}

			//var page = CreatePage(typeof(BasesViewModel));
			var page = new BasesPage();
			await Navigator.PushPage(page);
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
			WritMessage.Send(this, WritMessage.WritCanExecuteChanged, "AddAccount");
		}
	}
}
