using System;
using System.Threading.Tasks;
using Dwares.Dwarf.Toolkit;
using Dwares.Rookie.Models;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


namespace Dwares.Rookie.Bases
{
	public class PeriodsTable : AirTable<PeriodRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PeriodsTable));

		public PeriodsTable(AirBase airBase) :
			base(airBase, "Periods")
		{
			//Debug.EnableTracing(@class);
		}

		public async Task<PeriodRecord> StartPeriod(DateTime startTime, int startMileage)
		{
			var record = new PeriodRecord {
				StartTime = startTime,
				StartMileage = startMileage,
			};

			record =  await CreateRecord(record, PeriodRecord.START_TIME, PeriodRecord.START_MILEAGE);
			return record;
		}

		public async Task<PeriodRecord> FinishPeriod(PeriodRecord record, DateTime endTime, int endMileage)
		{
			record.EndTime = endTime;
			record.EndMileage = endMileage;

			record =  await UpdateRecord(record, PeriodRecord.END_TIME, PeriodRecord.END_MILEAGE);
			return record;
		}

		public async Task<PeriodRecord> FinishPeriod(string periodId, DateTime endTime, int endMileage)
		{
			var record = await GetRecord(periodId);
			if (record != null) {
				await FinishPeriod(record, endTime, endMileage);
			}
			return record;
		}

		static string PeriodsForDateFormula(string fieldName, DateOnly date)
			=> $"AND(YEAR({{{fieldName}}}) = {date.Year}, MONTH({{{fieldName}}}) = {date.Month}, DAY({{{fieldName}}}) = {date.Day}";

		public async Task<PeriodRecord[]> GetPeriodsForDate(DateOnly date)
		{
			var formula = PeriodsForDateFormula(PeriodRecord.START_TIME, date);
			var list = await FilterRecords(formula);
			return list.Records;
		}
	}

	public class PeriodRecord : AirRecord, IWorkPeriod
	{
		public const string START_TIME = "Start time";
		public const string START_MILEAGE = "Start mileage";
		public const string END_TIME = "End time";
		public const string END_MILEAGE = "End mileage";
		public const string DISTANCE = "Distance";
		public const string CASH = "Cash";
		public const string CREDIT = "Credit";
		public const string LEASE = "Lease";
		public const string GAS = "Gas";
		public const string EXPENSES = "Expenses";

		public DateTime StartTime {
			get => GetField<DateTime>(START_TIME);
			set => SetField(START_TIME, value);
		}
		
		public DateTime EndTime {
			get => GetField<DateTime>(END_TIME);
			set => SetField(END_TIME, value);
		}

		public int StartMileage {
			get => GetField<int>(START_MILEAGE);
			set => SetField(START_MILEAGE, value);
		}

		public int EndMileage {
			get => GetField<int>(END_MILEAGE);
			set => SetField(END_MILEAGE, value);
		}

		public decimal Distance {
			get => GetField<decimal>(DISTANCE);
			set => SetField(DISTANCE, value);
		}

		public decimal Cash {
			get => GetField<decimal>(CASH);
			set => SetField(CASH, value);
		}

		public decimal Credit {
			get => GetField<decimal>(CREDIT);
			set => SetField(CREDIT, value);
		}

		public decimal Lease {
			get => GetField<decimal>(LEASE);
			set => SetField(LEASE, value);
		}

		public decimal Gas {
			get => GetField<decimal>(GAS);
			set => SetField(GAS, value);
		}

		public decimal Expenses {
			get => GetField<decimal>(EXPENSES);
			set => SetField(EXPENSES, value);
		}

	}
}
