using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Rookie.Models;


namespace Dwares.Rookie.ViewModels
{
	public class NewUserViewModel : ViewModel
	{
		public NewUserViewModel() { }

		public string Username { get; set; }
		public string Password { get; set; }
		public string ApiKey { get; set; }

		public async void OnLogin()
		{
			//var user = new KnownUser(Username, Password, ApiKey);
			//AppData.AddUser(user);

			//await Navigator.PopPage();
		}

		public async void OnCancel()
		{
			await Navigator.PopPage();
		}
	}
}
