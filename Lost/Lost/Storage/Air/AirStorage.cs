using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using Lost.Models;
using System.Collections.Generic;

namespace Lost.Storage.Air
{
	public class AirStorage : AirClient, IAppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AirStorage));
		const string ApiKey = "keyn9n03pU21UkxTg";
		const string MainBaseId = "appAweiOzKosFPxht";

		public AirStorage()
		{
			//Debug.EnableTracing(@class);

			AirClient.Instance = this;
		}

		public async Task<Shift> Initialize()
		{
			DataBase = new DataBase(ApiKey, MainBaseId);
			await DataBase.Initialize();

			var shiftRecId = DataBase.PropertiiesTable.GetShiftRecordId();
			if (string.IsNullOrEmpty(shiftRecId))
				return null;

			try {
				ShiftRecord = await DataBase.DailyTable.GetRecord(shiftRecId);
				if (ShiftRecord == null)
					return null;
			} catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}

			Shift = new Shift(ShiftRecord.Date);

			var shiftState = ShiftRecord.ShiftState;
			if (shiftState >= ShiftState.Enroute)
				Shift.StartShift(ShiftRecord.ShiftStart, ShiftRecord.StartMileage);
			if (shiftState >= ShiftState.Working)
				Shift.FirtPickup(ShiftRecord.FirstPickup);
			if (shiftState >= ShiftState.Finishing)
				Shift.LastDropoff(ShiftRecord.LastDropoff);
			if (shiftState == ShiftState.Ended)
				Shift.EndShift(ShiftRecord.ShiftEnd, ShiftRecord.EndMileage);

			return Shift;
		}

		public DataBase DataBase { get; private set; }
		public ShiftRecord ShiftRecord { get; private set; }
		public Shift Shift { get; private set; }

		public async Task NewShift(Shift shift)
		{
			Shift = shift;
			ShiftRecord = new ShiftRecord {
				Date = Shift.Date
			};

			ShiftRecord = await DataBase.DailyTable.CreateRecord(ShiftRecord, ShiftRecord.DATE);
			await DataBase.PropertiiesTable.PutShiftRecordId(ShiftRecord.Id);
		}

		public async Task StartShift()
		{
			ShiftRecord.ShiftStart = (DateTime)Shift.ShiftStartTime;
			ShiftRecord.StartMileage = (int)Shift.StartMileage;
			ShiftRecord.ShiftState = Shift.State;

			ShiftRecord = await DataBase.DailyTable.UpdateRecord(ShiftRecord, ShiftRecord.SHIFT_STATE, ShiftRecord.SHIFT_START, ShiftRecord.START_MILEAGE);
		}

		public async Task FirstPickup()
		{
			ShiftRecord.FirstPickup = (DateTime)Shift.FirstPickupTime;
			ShiftRecord.ShiftState = Shift.State;

			ShiftRecord = await DataBase.DailyTable.UpdateRecord(ShiftRecord, ShiftRecord.SHIFT_STATE, ShiftRecord.FIRST_PICKUP);
		}

		public async Task LastDropoff()
		{
			ShiftRecord.LastDropoff = (DateTime)Shift.LastDropoffTime;
			ShiftRecord.ShiftState = Shift.State;

			ShiftRecord = await DataBase.DailyTable.UpdateRecord(ShiftRecord, ShiftRecord.SHIFT_STATE, ShiftRecord.LAST_DROPOFF);
		}

		public async Task EndShift()
		{
			ShiftRecord.ShiftEnd = (DateTime)Shift.ShiftEndTime;
			ShiftRecord.EndMileage = (int)Shift.EndMileage;
			ShiftRecord.ShiftState = Shift.State;

			ShiftRecord = await DataBase.DailyTable.UpdateRecord(ShiftRecord, ShiftRecord.SHIFT_STATE, ShiftRecord.SHIFT_END, ShiftRecord.END_MILEAGE);
		}

		public async Task ListShifts(ICollection<PeriodInfo> list)
		{
			var result = await DataBase.DailyTable.ListRecords(0, ShiftRecord.DATE);

			foreach (var record in result) {
				var info = new PeriodInfo {
					Period = new ShiftPeriod(record.Date)
				};

				if (record.ShiftState >= ShiftState.Finishing)
					info.WorkTime = (int)Math.Round((record.LastDropoff - record.ShiftStart).TotalMinutes);
				
				if (record.ShiftState == ShiftState.Ended) {
					info.FullTime = (int)Math.Round((record.ShiftEnd - record.ShiftStart).TotalMinutes);
					info.Mileage = record.EndMileage - record.StartMileage;
				}


				list.Add(info);
			}
		}
	}
}
