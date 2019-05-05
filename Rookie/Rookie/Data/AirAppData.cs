﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Rookie.Models;
using Dwares.Rookie.Bases;


namespace Dwares.Rookie.Data
{
	public class AirAppData : PropertyNotifier, IAppData
	{
		//static ClassRef @class = new ClassRef(typeof(AppData));

		public List<TripBase> Bases { get; } = new List<TripBase>();

		public string ApiKey { get; set; }
		public string BaseId { get; set; }

		public MainBase MainBase { get; set; }
		public TripBase TripBase { get; set; }
		public TripBase TemplateBase { get; set; }

		public AirAppData()
		{
			//Debug.EnableTracing(@class);
		}

		public AirAppData(string apiKey, string baseId)
		{
			//Debug.EnableTracing(@class);

			ApiKey = Guard.ArgumentNotEmpty(apiKey, nameof(apiKey));
			BaseId = Guard.ArgumentNotEmpty(baseId, nameof(baseId));
		}

		public async Task Initialize()
		{
			MainBase = new MainBase(ApiKey, BaseId);
			await MainBase.Initialize();
		}

		public async Task Initialize(string apiKey, string baseId)
		{
			ApiKey = Guard.ArgumentNotEmpty(apiKey, nameof(apiKey));
			BaseId = Guard.ArgumentNotEmpty(baseId, nameof(baseId));
			MainBase = new MainBase(ApiKey, BaseId);
			await MainBase.Initialize();
		}

		public void Reset()
		{
			MainBase = null;
			TripBase = null;
			TemplateBase = null;
		}

		public async Task<BaseRecord[]> GetBases()
		{
			var list = await MainBase.BasesTable.ListRecords();
			var records = list.Records;

			foreach (var rec in records) {
				if (rec.Year == 0 && rec.Month == 1) {
					Debug.Assert(TemplateBase == null);
					TemplateBase = new TripBase(MainBase.ApiKey, rec.BaseId, 0, 0);
					break;
				}

				if (rec.Year > 0) {
					var tripBase = new TripBase(MainBase.ApiKey, rec.BaseId, rec.Year, rec.Month);
					Bases.Add(tripBase);
				}
			}

			return records;
		}

		public async Task<BaseRecord> AddBase(TripBase tripBase, int year, int month, string notes)
		{
			var record = new BaseRecord {
				BaseId = tripBase.BaseId,
				Year = year,
				Month = month,
				Notes = notes
			};

			record = await MainBase.BasesTable.CreateRecord(record);
			return record;
		}

		public Exception CheckBaseIsNew(int year, int month, string baseId)
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
				if (db.BaseId.ToString() == baseId) {
					return new UserError("Database with Id \"{0}\" already exists", baseId);
				}
			}

			return null;
		}

		public async Task<IWorkPeriod> GetLastPeriod(string lastPeriodId)
		{
			PeriodRecord lastPeriod = null;
			if (TripBase != null) {
				try {
					if (string.IsNullOrEmpty(lastPeriodId)) {
						lastPeriod = await TripBase.GetLastCreatedPeriod();
					} else {
						lastPeriod = await TripBase.GetPeriod(lastPeriodId);
					}
				}
				catch (Exception exc) {
					Debug.ExceptionCaught(exc);
				}
			}
			return lastPeriod;
		}

		public Task<PeriodRecord[]> GetPeriodsForDate(DateOnly date)
			=> TripBase.PeriodsTable.GetPeriodsForDate(date);
		
		public async Task<IWorkPeriod> StartPeriod(DateTime startTime, int startMileage) 
			=> await TripBase.PeriodsTable.StartPeriod(startTime, startMileage);
		
		public async Task<IWorkPeriod> FinishPeriod(IWorkPeriod period, DateTime endTime, int endMileage)
		{
			if (period is PeriodRecord record) {
				return await TripBase.PeriodsTable.FinishPeriod(record, endTime, endMileage);
			} else {
				return await TripBase.PeriodsTable.FinishPeriod(period.Id, endTime, endMileage);
			}
		}

		public Task<PeriodRecord> UpdatePeriodEarnings(PeriodRecord period)
			=> TripBase.PeriodsTable.UpdateRecord(period, PeriodRecord.CASH, PeriodRecord.CREDIT, PeriodRecord.EXPENSES);

		public Task<PeriodRecord> UpdatePeriodLease(PeriodRecord period)
			=> TripBase.PeriodsTable.UpdateRecord(period, PeriodRecord.LEASE);

		//public Task<TripRecord> AddTrip(TripRecord record, params string[] fieldNames)
		//	=> TripBase.TripsTable.CreateRecord(record, fieldNames);

		public Task<TripRecord> AddTrip(TripRecord record)
			=> TripBase.TripsTable.CreateRecord(record);

		public Task<LeaseRecord> AddLease(LeaseRecord record)
			=> TripBase.LeaseTable.CreateRecord(record);

		public Task PutProperty(string key, object value) => MainBase.PropertiiesTable.PutRecord(key, value?.ToString());
		public string GetStringProperty(string key) => MainBase?.PropertiiesTable.GetString(key);
		public int? GetIntegerProperty(string key) => MainBase?.PropertiiesTable.GetInteger(key);
		public bool? GetBooleanProperty(string key) => MainBase?.PropertiiesTable.GetBoolean(key);
	}
}