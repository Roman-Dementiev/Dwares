using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Services;
using Dwares.Rookie.Models;
using Dwares.Rookie.Data;
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

		//public ObservableCollection<TripBase> Bases { get; } = new ObservableCollection<TripBase>();
		public ObservableCollection<Account> Accounts { get; } = new ObservableCollection<Account>();
		public ObservableCollection<YearlyTripData> TripData { get; } = new ObservableCollection<YearlyTripData>();

		public AirAppData AppData { get; }

		public AppScope() : base(null)
		{
			Debug.AssertIsNull(Instance);
			Instance = this;
			AppData = new AirAppData();
		}

		Account account;
		public Account Account {
			get => account;
			set => SetProperty(ref account, value);
		}

		IWorkPeriod lastPeriod;
		public IWorkPeriod LastPeriod {
			get => lastPeriod;
			set => SetProperty(ref lastPeriod, value);
		}

		bool isLoggedIn;
		public bool IsLoggedIn {
			get => isLoggedIn;
			set => SetProperty(ref isLoggedIn, value);
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

		//public bool HasTripBase {
		//	get => AppData.TripBase != null;
		//}

		public static string Driver { get; private set; }

		public async Task Initialize(bool reset = false)
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

		Account GetAccount(string username, string password, bool usePassword)
		{
			foreach (var account in Accounts) {
				if (account.Username == username) {
					if (!usePassword || account.Password == password ||
						(string.IsNullOrEmpty(password) && string.IsNullOrEmpty(account.Password))
						)
						return account;

					break;
				}
			}
			return null;
		}

		public Account GetAccount(string username, string password) => GetAccount(username, password, true);
		public Account GetAccount(string username) => GetAccount(username, null, false);
		public bool HasAccount(string username) => GetAccount(username, null, false) != null;


		public async Task GetLastPeriod()
		{
			LastPeriod = await AppData.GetLastPeriod(LastPeriodId);

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

			await AppData.Initialize(apiKey, baseId);
			try {
				await InitTripBases();
				await GetLastPeriod();

				Driver = username;
				if (keepLoggedIn) {
					await SecureStorage.SetAsync(keyDriver, Driver + '|' + password);
				} else {
					await SecureStorage.SetAsync(keyDriver, Driver);
				}
				IsLoggedIn = true;
				return null;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				AppData.Reset();
				return new LoginException(exc);
			}
		}

		public async Task<Exception> Login(string username, string password, bool keepLoggedIn)
		{
			var account = GetAccount(username, password);
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
			IsLoggedIn = false;
			Account = null;
			AppData.Reset();
			//await SecureStorage.SetAsync(keyDriver, Driver);
		}


		public async Task<Exception> GoToWork(DateTime datetime, int mileage)
		{
			Debug.Assert(!IsWorking);
			if (IsWorking)
				return new ProgramError("Already working");

			try {
				var workPeriod = await AppData.StartPeriod(datetime, mileage);
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

		public async Task<Exception> GoOffWork(DateTime datetime, int mileage)
		{
			Debug.Assert(IsWorking);
			if (!IsWorking)
				return new ProgramError("Not working");

			try {
				await AppData.FinishPeriod(LastPeriod, datetime, mileage);
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
			if (LastPeriod != null) {
				await AppData.PutProperty(propLastPeriod, LastPeriod.Id);
			} else {
				//properties.Remove(propLastPeriod);
				await AppData.PutProperty(propLastPeriod, string.Empty);
			}
			await AppData.PutProperty(propIsWorking, IsWorking);
			await AppData.PutProperty(propMileage, mileage);
		}

		public async Task InitTripBases()
		{
			TripData.Clear();

			var records = await AppData.GetBases();

			foreach (var rec in records)
			{
				if (rec.Year > 0) {
					var tripBase = new TripBase(ApiKey, rec.BaseId, rec.Year, rec.Month);
					AddTripBase(tripBase);
				}
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

		public Exception CheckBaseIsNew(int year, int month, string baseId)
			=> AppData.CheckBaseIsNew(year, month, baseId);

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

				await AppData.AddBase(tripBase, year, month, notes);
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
					Debug.AssertIsNull(AppData.TripBase);
					AppData.TripBase = tripBase;
				}
			}
			else {
				var yearlyData = new YearlyTripData(year, tripBase);
				TripData.Add(yearlyData);
				if (year == today.Year) {
					Debug.AssertIsNull(AppData.TripBase);
					AppData.TripBase = tripBase;
				}
			}
		}

		public async Task AddTrip(TripRecord record)
		{
			Debug.Assert(IsLoggedIn && IsWorking);

			record = await AppData.AddTrip(record);

			LastPeriod.Cash += record.Cash;
			LastPeriod.Credit += record.Credit;
			LastPeriod.Expenses += record.Expences;
			LastPeriod = await AppData.UpdatePeriodEarnings(LastPeriod as PeriodRecord);

			Earnings = new Earnings(LastPeriod);
		}

		public async Task AddLease(LeaseRecord record)
		{
			Debug.Assert(IsLoggedIn);

			record = await AppData.AddLease(record);
			await PutDateOnlyProperty(propLastPaidDate, record.Date);
			
			var period = await GetPeriod(record.Date);
			if (period != null) {
				period.Lease += record.Amount;
				period = await AppData.UpdatePeriodLease(period);

				if (period.Id == LastPeriod?.Id) {
					Earnings = new Earnings(period);
				}
			}
		}

		public async Task<PeriodRecord> GetPeriod(DateOnly date)
		{
			Debug.Assert(IsLoggedIn);

			if (LastPeriod != null && date.Equals((DateOnly)LastPeriod.StartTime)) {
				return LastPeriod as PeriodRecord;
			}

			var periods = await AppData.GetPeriodsForDate(date);
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
			try {
				await AppData.PutProperty(key, value);
				return true;
			} catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}
			return false;
		}

		public Task PutStringProperty(string key, string value) => PutProperty(key, value);
		public Task PutIntegerProperty(string key, int value) => PutProperty(key, value);
		public Task PutBooleanProperty(string key, bool value) => PutProperty(key, value);
		public Task PutDateOnlyProperty(string key, DateOnly value) => PutStringProperty(key, value.ToString());

		public string GetStringProperty(string key, string defaultValue = null)
		{
			var value = AppData.GetStringProperty(key);
			return value ?? defaultValue;
		}

		public int GetIntegerProperty(string key, int defaultValue = 0)
		{
			var value = AppData.GetIntegerProperty(key);
			return value ?? defaultValue;
		}

		public bool GetBooleanProperty(string key, bool defaultValue = false)
		{
			var value = AppData.GetBooleanProperty(key);
			return value ?? defaultValue;
		}

		public DateOnly? GetDateOnlyProperty(string key, DateOnly? defaultValue = null)
		{
			var str = AppData.GetStringProperty(key);
			if (str != null) {
				DateOnly value;
				if (DateOnly.TryParse(str, out value))
					return value;
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
		public static string LastPeriodId => Instance.GetStringProperty(propLastPeriod);

		public static string ApiKey => Instance.AppData.MainBase?.ApiKey;
	}


	public class LoginException : DwarfException
	{
		public const string kLoginError = "Can not login";

		public LoginException(string message = kLoginError) : base(message) { }
		public LoginException(Exception innerException) : base(kLoginError, innerException) { }
	}
}
