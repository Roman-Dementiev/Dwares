	using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Services;
using Dwares.Rookie.Models;
using Dwares.Rookie.Bases;


namespace Dwares.Rookie
{
	public class AppScope : BindingScope
	{
		const string keyAccounts = "Accounts";
		const string keyDriver = "Driver";

		const string propLastPeriod = "LastPeriod";
		const string propIsWorking = "IsWorking";
		const string propMileage = "Mileage";

		public static AppScope Instance { get; private set; }

		public AppScope() : base(null)
		{
			Debug.AssertIsNull(Instance);
			Instance = this;
		}

		public ObservableCollection<TripBase> Bases { get; } = new ObservableCollection<TripBase>();
		public ObservableCollection<Account> Accounts { get; } = new ObservableCollection<Account>();
		public ObservableCollection<YearlyTripData> TripData { get; } = new ObservableCollection<YearlyTripData>();

		MainBase mainBase;
		public MainBase MainBase {
			get => mainBase;
			set => SetProperty(ref mainBase, value);
		}

		TripBase tripBase;
		public TripBase TripBase {
			get => tripBase;
			set => SetProperty(ref tripBase, value);
		}

		TripBase templateBase;
		public TripBase TemplateBase {
			get => templateBase;
			set => SetProperty(ref templateBase, value);
		}

		PeriodRecord lastPeriod;
		public PeriodRecord LastPeriod {
			get => lastPeriod;
			set => SetProperty(ref lastPeriod, value);
		}

		bool isWorking;
		public bool IsWorking {
			get => isWorking;
			set => SetProperty(ref isWorking, value);
		}

		public static string Driver { get; private set; }

		public async Task Initialize(bool reset)
		{
			if (reset) {
				await ClearAccounts();
			} else {
				await InitAccounts();
			}
		}

		public async Task ClearAccounts()
		{
			await SecureStorage.RemoveAsync(keyAccounts);
		}

		public async Task InitAccounts()
		{
			Accounts.Clear();

			var value = await SecureStorage.GetAsync(keyAccounts);
			if (!string.IsNullOrEmpty(value)) {
				var split = value.Split(new char[] { '|' });
				foreach (var str in split) {
					var account = new Account();
					if (account.Decode(str)) {
						Accounts.Add(account);
					}
				}
			}

			value = await SecureStorage.GetAsync(keyDriver);
			if (!string.IsNullOrEmpty(value)) {
				var split = value.Split(new char[] { '|' });
				Driver = split[0];
				if (split.Length > 1) {
					var password = split[1];
					var err = await Instance.Login(Driver, password, true);
				}
			}
		}

		public async Task SaveAccounts()
		{
			string value = string.Empty;
			
			foreach (var account in Accounts)
			{
				if (value.Length > 0) {
					value += "|";
				}
				value += account.Encode();
			}

			await SecureStorage.SetAsync(keyAccounts, value);
		}

		public async Task AddAccount(Account account)
		{
			Accounts.Add(account);
			await SaveAccounts();
		}

		public Account GetAccount(string username, string password)
		{
			foreach (var account in Accounts) {
				if (account.Username == username) {
					if (password == null || account.Password == password)
						return account;
					break;
				}
			}
			return null;
		}


