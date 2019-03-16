using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

		static string PeriodsForDateFormula(string fieldName, DateOnly date)
		{
			return $"AND(YEAR({{{fieldName}}}) = {date.Year}, MONTH({{{fieldName}}}) = {date.Month}, DAY({{{fieldName}}}) = {date.Day}";
		}

		public async Task<PeriodRecord[]> GetPeriodsForDate(DateOnly date)
		{
			//string formula = PeriodsForDateFormula(PeriodRecord.START_TIME, date);
			var field = PeriodRecord.START_TIME;
			string formula = $"AND(YEAR({{{field}}}) = 2019, MONTH({{{field}}}) = 3, DAY({{{field}}}) = 10)";
			var list = await FilterRecords(formula);
			return list.Records;
		}
	}

	public class PeriodRecord : AirRecord
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

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int StartMileage{ get; set; }
		public int EndMileage { get; set; }
		public decimal Distance { get; set; }
		public decimal Cash { get; set; }
		public decimal Credit { get; set; }
		public decimal Lease { get; set; }
		public decimal Gas { get; set; }
		public decimal Expenses { get; set; }

		public override void CopyFieldsToProperties()
		{
			StartTime = GetField<DateTime>(START_TIME);
			EndTime = GetField<DateTime>(END_TIME);
			StartMileage = GetField<int>(START_MILEAGE);
			EndMileage = GetField<int>(END_MILEAGE);
			Distance = GetField<decimal>(DISTANCE);
			Cash = GetField<decimal>(CASH);
			Credit = GetField<decimal>(CREDIT);
			Lease = GetField<decimal>(LEASE);
			Gas = GetField<decimal>(GAS);
			Expenses = GetField<decimal>(EXPENSES);
		}

		public override void CopyPropertiesToFields(IEnumerable<string> fieldNames)
		{
			SetField(START_TIME, StartTime, fieldNames);
			SetField(END_TIME, EndTime, fieldNames);
			SetField(START_MILEAGE, StartMileage, fieldNames);
			SetField(END_MILEAGE, EndMileage, fieldNames);
			SetField(DISTANCE, Distance, fieldNames);
			SetField(CASH, Cash, fieldNames);
			SetField(CREDIT, Credit, fieldNames);
			SetField(LEASE, Lease, fieldNames);
			SetField(GAS, Gas, fieldNames);
			SetField(EXPENSES, Expenses, fieldNames);
		}

	}
}
