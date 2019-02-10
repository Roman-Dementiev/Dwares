using System;
using Dwares.Dwarf;


namespace Dwares.Rookie.Models
{
	public class Account
	{
		//static ClassRef @class = new ClassRef(typeof(Account));

		public const int FreeAccount = 0;
		public const int PaidAccount = 1;

		public Account()
		{
			//Debug.EnableTracing(@class);
		}

		public Account(int accountType, string username, string password, string apiKey, string baseId)
		{
			//Debug.EnableTracing(@class);

			AccountType = accountType;
			Username = username;
			Password = password;
			ApiKey = apiKey;
			BaseId = baseId;
		}

		public int AccountType { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string ApiKey { get; set; }
		public string BaseId { get; set; }

		public string Encode()
		{
			return string.Format("{0}:{1}:{2}:{3}:{4}", Username, Password, ApiKey, BaseId, AccountType);
		}

		public bool Decode(string str)
		{
			if (!string.IsNullOrEmpty(str)) {
				var split = str.Split(new char[] { ':' });
				if (split != null && split.Length == 5) {
					Username = split[0];
					Password = split[1];
					ApiKey = split[2];
					BaseId = split[3];
					AccountType = split[4]=="0" ? FreeAccount : PaidAccount;
					return true;
				}
			}
			return false;
		}

		public override string ToString() => Username;
	}
}
