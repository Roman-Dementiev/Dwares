using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using Dwares.Rookie.Models;
using Dwares.Rookie.Data;
using Dwares.Rookie.Airtable;


namespace Dwares.Rookie
{
	public class AppData
	{
		const string keyAccounts = "Accounts";

		static AppData instance;
		public static AppData Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}

		public AppData()
		{
			//users.Add(new KnownUser { Username = "Roman", Password = "", ApiKey = "keyn9n03pU21UkxTg" });
		}


		MainBase mainBase;
		public static MainBase MainBase {
			get => Instance.mainBase;
			set => Instance.mainBase = value;
		}

		public static string ApiKey => MainBase?.ApiKey;

		ObservableCollection<TripBase> bases = new ObservableCollection<TripBase>();
		public static ObservableCollection<TripBase> Bases => Instance.bases;

		ObservableCollection<Account> accounts = new ObservableCollection<Account>();
		public static ObservableCollection<Account> Accounts => Instance.accounts;

		ObservableCollection<YearlyTripData> data = new ObservableCollection<YearlyTripData>();
		public static ObservableCollection<YearlyTripData> Data => Instance.data;

		public async Task Initialize(bool reset)
		{
			if (reset) {
				await ClearAccounts();
			} else {
				await InitAccounts();
			}
		}

		public static async Task ClearAccounts()
		{
			await SecureStorage.RemoveAsync(keyAccounts);
		}

		public static async Task InitAccounts()
		{
			var accounts = Accounts;
			accounts.Clear();

			var value = await SecureStorage.GetAsync(keyAccounts);
			if (!string.IsNullOrEmpty(value)) {
				var split = value.Split(new char[] { '|' });
				foreach (var str in split) {
					var account = new Account();
					if (account.Decode(str)) {
						accounts.Add(account);
					}
				}
			}
		}

		public static async Task SaveAccounts()
		{
			var accounts = Accounts;
			string value = string.Empty;
			
			foreach (var account in accounts)
			{
				if (value.Length > 0) {
					value += "|";
				}
				value += account.Encode();
			}

			await SecureStorage.SetAsync(keyAccounts, value);
		}

		public static async Task AddAccount(Account account)
		{
			Accounts.Add(account);
			await SaveAccounts();
		}

		public static Account GetAccount(string username, string password)
		{
			foreach (var account in Accounts) {
				if (account.Username == username) {
					if (account.Password == (password ?? string.Empty))
						return account;
					break;
				}
			}
			return null;
		}

		public async Task<Exception> TryLogin(string apiKey, string baseId)
		{
			Logout();

			MainBase = new MainBase(apiKey, baseId);
			try {
				await InitTripBases();
				return null;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				MainBase = null;
				return new LoginException(exc);
			}
		}

		//public async Task<Exception> Login(Account account)
		//{
		//	return await TryLogin(account.ApiKey, account.BaseId);
		//}


		public async Task<Exception> Login(string username, string password)
		{
			var account = GetAccount(username, password);
			if (account == null) {
				return new LoginException("Invalid Username or Password");
			}

			return await TryLogin(account.ApiKey, account.BaseId);
		}

		public void Logout()
		{
			MainBase = null;
			//AirClient = null;
		}

		public async Task InitTripBases()
		{
			var data = Data;
			data.Clear();

			var records = await MainBase.ListBaseRecords();

			foreach (var rec in records)
			{
				if (rec.Year <= 0)
					continue;

				var tripBase = new TripBase(ApiKey, rec.BaseId, rec.Year, rec.Month);

				if (rec.Month > 0) {
					var yearlyData = GetYearlyTripData(rec.Year);
					yearlyData.AddMonth(rec.Month, tripBase);
				} else {
					var yearlyData = new YearlyTripData(rec.Year, tripBase);
					data.Add(yearlyData);
				}

				Bases.Add(tripBase);
			}
		}

		YearlyTripData GetYearlyTripData(int year, bool create = true)
		{
			var data = Data;
			foreach (var yearlyData in data) {
				if (yearlyData.Year == year)
					return yearlyData;
			}

			if (create) {
				var yearlyData = new YearlyTripData(year);
				data.Add(yearlyData);
				return yearlyData;
			}

			return null;
		}

		public static Exception CheckBaseIsNew(int year, int month, string baseId)
		{
			foreach (var db in Bases) {
				if (db.Year == year) {
					if (db.Month == 0) {
						return new UserError("Database for year {0} already exists");
					}
					if (db.Month == month) {
						string monthStr = new DateTime(year, month, 1).ToString("MMMM yyyy");
						return new UserError("Database for {0} already exists", monthStr);
					}
				}
				if (db.BaseId == baseId) {
					return new UserError("Database with Id \"{0}\" already exists", baseId);
				}
			}

			return null;
		}

		public async Task<Exception> AddBase(int year, int month, TripBase tripBase)
		{
			try {
				var record = new BaseRecord();
				record.Year = year;
				record.Month = month;
				record.BaseId = tripBase.BaseId;
				if (month > 0) {
					record.Notes = new DateTime(year, month, 1).ToString("MMM yyyy");
				} else {
					record.Notes = year.ToString();

				}

				record = await MainBase.AddBaseRecord(record);

				await tripBase.CopyVendors(MainBase);

				if (month > 0) {
					var yearlyData = GetYearlyTripData(year);
					yearlyData.AddMonth(month, tripBase);
				}
				else {
					var yearlyData = new YearlyTripData(year, tripBase);
					Data.Add(yearlyData);
				}

				Bases.Add(tripBase);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return exc;
			}
			return null;
		}
	}

	public class LoginException : DwarfException
	{
		public const string kLoginError = "Can not login";

		public LoginException(string message = kLoginError) : base(message) { }
		public LoginException(Exception innerException) : base(kLoginError, innerException) { }
	}
}
