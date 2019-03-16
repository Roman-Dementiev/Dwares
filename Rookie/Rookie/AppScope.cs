using System;
using System.Collections.Generic;
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
		const string propLastPaidDate = "LastPaidDate";

		public static AppScope Instance { get; private set; }

		public AppScope() : base(null)
		{
			Debug.AssertIsNull(Instance);
			Instance = this;
		}

		public ObservableCollection<TripBase> Bases { get; } = new ObservableCollection<TripBase>();
		public ObservableCollection<Account> Accounts { get; } = new ObservableCollection<Account>();
		public ObservableCollection<YearlyTripData> TripData { get; } = new ObservableCollection<YearlyTripData>();

		Account account;
		public Account Account {
			get => account;
			set => SetProperty(ref account, value);
		}

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

		Earnings earnings;
		public Earnings Earnings {
			get => earnings;
			set => SetProperty(ref earnings, value);
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
			LastPeriod = null;
			var lastPeriodId = GetStringProperty(propLastPeriod);
			if (!string.IsNullOrEmpty(lastPeriodId)) {
				try {
					LastPeriod = await TripBase.GetPeriod(lastPeriodId);
				}
				catch (Exception exc) {
					Debug.ExceptionCaught(exc);
				}
			}
			if (LastPeriod == null) {
				LastPeriod = await TripBase.GetLastCreatedPeriod();
			}

			if (LastPeriod != null) {
				IsWorking = GetBooleanProperty(propIsWorking);
				if (IsWorking) {
					Earnings = new Earnings(LastPeriod);
				}
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

			var error = await TryLogin(username, password, account.ApiKey, account.BaseId, keepLoggedIn);
			if (error == null) {
				Account = account;
			}
			return error;
		}

		public async Task Logout()
		{
			Account = null;
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
				Earnings = new Earnings(workPeriod);

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
				Earnings = new Earnings();

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
			Debug.Assert(MainBase != null);
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

		MonthlyTripData GetMonthlyTripData(int year, int month)
		{
			var yearlyData = GetYearlyTripData(year, false);
			return yearlyData?.GetMonth(month);
		}

		public void GetUpcomingMonth(out int year, out int month)
		{
			var today = DateTime.Today;
			year = today.Year;
			month = today.Month;

			for (;;) {
				var data = GetMonthlyTripData(year, month);
				if (data == null)
					return;

				if (month < 12) {
					month++;
				} else {
					year++;
					month = 1;
				}
			}
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

		public async Task AddTrip(TripRecord record)
		{
			Debug.Assert(IsLoggedIn && IsWorking);

			record = await TripBase.TripsTable.CreateRecord(record);

			LastPeriod.Cash += record.Cash;
			LastPeriod.Credit += record.Credit;
			LastPeriod.Expenses += record.Expences;
			LastPeriod = await TripBase.PeriodsTable.UpdateRecord(LastPeriod, PeriodRecord.CASH, PeriodRecord.CREDIT, PeriodRecord.EXPENSES);

			Earnings = new Earnings(LastPeriod);
		}

		public async Task AddLease(LeaseRecord record)
		{
			Debug.Assert(IsLoggedIn);

			record = await TripBase.LeaseTable.CreateRecord(record);
			await PutDateOnlyProperty(propLastPaidDate, record.Date);
			
			var period = await GetPeriod(record.Date);
			if (period != null) {
				period.Lease += record.Amount;
				period = await TripBase.PeriodsTable.UpdateRecord(period, PeriodRecord.LEASE);

				if (period.Id == LastPeriod?.Id) {
					Earnings = new Earnings(period);
				}
			}
		}

		public async Task<PeriodRecord> GetPeriod(DateOnly date)
		{
			Debug.Assert(IsLoggedIn);

			if (LastPeriod != null && date.Equals((DateOnly)LastPeriod.StartTime)) {
				return LastPeriod;
			}

			var periods = await TripBase.PeriodsTable.GetPeriodsForDate(date);
			if (periods != null && periods.Length > 0) {
				var lastPeriod = periods[0];
				for (int i = 1; i < periods.Length; i++) {
					var period = periods[i];
					if (period.StartTime > lastPeriod.StartTime) {
						lastPeriod = period;
					}
				}
				return lastPeriod;
			}

			return null;
		}

		public async Task<bool> PutProperty(string key, object value)
		{
			if (MainBase != null) {
				try {
					await MainBase.PropertiiesTable.PutValue(key, value);
					return true;
				} catch (Exception exc) {
					Debug.ExceptionCaught(exc);
				}
			}
			return false;
		}

		public Task PutStringProperty(string key, string value) => PutProperty(key, value);
		public Task PutIntegerProperty(string key, int value) => PutProperty(key, value);
		public Task PutBooleanProperty(string key, bool value) => PutProperty(key, value);
		public Task PutDateOnlyProperty(string key, DateOnly value) => PutStringProperty(key, value.ToString());

		public string GetStringProperty(string key, string defaultValue = null)
		{
			if (MainBase != null) {
				var value = MainBase.PropertiiesTable.GetString(key);
				return value;
			}
			return defaultValue;
		}

		public int GetIntegerProperty(string key, int defaultValue = 0)
		{
			if (MainBase != null) {
				var value = MainBase.PropertiiesTable.GetInteger(key);
				if (value != null)
					return (int)value;
			}
			return defaultValue;
		}

		public bool GetBooleanProperty(string key, bool defaultValue = false)
		{
			if (MainBase != null) {
				var value = MainBase.PropertiiesTable.GetBoolean(key);
				if (value != null)
					return (bool)value;
			}
			return defaultValue;
		}

		public DateOnly? GetDateOnlyProperty(string key, DateOnly? defaultValue = null)
		{
			if (MainBase != null) {
				var str = MainBase.PropertiiesTable.GetString(key);
				if (str != null) {
					DateOnly value;
					if (DateOnly.TryParse(str, out value))
						return value;
				}
			}
			return defaultValue;
		}

		public string GetUnpaidDays(int maxDays = 3)
		{
			var lastPaid = GetDateOnlyProperty(propLastPaidDate);
			if (lastPaid == null)
				return string.Empty;

			var today = DateOnly.Today;
			today = today.NextDay().NextDay().NextDay().NextDay().NextDay().NextDay().NextDay().NextDay().NextDay();

			var list = new List<DateOnly>();
			bool tooMany = false;
			int count = 0;
			for (var date = (DateOnly)lastPaid; date.CompareTo(today) < 0; date = date.NextDay()) {
				var dayOfWeek = date.DayOfWeek;
				if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
					continue;

				if (count < maxDays) {
					list.Add(date);
				} else {
					tooMany = true;
				}
				count++;
			}

			if (tooMany) {
				return $"{count} days";
			} else {
				string str = string.Empty;
				foreach (var date in list) {
					var dayStr = date.DateTime.ToString("ddd, MMM d");
					if (str.Length > 0)
						str += "\n";
					str += dayStr;
				}
				return str;
			}
		}

		public static int LastMileage => Instance.GetIntegerProperty(propMileage);

		public static string ApiKey => Instance.MainBase?.ApiKey;
		public static bool IsLoggedIn => Instance.Account != null;
	}

	public struct Earnings
	{
		public Earnings(PeriodRecord record)
		{
			if (record != null) {
				Income = record.Cash;
				Credit = record.Credit;
				Expenses = record.Lease + record.Gas + record.Expenses;
			} else {
				Income  = Credit = Expenses = 0;
			}
		}

		public decimal Income { get; set; }
		public decimal Credit { get; set; }
		public decimal Expenses { get; set; }
		public decimal Total => Income + Credit - Expenses;
	}

	public class LoginException : DwarfException
	{
		public const string kLoginError = "Can not login";

		public LoginException(string message = kLoginError) : base(message) { }
		public LoginException(Exception innerException) : base(kLoginError, innerException) { }
	}
}
