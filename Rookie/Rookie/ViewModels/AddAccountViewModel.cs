using System;
using System.Threading.Tasks;
using Dwares.Dwarf.Validation;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Rookie.Models;


namespace Dwares.Rookie.ViewModels
{
	public class AddAccountViewModel : FramedFormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(AddAccountViewModel));

		public AddAccountViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Add Account";

			username = new Field<string>(true) { MsgFieldIsRequired = "Driver username is required" };
			password = new Field<string>();
			apiKey = new Field<string>(true) { MsgFieldIsRequired = "Api key is required" };
			baseId = new Field<string>(true) { MsgFieldIsRequired = "Base Id is required" };
			accountType = new Field<int>();

			fields = new Validatables(username, password, apiKey, baseId);
		}

		Field<string> username;
		public string Username {
			get => username;
			set => SetProperty(username, value);
		}

		Field<string> password;
		public string Password {
			get => password;
			set => SetProperty(password, value);
		}

		Field<string> apiKey;
		public string ApiKey {
			get => apiKey;
			set => SetProperty(apiKey, value);
		}

		Field<string> baseId;
		public string BaseId {
			get => baseId;
			set => SetProperty(baseId, value);
		}

		Field<int> accountType;
		public int AccountType {
			get => accountType;
			set => SetProperty(accountType, value);
		}

		protected override async Task<Exception> Validate()
		{
			var error = await base.Validate();
			if (error == null) {
				error = await Rookie.AppScope.Instance.TryLogin(Username, Password, apiKey, baseId, false);
			}
			return error;
		}

		protected override async Task DoAccept()
		{
			var account = new Account(AccountType, Username, Password, ApiKey, BaseId);
			await AppScope.Instance.AddAccount(account);
		}
	}
}
