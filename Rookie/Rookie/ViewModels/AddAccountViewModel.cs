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

		TextField username;
		TextField password;
		TextField apiKey;
		TextField baseId;
		Field<int> accountType;

		public AddAccountViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Add Account";

			username = new TextField("Driver username") { MsgFieldIsRequired = "Driver username is required" };
			password = new TextField("Password");
			apiKey = new TextField("Apt key") { IsRequired = true };
			baseId = new TextField("Base Id") { IsRequired = true };
			accountType = new Field<int>("Account type");

			Fields = new FieldList(username, password, apiKey, baseId, accountType);
		}

		public string Username {
			get => username;
			set => SetProperty(username, value);
		}

		public string Password {
			get => password;
			set => SetProperty(password, value);
		}

		public string ApiKey {
			get => apiKey;
			set => SetProperty(apiKey, value);
		}

		public string BaseId {
			get => baseId;
			set => SetProperty(baseId, value);
		}

		public int AccountType {
			get => accountType;
			set => SetProperty(accountType, value);
		}

		public override async Task<Exception> Validate()
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
