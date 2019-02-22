using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Rookie.Airtable;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf;


namespace Dwares.Rookie.Bases
{
	public class PropertiiesTable : AirTable<PropertyRecord>, IPropContainer<string>
	{
		//static ClassRef @class = new ClassRef(typeof(PropertirdTable));

		Dictionary<string, PropertyRecord> records;
		StringProps props;

		public PropertiiesTable(AirBase airBase) :
			base(airBase, "Properties")
		{
			//Debug.EnableTracing(@class);
			records = new Dictionary<string, PropertyRecord>();
			props = new StringProps(this);
		}

		public override async Task Initialize()
		{
			var list = await ListRecords();

			foreach (var record in list.Records) {
				records.Add(record.Key, record);
			}
		}

		//public PropertyRecord GetRecord(string key)
		//{
		//	if (properties.ContainsKey(key))
		//		return properties[key];

		//	return null;
		//}

		public string GetString(string key)
		{
			//var record = GetRecord(key);
			//if (record != null)
			//	return record.Text;

			string value;
			if (props.GetString(key, out value))
				return value;
			return null;
		}

		public int? GetInteger(string key)
		{
			//var record = GetRecord(key);
			//if (record != null)
			//	return record.Integer;

			int value;
			if (props.GetInteger(key, out value))
				return value;
			return null;
		}

		public async Task PutRecord(string key, string value)
		{
			PropertyRecord newRecord;

			if (records.ContainsKey(key)) {
				var record = records[key];
				record.Value = value ?? string.Empty;
				record.CopyPropertiesToFields();

				newRecord = await UpdateRecord(record);
			} else {
				var record = new PropertyRecord {
					Key = key,
					Value = value ?? string.Empty
				};
				record.CopyPropertiesToFields();

				newRecord = await CreateRecord(record);
			}

			records[key] = newRecord;
		}

		public Task PutValue(string key, object value) => PutRecord(key, value?.ToString());

		// IPropContainer<string>
		public bool ContainsKey(string key)
		{
			return records.ContainsKey(key);
		}

		public bool GetStored(string key, out string stored)
		{
			if (records.ContainsKey(key)) {
				stored = records[key].Value;
				return true;
			} else {
				stored = null;
				return false;
			}
		}

		public async void SetStored(string key, string stored)
		{
			await PutRecord(key, stored);
		}

		public async void Remove(string key)
		{
			if (records.ContainsKey(key)) {
				var record = records[key];
				await DeleteRecord(record.Id);
				records.Remove(key);
			}
		}

	}


	public class PropertyRecord : AirRecord
	{
		const string KEY = "Key";
		const string VALUE = "Value";

		public string Key { get; set; }
		public string Value { get; set; }

		public override void CopyFieldsToProperties()
		{
			Key = GetField<string>(KEY);
			Value = GetField<string>(VALUE);
		}

		public override void CopyPropertiesToFields()
		{
			Fields[KEY] = Key;
			Fields[VALUE] = Value;
		}
	}
}
