using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Drudge.Airtable;
using Lost.Models;


namespace Lost.Storage.Air
{
	public class PropertiiesTable : AirTable<PropertyRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PropertiiesTable));

		public const string SHIFT_RECORD_ID = "Shift RecordId";
		//public const string SHIFT_STATE = "Shift State";
		public const string LAST_MILEAGE = "Last Mileage";

		Dictionary<string, PropertyRecord> records;

		public PropertiiesTable(AirBase airBase) :
			base(airBase, "Properties")
		{
			//Debug.EnableTracing(@class);
			records = new Dictionary<string, PropertyRecord>();
		}

		public override async Task Initialize()
		{
			var list = await ListRecords();

			foreach (var record in list) {
				if (string.IsNullOrEmpty(record.Name)) {
					await DeleteRecord(record.Id);
				} else {
					records.Add(record.Name, record);
				}
			}
		}

		public string GetValue(string name)
		{
			PropertyRecord record;
			if (records.TryGetValue(name, out record))
				return record.Value;
			return null;
		}

		public async Task PutValue(string name, string value)
		{
			PropertyRecord record;
			if (records.TryGetValue(name, out record)) {
				record.Value = value;
				await UpdateRecord(record);
			} else {
				record = new PropertyRecord {
					Name = name,
					Value = value
				};
				record = await CreateRecord(record);
				records.Add(name, record);
			}
		}

		public string GetShiftRecordId()
		{
			var value = GetValue(SHIFT_RECORD_ID);
			return value;
		}

		//public ShiftState? GetShiftState()
		//{
		//	var value = GetValue(SHIFT_STATE);
		//	if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int state) && 
		//		state >= (int)ShiftState.None && state <= (int)ShiftState.Ended)
		//	{
		//		return (ShiftState)state;
		//	} else {
		//		return null;
		//	}

		//}

		public async Task PutShiftRecordId(string recordId)
		{
			await PutValue(SHIFT_RECORD_ID, recordId);
		}

		//public async Task PutShiftState(ShiftState state)
		//{
		//	await PutValue(SHIFT_STATE, ((int)state).ToString());
		//}
	}

	public class PropertyRecord : AirRecord
	{
		const string NAME = "Name";
		const string VALUE = "Value";

		public string Name {
			get => GetField<string>(NAME);
			set => SetField(NAME, value);
		}

		public string Value {
			get => GetField<string>(VALUE);
			set => SetField(VALUE, value);
		}

	}
}
