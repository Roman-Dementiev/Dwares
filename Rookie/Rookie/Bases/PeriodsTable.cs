using System;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf;


namespace Dwares.Rookie.Bases
{
	public class PeriodsTable : AirTable<PeriodRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PeriodsTable));

		public PeriodsTable(AirBase airBase) :
			base(airBase, "Daily")
		{
			//Debug.EnableTracing(@class);
		}

		public async Task<PeriodRecord> StartPeriod(DateTime startTime, int startMileage)
		{
			var record = new PeriodRecord {
				StartTime = startTime,
				StartMileage = startMileage,
			};
			record.CopyPropertiesToFields();

			record =  await CreateRecord(record, PeriodRecord.START_TIME, PeriodRecord.START_MILEAGE);
			return record;
		}

		public async Task<PeriodRecord> FinishPeriod(PeriodRecord record, DateTime endTime, int endMileage)
		{
			record.EndTime = endTime;
			record.EndMileage = endMileage;
			record.CopyPropertiesToFields();

			record =  await UpdateRecord(record, PeriodRecord.END_TIME, PeriodRecord.END_MILEAGE);
			return record;
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

		public override void CopyPropertiesToFields()
		{
			Fields[START_TIME] = StartTime;
			Fields[END_TIME] = EndTime;
			Fields[START_MILEAGE] = StartMileage;
			Fields[END_MILEAGE] = EndMileage;
			Fields[DISTANCE] = Distance;
			Fields[CASH] = Cash;
			Fields[CREDIT] = Credit;
			Fields[LEASE] = Lease;
			Fields[GAS] = Gas;
			Fields[EXPENSES] = Expenses;
		}

	}
}