		public async Task GetLastPeriod()
		{
			var lastPeriodId = MainBase.PropertiiesTable.GetString(propLastPeriod);
			if (string.IsNullOrEmpty(lastPeriodId))
				return;

			try {
				LastPeriod = await TripBase.GetPeriod(lastPeriodId);

				//IsWorking = MainBase.PropertiiesTable.GetBoolean(propIsWorking);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}

			if (LastPeriod != null) {
				IsWorking = MainBase.PropertiiesTable.GetBoolean(propIsWorking) == true;
			}
		}



		public async Task<Exception> TryLogin(string username, string password, string apiKey, string baseId, bool keepLoggedIn)
		{
			await Logout();

			MainBase = new MainBase(apiKey, baseId);
			await MainBase.Initialize();
			try {
				await InitTripBases();

				if (TripBase != null) {
					await GetLastPeriod();
				}

				Driver = username;
				if (keepLoggedIn) {
					await SecureStorage.SetAsync(keyDriver, Driver + '|' + password);
				} else {
					await SecureStorage.SetAsync(keyDriver, Driver);
				}
				return null;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				MainBase = null;
				TripBase = null;
				TemplateBase = null;
				return new LoginException(exc);
			}
		}


		public async Task<Exception> Login(string username, string password, bool keepLoggedIn)
		{
			var account = GetAccount(username, password ?? string.Empty);
			if (account == null) {
				return new LoginException("Invalid Username or Password");
			}

			return await TryLogin(username, password, account.ApiKey, account.BaseId, keepLoggedIn);
		}

		public async Task Logout()
		{
			MainBase = null;
			TripBase = null;
			TemplateBase = null;
			await SecureStorage.SetAsync(keyDriver, Driver);
		}

		public Task<Exception> GoToWork(TimeSpan time, int mileage)
		{
			var dt = DateTime.Today.Add(time);
			return GoToWork(dt, mileage);
		}

		public Task<Exception> GoOffWork(TimeSpan time, int mileage)
		{
			var dt = DateTime.Today.Add(time);
			return GoOffWork(dt, mileage);
		}

		public async Task<Exception> GoToWork(DateTime time, int mileage)
		{
			Debug.Assert(!IsWorking);
			if (IsWorking)
				return new ProgramError("Already working");

			Debug.AssertNotNull(MainBase);
			Debug.AssertNotNull(TripBase);

			try {
				var workPeriod = await TripBase.PeriodsTable.StartPeriod(time, mileage);
				LastPeriod = workPeriod;
				IsWorking = true;

				await UpdateWorkPeriodProperties(mileage);
				return null;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return exc;
			}
		}

		public async Task<Exception> GoOffWork(DateTime time, int mileage)
		{
			Debug.Assert(IsWorking);
			if (!IsWorking)
				return new ProgramError("Not working");

			Debug.AssertNotNull(MainBase);
			Debug.AssertNotNull(TripBase);

			try {
				await TripBase.PeriodsTable.FinishPeriod(LastPeriod, time, mileage);
				IsWorking = false;

				await UpdateWorkPeriodProperties(mileage);
				return null;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return exc;
			}
		}

		public async Task UpdateWorkPeriodProperties(int mileage)
		{
			var properties = MainBase.PropertiiesTable;

			if (LastPeriod != null) {
				await properties.PutValue(propLastPeriod, LastPeriod.Id);
			} else {
				//properties.Remove(propLastPeriod);
				await properties.PutValue(propLastPeriod, string.Empty);
			}
			await properties.PutValue(propIsWorking, IsWorking);
			await properties.PutValue(propMileage, mileage);

		}

		public async Task InitTripBases()
		{
			TripData.Clear();

			var records = await MainBase.BasesTable.ListBases();

			foreach (var rec in records)
			{
				if (rec.Year <= 0) {
					if (rec.Year == 0 && rec.Month == 1) {
						Debug.Assert(TemplateBase == null);
						TemplateBase = new TripBase(ApiKey, rec.BaseId, 0, 0);
					}
					continue;
				}

				var tripBase = new TripBase(ApiKey, rec.BaseId, rec.Year, rec.Month);
				AddTripBase(tripBase);
			}
		}


		YearlyTripData GetYearlyTripData(int year, bool create = true)
		{
			foreach (var yearlyData in TripData) {
				if (yearlyData.Year == year)
					return yearlyData;
			}

			if (create) {
				var yearlyData = new YearlyTripData(year);
				TripData.Add(yearlyData);
				return yearlyData;
			}

			return null;
		}

		public static Exception CheckBaseIsNew(int year, int month, string baseId)
		{
			foreach (var db in Instance.Bases) {
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

		public async Task<Exception> AddBase(TripBase tripBase, int year, int month, string notes = null)
		{
			try {
				if (notes == null) {
					if (month > 0) {
						notes = new DateTime(year, month, 1).ToString("MMM yyyy");
					} else {
						notes = year.ToString();

					}
				}

				await MainBase.BasesTable.AddRecord(tripBase.BaseId, year, month, notes);
				//await tripBase.CopyVendors(MainBase);

				AddTripBase(tripBase);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return exc;
			}
			return null;
		}

		private void AddTripBase(TripBase tripBase)
		{
			var today = DateTime.Today;
			int year = tripBase.Year;
			int month = tripBase.Month;

			if (month > 0) {
				var yearlyData = GetYearlyTripData(year);
				yearlyData.AddMonth(month, tripBase);
				if (year == today.Year && month == today.Month) {
					Debug.AssertIsNull(TripBase);
					TripBase = tripBase;
				}
			}
			else {
				var yearlyData = new YearlyTripData(year, tripBase);
				TripData.Add(yearlyData);
				if (year == today.Year) {
					Debug.AssertIsNull(TripBase);
					TripBase = tripBase;
				}
			}

			Bases.Add(tripBase);
		}

		public Task AddTrip(TripRecord record)
		{
			return TripBase.TripsTable.CreateRecord(record);
		}

		public int GetIntegerProperty(string key, int defaultValue = 0)
		{
			if (MainBase != null) {
				var lastMileage = MainBase.PropertiiesTable.GetInteger(key);
				if (lastMileage != null)
					return (int)lastMileage;
			}
			return defaultValue;
		}

		public static int LastMileage => Instance.GetIntegerProperty(propMileage);

		public static string ApiKey => Instance.MainBase?.ApiKey;
		public static bool IsLoggedIn => Instance.MainBase != null;
	}

	public class LoginException : DwarfException
	{
		public const string kLoginError = "Can not login";

		public LoginException(string message = kLoginError) : base(message) { }
		public LoginException(Exception innerException) : base(kLoginError, innerException) { }
	}
}
